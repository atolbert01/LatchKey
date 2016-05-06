/*
This script is intended to be added to Introductory splash screens
and to transition them in 10 seconds or by the user pressing the 
ESC key.
*/

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SplashScreenDelay : MonoBehaviour {

    //Create global variables to calculate timing in game
    private float DELAY;

    private float timer;

	// Use this for initialization
	void Start () {
        //DELAY is set for 10 seconds after scene start
        DELAY = 10f;
        //Timer is set at 0 seconds after scene start
        timer = 0f;
	}
	
	// Update is called once per frame
	void Update () {
        //The timer variable will calculate the time from each frame and add it to itself
        timer += Time.deltaTime;

        //if the timer reaches 10 seconds or more OR the user presses the escape key
        //it will switch to the next scene
        if ((timer >= DELAY) || (Input.GetKeyDown(KeyCode.Escape))){
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
	}
}
