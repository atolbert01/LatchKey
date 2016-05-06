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

    public bool isColliding;


	// Use this for initialization
	void Start ()
	{
		player = GameObject.FindWithTag ("Player");
		playerController = player.GetComponent<PlayerController>();
		Physics2D.IgnoreCollision (player.GetComponent<Collider2D> (), GetComponent<Collider2D> ());

		shieldReturner = (GameObject)Resources.Load ("Shield");
        isColliding = false;
	}

	void OnCollisionEnter2D (Collision2D col)
	{
		if (hitCounter <= 0) {
			//playerController.Warp (transform.position);
			Kill();
		} else {

			if (col.gameObject.layer == 8) {
				--hitCounter;
                isColliding = true;
			}
            else if(col.gameObject.layer == 13)
            {
                --hitCounter;
            }
            else if (col.gameObject.tag == "Hazard" || col.gameObject.layer == 9) {
				Kill ();
			}
			else if(col.gameObject == player){
				Kill ();
			}

		}
        
	}
    void OnCollisionExit2D(Collision2D col)
    {
        isColliding = false;
    }
    


    void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "Hazard" || col.gameObject.tag == "Slider") {
			Kill ();
		}     
	}
    void OnTriggerExit2D(Collider2D col)
    {
        isColliding = false;
    }

    // Destroys the shield projectile and resets the player shield capabilities.
    public void Kill(){
		Destroy (this.gameObject);
		playerController.shieldDeployed = false;
        playerController.shieldReturn = false;
        isColliding = false;
		//playerController.shieldCounter = 0;
	}

}
