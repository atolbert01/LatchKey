using UnityEngine;
using System.Collections;

// This script controls the interaction between the shield projectile and rift

public class SwitchController : MonoBehaviour {

	private Animator switchAnim;

	public bool isActive;

    private static int switchCount;    //current active switches
    private static int numSwitch;      //total switches to be activated

	GameObject rift;

	RiftController riftController;

	private AudioSource source;

	public AudioClip switchActivatedSound;

	void Awake(){
		source = GetComponent<AudioSource> ();
	}

	// Use this for initialization
	void Start () {
		switchAnim = GetComponent<Animator> ();
		rift = GameObject.Find ("Rift");
		riftController = rift.GetComponent<RiftController> ();
        switchCount = 0;
        numSwitch = GameObject.FindGameObjectsWithTag("Switch").Length;
    }
	
	// Update is called once per frame
	void Update () {
		if (isActive) {
			switchAnim.SetBool ("isActive", true);
		}
    }

	void OnCollisionEnter2D(Collision2D col){
		if (col.gameObject.name == "projectile") {
            if (!isActive) {
                switchCount++;

				if (switchCount == numSwitch)
				{
					riftController.SetActive();
				}
            }  
            isActive = true;

			source.PlayOneShot (switchActivatedSound, 0.25f);
        }
	}
}
