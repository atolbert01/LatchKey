using UnityEngine;
using System.Collections;

// This script contains the shield projectile behavior.

public class ShieldController : MonoBehaviour
{
	// Controls how many bounces it takes before the shield is "returned".
	// We need a more reliable solution for this problem, but this was simply
	// an expedient solution.
	public int hitCounter;

	public GameObject player;

	public GameObject shieldReturner;

	public GameObject shieldR;

	public PlayerController playerController;

	private AudioSource source;

	public AudioClip ricochetSound;

	public AudioClip enemyDeathSound;

	private LobsterKid lobsterKid;

	public bool isColliding;

	void Awake(){
		source = GetComponent<AudioSource> ();
	}

	// Use this for initialization
	void Start ()
	{

		player = GameObject.FindWithTag ("Player");
		playerController = player.GetComponent<PlayerController>();
		Physics2D.IgnoreCollision (player.GetComponent<Collider2D> (), GetComponent<Collider2D> ());

		if (GameObject.FindWithTag ("LobsterKid") != null) {
			lobsterKid = (LobsterKid)GameObject.FindWithTag ("LobsterKid").GetComponent<LobsterKid> ();
		}

		shieldReturner = (GameObject)Resources.Load ("Shield");
		isColliding = false;
	}

	// Update is called once per frame
	void Update ()
	{
	}

	void FixedUpdate(){
		//if (isReturnable == false) {
		//	Physics2D.IgnoreCollision (player.GetComponent<Collider2D> (), GetComponent<Collider2D> ());
		//}


	}

	void OnCollisionEnter2D(Collision2D col){
		if (col.gameObject.layer == 13) {
			source.PlayOneShot (enemyDeathSound, 0.5f);

		} else if (col.gameObject.layer == 8) {
			source.PlayOneShot (ricochetSound, 0.5f);
		}
	}

	void OnCollisionExit2D (Collision2D col)
	{
		isColliding = false;
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "Hazard" || col.gameObject.tag == "Slider") {
			Kill ();
		} else if (col.gameObject.layer == 13 && col.gameObject.tag != "LobsterKid") {
			source.PlayOneShot (enemyDeathSound, 0.5f);
		} else if (col.gameObject.tag == "LobsterKid") {
			lobsterKid = (LobsterKid) col.gameObject.GetComponent<LobsterKid>();
			if (!lobsterKid.isVisible) {
				return;
			} else {
				source.PlayOneShot (enemyDeathSound, 0.5f);
			}
		}
	}

	void OnTriggerExit2D(Collider2D col)
	{
		isColliding = false;
	}

	// Destroys the shield projectile and resets the player shield capabilities.
	public void Kill(){
		playerController.PlayReturnSound ();
		playerController.shieldDeployed = false;
		playerController.shieldReturn = false;
		isColliding = false;
		Destroy (this.gameObject);
		//playerController.shieldCounter = 0;
	}

}
