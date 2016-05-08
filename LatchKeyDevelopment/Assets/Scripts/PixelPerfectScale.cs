using UnityEngine;
using System.Collections;

// This script enables the game to run in pixel perfect display mode.
// Unity is a 3D engine and as such it is designed with modern displays in mind.
// This means that with the default camera mode low-resolution pixel art looks really crappy.
// The PixelPerfectCamera is a work around for this issue.

[ExecuteInEditMode]
public class PixelPerfectScale : MonoBehaviour {

	public int screenVerticalPixels = 320;

	public bool preferUncropped = true;

	private float screenPixelsY = 0;

	private bool currentCropped = false;
	
	// Update is called once per frame
	void Update () {

		if (screenPixelsY != (float)Screen.height || currentCropped != preferUncropped) {
			screenPixelsY = (float)Screen.height;
			currentCropped = preferUncropped;

			float screenRatio = screenPixelsY / screenVerticalPixels;
			float ratio;

			if (preferUncropped) {
				ratio = Mathf.Floor (screenRatio) / screenRatio;
			} else {
				ratio = Mathf.Ceil (screenRatio) / screenRatio;
			}

			transform.localScale = Vector3.one * ratio;
		}
	
	}
}
