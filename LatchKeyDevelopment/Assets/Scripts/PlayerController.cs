using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

// This script contains all the player behavior

public class PlayerController : MonoBehaviour
{

	public float moveSpeed;

	private Vector2 startPosition;

	private Vector2 playerMovement;

	// Keeps track of the direction the player is currently facing. Defaults to Down.
	public Vector2 currentDirection;

	// The script that controls the shield. We may be calling methods from this script.
	public ShieldController shieldController;

	public GameObject shield;

	public GameObject enemy;

	public GameObject projectile;

	public GameObject centerPoint;

	public GameObject blocker;

	//public BoxCollider2D blockerCol;

	public float centerRot;

	public Quaternion centerQuaternion;

	private Animator playerAnim;

	public bool shieldDeployed;

	public bool isRolling;

	public bool isWalking;

	public bool canBlock;

	public bool isBlocking;

	public bool isThrowing;

	public static int lifeCount = 7;

	public static int lvlScore;

	public static int totalScore = 0;

	public static float time = 0;

	public static bool lvlComplete;

	public int shieldCounter = 0;

	public bool didJustRoll = false;

	public bool canThrow;

	public bool testThrow;

	public bool testBlock;

	public bool testRoll;

	public bool testRespawn;

	public bool shieldReturn;

	public AudioClip rollSound;

	public AudioClip throwSound;

	public AudioClip blockSound;

	public AudioClip shieldReturnSound;

	private AudioSource source;

	public SoundController soundController;

	private CircleCollider2D cirCol;

	public bool isDead;

	// The current scene.
	private int currentScene;

	void Awake(){
		source = GetComponent<AudioSource> ();
	}

	// Use this for initialization
	void Start ()
	{
		isDead = false;
		cirCol = GetComponent<CircleCollider2D> ();
		cirCol.enabled = true;
		time = 0;

		lvlScore = 0;
		lvlComplete = false;

		// Just in case.
		name = "Player";

		// Sets the currentScene index to the actual current scene.
		currentScene = SceneManager.GetActiveScene ().buildIndex;

		playerAnim = GetComponent<Animator> ();

		// Identifies the prefab we'll be instantiating shield projectiles from.
		shield = (GameObject)Resources.Load ("Shield");

		// currentDirection == Down. Player will always start facing down.
		currentDirection = new Vector2 (0, -1);

		startPosition = transform.position;

		centerPoint = transform.GetChild (0).gameObject;

		soundController = GameObject.Find("MusicManager").GetComponent<SoundController> ();
		//blocker = transform.GetChild (1).gameObject;
		//blockerCol = blocker.GetComponent<BoxCollider2D> ();
	}

	// Update is called once per frame
	void Update ()
	{   
		playerAnim.SetBool ("isDead", isDead);

		if(!lvlComplete)
			time += Time.deltaTime;

		if (GameObject.FindGameObjectWithTag ("Shield") == null) {
			canThrow = true;
		} else {
			canThrow = false;
		}

		if (isBlocking) {
			blocker.GetComponent<BoxCollider2D>().enabled = true;
		} else {
			blocker.GetComponent<BoxCollider2D>().enabled = false;
		}


		// Defines the currentDirection, this is used to control the direction of the shield throw
		if (Input.GetAxisRaw ("Horizontal") == 0 && Input.GetAxisRaw ("Vertical") >= 0.1) {
			currentDirection = new Vector2 (0, 1); // currentDirection == Up
		} else if (Input.GetAxisRaw ("Horizontal") <= -0.1 && Input.GetAxisRaw ("Vertical") >= 0.1) {
			currentDirection = new Vector2 (-1, 1); // currentDirection == UpLeft
		} else if (Input.GetAxisRaw ("Horizontal") >= 0.1 && Input.GetAxisRaw ("Vertical") >= 0.1) {
			currentDirection = new Vector2 (1, 1); // currentDirection == UpRight
		} else if (Input.GetAxisRaw ("Horizontal") <= -0.1 && Input.GetAxisRaw ("Vertical") == 0) {
			currentDirection = new Vector2 (-1, 0); // currentDirection == Left
		} else if (Input.GetAxisRaw ("Horizontal") >= 0.1 && Input.GetAxisRaw ("Vertical") == 0) {
			currentDirection = new Vector2 (1, 0); // currentDirection == Right
		} else if (Input.GetAxisRaw ("Horizontal") == 0 && Input.GetAxisRaw ("Vertical") <= -0.1) {
			currentDirection = new Vector2 (0, -1); // currentDirection == Down
		} else if (Input.GetAxisRaw ("Horizontal") <= -0.1 && Input.GetAxisRaw ("Vertical") <= -0.1) {
			currentDirection = new Vector2 (-1, -1); // currentDirection == DownLeft
		} else if (Input.GetAxisRaw ("Horizontal") >= 0.1 && Input.GetAxisRaw ("Vertical") <= -0.1) {
			currentDirection = new Vector2 (1, -1); // currentDirection == DownRight
		}


		if (!isRolling && !isThrowing) {
			playerMovement = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));

