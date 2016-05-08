using UnityEngine;
using System.Collections;

// Controls rift behavior.

public class RiftController : MonoBehaviour {

	private Animator riftAnim;
	public bool isActive;

	private AudioSource source;

	public AudioClip riftActivatedSound;

	void Awake(){
		source = GetComponent<AudioSource> ();
	}

	// Use this for initialization
	void Start () {
		riftAnim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (isActive) {
			riftAnim.SetBool ("isActive", true);
		}
	}

	public void SetActive(){
		isActive = true;
		GetComponent<BoxCollider2D> ().enabled = true;
		source.PlayOneShot (riftActivatedSound, 0.15f);
	}
		
}
