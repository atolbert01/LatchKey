using UnityEngine;
using System.Collections;

public class LobsterProjectile : MonoBehaviour {

    // public int hitCounter;
    public PlayerController playCont;

    public Transform playerPos;            //fix this
    public GameObject player;
    public GameObject enemy;
    Vector2 dir;
    private float projectileSpeed = 10f;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerPos = player.transform;
        
        dir = playerPos.position - transform.position;
        
        playCont = player.GetComponent<PlayerController>();
    }

    void FixedUpdate()
    {
        //Projectile moves forward in a line at given speed
        this.GetComponent<Rigidbody2D>().velocity = dir.normalized * projectileSpeed;
        Physics2D.IgnoreCollision(enemy.GetComponent<Collider2D>(), GetComponent<Collider2D>());
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.layer == 8)  //wall
        {
            Kill();
        }
        if (col.gameObject.tag == "Hazard")
        {
            Kill();
        }
        if(col.gameObject.tag == "Shield")
        {
            Kill();
        }
        if (col.gameObject == player)
        {
            if (playCont.isBlocking)
            {
                Kill();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 8)  //wall
        {
            Kill();
        }
        else if (col.gameObject.tag == "Hazard")
        {
            Kill();
        }
        else if (col.gameObject.tag == "Shield")
        {
            Kill();
        }
    }

    // Destroys the shield projectile and resets the player shield capabilities.
    public void Kill()
    {
        Destroy(this.gameObject);
    }
}
