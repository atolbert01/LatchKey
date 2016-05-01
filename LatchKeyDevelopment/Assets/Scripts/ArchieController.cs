using UnityEngine;
using System.Collections;

public class ArchieController : MonoBehaviour
{

    private Vector2 moveDir;
    private int moveChoice;

    private float time;

    private bool colliding;

    private Animator boblinAnim;

    private bool isWalking;

    private Vector3 target;

    private float lineOfSight;
    private float moveSpeed;

    private GameObject arrow;

    private GameObject player;


    private float shotTimer;

	public PlayerController playerController;


    void Start()
    {
        arrow = (GameObject)Resources.Load("Arrow");

        player = GameObject.FindGameObjectWithTag("Player");

        shotTimer = 0;

		playerController = player.GetComponent<PlayerController> ();

        //Random direction

        boblinAnim = GetComponent<Animator>();
        moveChoice = Random.Range(-2, 2);

        lineOfSight = 10f;

        switch (moveChoice)
        {
            case -1:
                moveDir = Vector2.down;
                break;
            case 1:
                moveDir = Vector2.up;
                break;
            case 2:
                moveDir = Vector2.right;
                break;
            case -2:
                moveDir = Vector2.left;
                break;
            case 0:
                moveChoice = Random.Range(-2, 2);
                break;
        }


        //Random time to move in direction
        time = Random.Range(1f, 3f);


    }

    void Update()
    {
        time -= Time.deltaTime;
        this.GetComponent<Rigidbody2D>().velocity = moveDir.normalized * 2f;
        boblinAnim.SetInteger("moveChoice", moveChoice);

        switch (moveChoice)
        {
            case -1:
                moveDir = Vector2.down;
                break;
            case 1:
                moveDir = Vector2.up;
                break;
            case 2:
                moveDir = Vector2.right;
                break;
            case -2:
                moveDir = Vector2.left;
                break;
            case 0:
                moveChoice = Random.Range(-2, 2);
                break;
        }

        if (colliding)    //Reverse direction
        {
            moveChoice *= -1;
            colliding = false;
        }

        //Time is up, new random direction and interval
        if (time <= 0)
        {
            moveChoice = Random.Range(-2, 2);

            switch (moveChoice)
            {
                case -1:
                    moveDir = Vector2.down;
                    break;
                case 1:
                    moveDir = Vector2.up;
                    break;
                case 2:
                    moveDir = Vector2.right;
                    break;
                case -2:
                    moveDir = Vector2.left;
                    break;
                case 0:
                    moveChoice = Random.Range(-2, 2);
                    break;
            }

            time = Random.Range(1f, 3f);
        }
        if (Vector2.Distance(transform.position, player.transform.position) < lineOfSight)
        {
            //if (Mathf.Floor(transform.localPosition.y) == Mathf.Floor(player.transform.localPosition.y) || Mathf.Floor(transform.localPosition.x) == Mathf.Floor(player.transform.localPosition.x))
           // {
                Shoot();
                
            //}
            //else shotTimer = 0;
        }

        
    }

 

    void OnCollisionEnter2D(Collision2D col)
    {
        //collisions that reverse direction
		if (col.gameObject.layer == 8) {  //wall
			colliding = true;
		} else if (col.gameObject.layer == 9) { //hazard
			colliding = true;
		} else if (col.gameObject.tag == "Lava") {
			colliding = true;
		} else if (col.gameObject.layer == 13) {    //enemy
			colliding = true;
		} else if (col.gameObject.tag == "Switch") {
			colliding = true;
		} else if (col.gameObject.layer == 12) { // player
			playerController.Kill ();
		} else if (col.gameObject.layer == 15) { // blocker
			colliding = true;
		}

        //collisions that kill boblin
        if (col.gameObject.name == "projectile")
        {
            PlayerController.lvlScore += 10;
            Kill();
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
		if (col.gameObject.layer == 9) { //hazard
			colliding = true;
		} else if (col.gameObject.layer == 15) {
			colliding = true;
		} else if (col.gameObject.layer == 12) {
			colliding = true;
		}
    }

    void Shoot()
    {
        shotTimer -= Time.deltaTime;
        if (shotTimer < 0) { 
            GameObject projectile = Instantiate(arrow) as GameObject;
            projectile.name = "Arrow";
            Physics2D.IgnoreCollision(projectile.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            projectile.transform.position = transform.position;
            shotTimer = 3f;
        }
    }

    public void Kill()
    {
        Destroy(this.gameObject);
    }
}
