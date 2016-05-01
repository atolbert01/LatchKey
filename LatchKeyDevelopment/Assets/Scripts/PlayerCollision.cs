using UnityEngine;
using System.Collections;

public class PlayerCollision : MonoBehaviour {

	public PlayerController playCont;

	void Start(){
		playCont = GetComponent<PlayerController> ();
	}
		
	//public virtual void OnTriggerStay2D (Collider2D col)
	void OnTriggerStay2D (Collider2D col)
	{
		if (col.gameObject.layer == 9) {    //Hazard or Lava
			//if (col.gameObject.tag == "Slider")       //Sliders always kill
				//playCont.Kill ();
			//else if (!playCont.isRolling)    //Can roll over other hazards
				//playCont.Kill ();
		} else if (col.gameObject.layer == 13) {    //Enemy
			//if (!isBlocking)
			//playCont.Kill ();
		} else if (col.gameObject.layer == 11) {  //Rift
			playCont.NextScene ();
		} else if (col.gameObject.layer == 14) {
			if (!playCont.isRolling) {
				playCont.Kill ();
			}
		}
	}
		
	//public virtual void OnCollisionEnter2D (Collision2D col)
	void OnCollisionEnter2D (Collision2D col)
	{
		if (col.gameObject == playCont.enemy) {
			//Kill();
		} else if (col.gameObject.layer == 13) {    //Enemy
			//if (!isBlocking)
			//playCont.Kill ();
		}

	}
}