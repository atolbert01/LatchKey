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

	private Animator playerAnim;

	public bool shieldDeployed;

	public bool isRolling;

	public bool isWalking;

	public bool canBlock;

    public bool isBlocking;

	// The current scene.
	private int currentScene;

	// Use this for initialization
	void Start ()
	{
		// Just in case.
		name = "Player";

		// Sets the currentScene index to the actual current scene.
		currentScene = SceneManager.GetActiveScene().buildIndex;

		playerAnim = GetComponent<Animator> ();

		// Identifies the prefab we'll be instantiating shield projectiles from.
		shield = (GameObject)Resources.Load ("Shield");

		// currentDirection == Down. Player will always start facing down.
		currentDirection = new Vector2 (0, -1);

		startPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!isRolling) {
			playerMovement = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));

			if (Input.GetAxisRaw ("Horizontal") != 0 || Input.GetAxisRaw ("Vertical") != 0) {

				playerAnim.SetBool ("isWalking", true);

				isWalking = true;

				DefinePlayerDirection (playerMovement);


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
			
			} else {

				playerAnim.SetBool ("isWalking", false);

				isWalking = false;
			}
		}

		// Setting up the Shield button behavior
		if (!shieldDeployed && !isRolling) {

			playerAnim.SetBool ("canBlock", true);

			canBlock = true;

			if (Input.GetButton ("Shield") && canBlock) {
				ShieldBlock ();
			}

			if (Input.GetButtonUp ("Shield") && !isRolling) {
				ShieldThrow (currentDirection);		
			}
			
		} else if (shieldDeployed) {

			playerAnim.SetBool ("canBlock", false);

			playerAnim.SetBool ("isBlocking", false);

			canBlock = false;

			shieldController = GameObject.Find ("projectile").GetComponent<ShieldController> ();

			} else {
				
			playerAnim.SetBool ("canBlock", false);
				
			playerAnim.SetBool ("isBlocking", false);
				
			canBlock = false;

            isBlocking = false;

        }


		// Setting up the Roll button behavior
		if (!isRolling && isWalking) {

			playerAnim.SetBool ("isRolling", false);

			if (Input.GetButtonDown ("Roll")) {

				isRolling = true;

				playerAnim.SetBool ("isRolling", true);

				playerAnim.SetBool ("canBlock", false);

				canBlock = false;
			}
		}
	}

	void FixedUpdate ()
	{
		if (!Input.GetButton ("Shield") || isRolling || shieldDeployed) {
			GetComponent<Rigidbody2D> ().MovePosition (GetComponent<Rigidbody2D> ().position + playerMovement * moveSpeed * Time.deltaTime);
		} else if (isRolling) {
			GetComponent<Rigidbody2D> ().MovePosition (GetComponent<Rigidbody2D> ().position * moveSpeed * Time.deltaTime);
		}
	}

	// Sends the axis input to the animator.
	void DefinePlayerDirection (Vector2 input)
	{
		playerAnim.SetFloat ("inputX", input.x);
		playerAnim.SetFloat ("inputY", input.y);
	}

	// Handles shield projectile behavior.
	void ShieldThrow (Vector2 currentDirection)
	{
		this.currentDirection = currentDirection;

		playerAnim.SetBool ("canBlock", false);

		GameObject projectile = Instantiate (shield) as GameObject;

		projectile.name = "projectile";

		projectile.transform.position = transform.position;

		Rigidbody2D rBody = projectile.GetComponent<Rigidbody2D> ();

		rBody.velocity = currentDirection * 12;

		shieldDeployed = true;

        isBlocking = false;
	}

	// This doesn't really do anything for now because there are
	// no blockable hazards (such as arrows) or enemies.
	void ShieldBlock ()
	{
		playerAnim.SetBool ("isBlocking", true);
        isBlocking = true;
	}

	// Enables shield blocking and throwing once the shield is returned.
	void ShieldReturn ()
	{
		shieldController = GameObject.Find ("projectile").GetComponent<ShieldController> ();
		shieldController.Kill ();
	}

	// This method is called at the end of the roll animation cycle via an event.
	// Events can be attached to animations inside the Animation window.
	// That is where you will find this method call.
	void RollEnd ()
	{
		isRolling = false;
		canBlock = true;
		playerAnim.SetBool ("canBlock", false);
		playerAnim.SetBool ("isRolling", false);
	}

	void OnTriggerStay2D(Collider2D col){
		if (col.gameObject.layer == 9) {    //Hazard or Lava
            if(!isRolling)
			    Kill();
		}
        else if (col.gameObject.layer == 13)    //Enemy
        {
            if(!isBlocking)
                Kill();
        }
		else if (col.gameObject.layer == 11) {  //Rift
			NextScene();
		}
	}

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject == enemy)
        {
            //Kill();
        }
        else if (col.gameObject.layer == 13)    //Enemy
        {
            if (!isBlocking)
                Kill();
        }

    }

    // Kill the player and reload the level.
    void Kill(){
		transform.position = startPosition;
		//Destroy(GameObject.Find ("projectile"));
		Destroy (this.gameObject);
		SceneManager.LoadScene(currentScene);
	}

	// Advance to the next level.
	// Note: this is just a hack.
	// Obviously we need to work out our scene transistions more thoroughly.
	void NextScene(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }
}
