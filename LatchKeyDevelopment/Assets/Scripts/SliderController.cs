using UnityEngine;
using System.Collections;

public class SliderController : MonoBehaviour {

	private Rigidbody2D rBody;

	private bool wallColliding;

	public float velocity;

	public LayerMask wallLayer;

	public Transform lineEnd;

	// Use this for initialization
	void Start () {
		rBody = GetComponent<Rigidbody2D> ();

	}

	// Update is called once per frame
	void Update () {
		rBody.velocity = new Vector2 (velocity, rBody.velocity.y);
	}

	void FixedUpdate(){
		// Returns true if the linecast intersects our wall layer.
		wallColliding = Physics2D.Linecast (rBody.position, lineEnd.position, wallLayer);

		// Flips the entire object and reverses velocity.
		if (wallColliding) {
			transform.localScale = new Vector2 (transform.localScale.x * -1, transform.localScale.y);
			velocity *= -1;
		}
	}

	void OnCollisionEnter2D(Collision2D col){

		if (col.gameObject.layer == 8) {
			wallColliding = true;
		}
	}
}
