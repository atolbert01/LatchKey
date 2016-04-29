using UnityEngine;
using System.Collections;

public class BatController : MonoBehaviour {

    private Quaternion defaultRotation;
    private Vector3 defaultPosition;
    private Vector3 target;
    private Vector3 dir;

    private float lineOfSight;
    private float moveSpeed;
    private float angle;
    private const float startRange = 0.1f;

    private GameObject player;
    private PlayerController playerController;

    // Use this for initialization
    void Start () {
        defaultPosition = transform.position;
        defaultRotation = transform.rotation;
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        //Range at which enemy will chase player
        lineOfSight = 5;
        //Enemy chase speed
        moveSpeed = 3.5f;
    }
	
	// Update is called once per frame
	void Update () {
        if (Vector2.Distance(transform.position, player.transform.position) < lineOfSight)  //Player in enemy range
        {
            target = player.transform.position;          
            MoveTowards(target);
        }
        else
        {           
            if (Vector2.Distance(transform.position, defaultPosition) > startRange) //Player out of enemy range, not at defaultPosition
            {
                MoveTowards(defaultPosition);
            }
            else    //Resets object/sprite position and rotation
            {
                transform.rotation = defaultRotation;
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

    //There's a much simpler way to do this using transform.LookAt(), but that rotates the z axis
    //Enemy faces and moves toward a target position
    void MoveTowards(Vector3 target)
    {   
        dir = target - transform.position;
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.position += transform.right * moveSpeed * Time.deltaTime;
    }

    public void Kill()
    {
        Destroy(this.gameObject);
    }

}
