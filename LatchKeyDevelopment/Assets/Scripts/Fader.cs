using UnityEngine;
using System.Collections;

public class Fader : MonoBehaviour {

    public Texture2D fadeOutTexture;    //creates an overlay
    public float fadeSpeed = 0.8f;

    private int drawDepth = -1000;      //order in draw heirarchy, low num = renders on top
    private int fadeDir = -1;       //
    private float alpha = 1.0f;

	
	void OnGUI () {
        alpha += fadeDir * fadeSpeed * Time.deltaTime;
        alpha = Mathf.Clamp01(alpha);

        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
        GUI.depth = drawDepth;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);
	}

    //fade in if -1, fade out if 1
    public float BeginFade (int direction)
    {
        fadeDir = direction;
        return fadeSpeed;
    }

    void OnLevelWasLoaded()
    {
        BeginFade(-1);
    }
	
}
