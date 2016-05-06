using UnityEngine;
using System.Collections;

public class BatController : MonoBehaviour {

    //private Quaternion defaultRotation;
    private Vector3 defaultPosition;
    private Vector3 target;

    private float lineOfSight;
    private float moveSpeed;
    private const float startRange = 0.1f;

    private GameObject player;
    private PlayerController playerController;

    // Use this for initialization
    void Start () {
        defaultPosition = transform.position;
        //defaultRotation = transform.rotation;
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        //Range at which enemy will chase player
        lineOfSight = 5;
        //Enemy chase speed
        moveSpeed = 0.07f;
    }
	
	// Update is called once per frame
	void Update () {
        if (Vector2.Distance(transform.position, player.transform.position) < lineOfSight)  //Player in enemy range
        {
            target = player.transform.position;          

			transform.position = Vector2.MoveTowards (transform.position, target, moveSpeed);
        }
        else
        {           
            if (Vector2.Distance(transform.position, defaultPosition) > startRange) //Player out of enemy range, not at defaultPosition
            {
				defaultPosition = transform.position;
				transform.position = defaultPosition;
            }
            else    //Resets object/sprite position and rotation
            {
				defaultPosition = transform.position;
                transform.position = defaultPosition;
            }
        }
	}

    void OnTriggerEnter2D(Collider2D col)
    {
		if (col.gameObject.name == "projectile") {
			PlayerController.lvlScore += 15;
			Kill ();
		} else if (col.gameObject.layer == 12 && !playerController.isRolling) {
			playerController.Kill ();
		}
        
        /*Logic for when player is blocking?*/

    }

	void OnCollisionEnter2D(Collision2D col){
		if (col.gameObject.layer == 12) { // player
			playerController.Kill();
		}
	}

    public void Kill()
    {
        Destroy(this.gameObject);
    }

}
