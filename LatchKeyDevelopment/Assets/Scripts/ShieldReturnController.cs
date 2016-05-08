using UnityEngine;
using System.Collections;

public class ShieldReturnController : MonoBehaviour {

	//private Rigidbody2D rigidbody;
	private GameObject player;

	// Use this for initialization
	void Start () {
		//rigidbody = GetComponent<Rigidbody2D> ();
		player = GameObject.Find ("Player");
	
	}
	
	// Update is called once per frame
	void Update () {
		//rigidbody.velocity = (player.transform.position) * 12 * Time.deltaTime;
		transform.Translate((transform.position - player.transform.position) * 12 * Time.deltaTime);
	
	}
}