			DefinePlayerDirection (currentDirection);

			if (Input.GetAxisRaw ("Horizontal") != 0 || Input.GetAxisRaw ("Vertical") != 0) {

				playerAnim.SetBool ("isWalking", true);

				isWalking = true;

				DefinePlayerDirection (playerMovement);


				//DefinePlayerDirection (currentDirection);
			} else {

				playerAnim.SetBool ("isWalking", false);

				isWalking = false;
			}
		}


		if (!isDead) {
			// Setting up the Shield behavior
			if (!shieldDeployed && !isRolling && !isThrowing) {

				playerAnim.SetBool ("canBlock", true);

				canBlock = true;

			} else if (shieldDeployed) {

				playerAnim.SetBool ("canBlock", false);

				playerAnim.SetBool ("isBlocking", false);

				playerAnim.SetBool ("isThrowing", false);

				isThrowing = false;

				canBlock = false;

				shieldController = GameObject.Find ("projectile").GetComponent<ShieldController> ();

			} else {

				playerAnim.SetBool ("canBlock", false);

				playerAnim.SetBool ("isBlocking", false);

				canBlock = false;

				isBlocking = false;

			}
		}

		//if(shieldDeployed && (Input.GetButtonDown("Shield") || Input.GetButtonUp("Shield") || Input.GetButton("Shield"))){
			if (shieldDeployed && (Input.GetButtonDown("Shield")))
			{
				shieldReturn = true;
			}
			if (shieldReturn)
			{
				ReturnShield();
			}

		// Setting the shieldCounter which determines the state of the player
		if (shieldDeployed) {
			shieldCounter = 2;
		} else if (isBlocking && !shieldDeployed) {
			shieldCounter = 1;
		} else if (isRolling && !shieldDeployed) {
			shieldCounter = -1;
		} else if (!shieldDeployed){
			shieldCounter = 0;
		}


		// When the shieldCounter is set to a certain value determine what happens.
		switch(shieldCounter){

		// if rolling
		case -1:

			if(shieldDeployed && Input.GetButtonUp("Shield")){
				ShieldReturn ();
			}
			break;
		// if idle or walking
		case 0:
			
			if (didJustRoll) {
				if (Input.GetButton ("Shield") && canThrow) {
					ShieldBlock ();
				}
			} else {
				if (Input.GetButtonDown ("Shield") && canThrow) {
					ShieldBlock ();
				}
			}
			break;
		// if blocking
		case 1:

			if (!isThrowing && Input.GetButtonUp ("Shield") && canThrow) {
				ShieldThrow ();
			}
			break;

		// if shieldDeployed
		case 2:
			
			if(Input.GetButtonUp("Shield")){
				//ShieldReturn ();
			}
			break;
		default:
			break;
		}
			
		if (Input.GetButtonDown ("Roll")) {
			shieldCounter = -1;
		}

		// Setting up the Roll button behavior
		if (!isRolling && isWalking && !isThrowing) {

			playerAnim.SetBool ("isRolling", false);

			if (Input.GetButtonDown ("Roll")) {

				isRolling = true;

				source.PlayOneShot (rollSound, 0.1f);

				isThrowing = false;

				playerAnim.SetBool ("isThrowing", false);

				playerAnim.SetBool ("isRolling", true);

				playerAnim.SetBool ("canBlock", false);

				canBlock = false;
			}
		}

		if (testThrow)
		{
			ShieldLaunch();
		}
		if (testBlock)
		{
			ShieldBlock();
		}
		if (testRoll)
		{
			isRolling = true;
		}


	}

	void FixedUpdate ()
	{
		centerRot = Mathf.Atan2(currentDirection.x, -currentDirection.y) * Mathf.Rad2Deg;
		centerPoint.transform.rotation = (Quaternion.Euler(new Vector3(0, 0, centerRot)));

		if (!isDead) {
			if ((!isBlocking || isRolling || shieldDeployed) && !isThrowing) {
				GetComponent<Rigidbody2D> ().MovePosition (GetComponent<Rigidbody2D> ().position + playerMovement * moveSpeed * Time.deltaTime);
			} else if (isRolling) {
				GetComponent<Rigidbody2D> ().MovePosition (GetComponent<Rigidbody2D> ().position + playerMovement * (moveSpeed * 1.25f) * Time.deltaTime);
			} else if (isThrowing) {
				GetComponent<Rigidbody2D> ().MovePosition (GetComponent<Rigidbody2D> ().position);
			}
		}
	}

	// Sends the axis input to the animator.
	void DefinePlayerDirection (Vector2 input)
	{
		playerAnim.SetFloat ("inputX", input.x);
		playerAnim.SetFloat ("inputY", input.y);
	}


	void ShieldBlock ()
	{
		source.PlayOneShot (blockSound, 0.15f);
		playerAnim.SetBool ("isBlocking", true);
		isBlocking = true;
		shieldCounter = 1;
		didJustRoll = false;
	}

	// Changes animation state to Throwing.
	void ShieldThrow ()
	{
		shieldCounter = 2;
		playerAnim.SetBool ("canBlock", false);
		playerAnim.SetBool ("isThrowing", true);
		isThrowing = true;
		didJustRoll = false;

	}

	// Handles shield projectile behavior.
	void ShieldLaunch(){

		if (GameObject.FindGameObjectWithTag ("Shield") == null) {
			source.PlayOneShot (throwSound, 0.25f);

			projectile = Instantiate (shield) as GameObject;

			projectile.name = "projectile";

			projectile.transform.position = transform.position;

			Rigidbody2D rBody = projectile.GetComponent<Rigidbody2D> ();

			rBody.velocity = currentDirection * 12;

			shieldDeployed = true;

			isBlocking = false;
		}
	}

	void ReturnShield()
	{   
		//kind of bounces until hitcounter reaches limit, use either this or kill on wall collision
		//if (!shieldController.isColliding)
		projectile.transform.position = Vector2.MoveTowards(projectile.transform.position, transform.position, 1f);

		if (projectile.transform.position == transform.position)
		{
			shieldController.Kill();         
		}

		//kills on wall collision
		else if (shieldController.isColliding)
		{
			shieldController.Kill();
		}

	}

	// Sets isThrowing to false after animation completes.
	void ThrowingEnd(){
		isThrowing = false;
		playerAnim.SetBool ("isThrowing", false);
		shieldCounter = 2;
	}




	// Enables shield blocking and throwing once the shield is returned.
	void ShieldReturn ()
	{
		GameObject[] projectiles = GameObject.FindGameObjectsWithTag("Shield");

		for (int i = 0; i < projectiles.Length; i++) {
			shieldController = projectiles [i].GetComponent<ShieldController> ();
			shieldController.Kill ();
		}
	}

	public void PlayReturnSound(){
		source.PlayOneShot (shieldReturnSound, 0.05f);
	}

	// This method is called at the end of the roll animation cycle via an event.
	// Events can be attached to animations inside the Animation window.
	// That is where you will find this method call.
	void RollEnd ()
	{
		isRolling = false;
		canBlock = true;
		didJustRoll = true;
		playerAnim.SetBool ("canBlock", true);
		playerAnim.SetBool ("isRolling", false);
	}   

	void DestroySelf(){
		Destroy (this.gameObject);

		if (lifeCount > 0)
			SceneManager.LoadScene (currentScene);
		else     //Game Over
		{
			lifeCount = 7;
			time = 0;
			totalScore = 0;
			//Game Over screen?
			SceneManager.LoadScene("GameOver");
		}
	}

	// Kill the player and reload the level.
	public void Kill ()
	{
		soundController.PlayerDeath ();
		if(testRespawn)
		{
			//Destroy(this.gameObject);
			isDead = true;
			cirCol.enabled = false;
			return;
		}

		//transform.position = startPosition;
		lifeCount--;
		//Destroy (this.gameObject);
		cirCol.enabled = false;
		isDead = true;

		/*
		if (lifeCount > 0)
			SceneManager.LoadScene (currentScene);
		else     //Game Over
		{
			lifeCount = 7;
			time = 0;
			totalScore = 0;
			//Game Over screen?
			SceneManager.LoadScene("GameOver");
		}*/

	}

	// Uncomment below for infinite lives.
	/*public void Kill ()
	{
		//transform.position = startPosition;
		//Destroy(GameObject.Find ("projectile"));
		//lifeCount--;
		//Destroy (this.gameObject);
		isDead = true;

	}*/

	// Uncomment below and comment out the above Kill() for testing mode.
	/*public void Kill(){
	}*/

	//Calculates total score for level
	void ScoreLvl()
	{
		lvlScore -= (int)time;

		if (lvlScore < 0)
			lvlScore = 0;

		Score.lvlScores[currentScene - 2] = lvlScore;
		totalScore += lvlScore;
	}
		
	// Advance to the next level.
	public IEnumerator NextScene ()
	{

		if (!lvlComplete) {
			ScoreLvl ();
			soundController.PlayerExit ();
		}

		lvlComplete = true;
		float fadeTime = GameObject.Find("Main Camera").GetComponent<Fader>().BeginFade(1);
		cirCol.enabled = false;
		GetComponent<SpriteRenderer> ().enabled = false;
		yield return new WaitForSeconds(fadeTime);


		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex + 1);
	}
}
