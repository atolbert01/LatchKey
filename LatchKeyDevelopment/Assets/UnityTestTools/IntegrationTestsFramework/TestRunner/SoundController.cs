using UnityEngine;
using System.Collections;

public class SoundController : MonoBehaviour {

	private AudioSource source;
	public AudioClip playerDeathSound;
	public AudioClip playerExitSound;

	void Awake(){
		source = GetComponent<AudioSource> ();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PlayerExit(){
		source.PlayOneShot (playerExitSound, 0.2f);	
	}

	public void PlayerDeath(){
		source.PlayOneShot (playerDeathSound, 0.2f);
	}
}
