using UnityEngine;
using System.Collections;

public class BlockerScript : MonoBehaviour {

	//public PlayerController playCont;

	// Use this for initialization
	void Start () {
		//playCont = GetComponentInParent<PlayerController> ();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	//public override void OnTriggerStay2D (Collider2D col)
	void OnTriggerStay2D (Collider2D col)
	{

		Physics2D.IgnoreCollision (GetComponent<BoxCollider2D> (), col);
		/*if (col.gameObject.layer == 9) {    //Hazard or Lava
			//if (col.gameObject.tag == "Slider")       //Sliders always kill
			Physics2D.IgnoreCollision (GetComponent<BoxCollider2D> (), col);
		} else if (col.gameObject.layer == 13) {    //Enemy
			Physics2D.IgnoreCollision (GetComponent<BoxCollider2D> (), col);
		} else if (col.gameObject.layer == 14) {
			Physics2D.IgnoreCollision (GetComponent<BoxCollider2D> (), col);
		} else {
			Physics2D.IgnoreCollision (GetComponent<BoxCollider2D> (), col);
		}*/
	}
		
	//public override void OnCollisionEnter2D (Collision2D col)
	void OnCollisionEnter2D (Collision2D col)
	{
		
		//if (col.gameObject.layer == 13) {    //Enemy
		//	Physics2D.IgnoreCollision(col);
		//}
	}
}
