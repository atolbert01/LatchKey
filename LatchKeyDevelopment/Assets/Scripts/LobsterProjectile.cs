using UnityEngine;
using System.Collections;

public class LobsterProjectile : MonoBehaviour {

    // public int hitCounter;
    public PlayerController playCont;

    public Transform playerPos;            //fix this
    public GameObject player;
    public GameObject enemy;
    Vector2 dir;
    private float projectileSpeed = 14f;

	private float shotTime;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerPos = player.transform;
        
        dir = playerPos.position - transform.position;
        
        playCont = player.GetComponent<PlayerController>();

		shotTime = 0.75f;  //time between lobster kid appearance and shot

        
    }

	void FixedUpdate()
	{
		shotTime -= Time.deltaTime;

		if (shotTime > 0)
			dir = playerPos.position - transform.position;

		else if (shotTime <= 0)
		{
			//Projectile moves forward in a line at given speed
			this.GetComponent<Rigidbody2D>().velocity = dir.normalized * projectileSpeed;
			Physics2D.IgnoreCollision(enemy.GetComponent<Collider2D>(), GetComponent<Collider2D>());
		}

	}

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.layer == 8)  //wall
        {
            Kill();
        }
        /*if (col.gameObject.tag == "Hazard")
        {
            Kill();
        }*/
        if(col.gameObject.tag == "Shield")
        {
            Kill();
        }
        //if (col.gameObject == player)
		if (col.gameObject.layer == 15)
		{
            if (playCont.isBlocking)
            {
                Kill();
            }
        }
    }

    
    void OnTriggerEnter2D(Collider2D col)
    {
		if (col.gameObject.layer == 8) {  //wall
			Kill ();
		}
        /*else if (col.gameObject.tag == "Hazard")
        {
            Kill();
        }*/ 
        else if (col.gameObject.tag == "Shield") {
			Kill ();
		} //else if (col.gameObject == player && playCont.isBlocking) {
		else if(col.gameObject.layer == 15 && playCont.isBlocking){
			Kill ();
		}
    } 

    // Destroys the shield projectile and resets the player shield capabilities.
    public void Kill()
    {
        Destroy(this.gameObject);
    }
}
