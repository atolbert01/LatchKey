using UnityEngine;
using System.Collections;

public class HazardCollision : MonoBehaviour {

	public PlayerController playerController;

	// Use this for initialization
	void Start () {
		playerController = GameObject.Find ("Player").GetComponent<PlayerController> ();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D col){
		if (col.gameObject.layer == 12){ // player
			playerController.Kill();
		}
		else if(col.gameObject.layer == 15){ // blocker
			
		}
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.layer == 12){ // player
			playerController.Kill();
		}
		else if(col.gameObject.layer == 15){ // blocker

		}
		
	}
}
