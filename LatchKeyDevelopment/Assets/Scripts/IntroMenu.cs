using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroMenu : MonoBehaviour {
    
    private float startTime;

    private int toolbarInt;

    private string[] toolbarstrings = { "Audio", "Graphics", "Stats" };

    private Color color;

    public enum Page
    {
        Main, Options, HighScore
    }

    private Page currentPage;

    // Use this for initialization
    void Start ()
    {
        startTime = 0.1f;
        currentPage = Page.Main;
        toolbarInt = 0;
        color = Color.white;
	}
	
	// Update is called once per frame
	void Update ()
    {
	}

    void OnGUI()
    {
        GUI.color = color;
        switch (currentPage)
        {
            case Page.Main: mainMenu(); break;
            case Page.Options: showToolbar(); break;
            case Page.HighScore: showHighScore(); break;
        }
    }

    void mainMenu()
    {
        int space = 5;

        BeginPage(200,200);
        if (GUILayout.Button("Start Game"))
        {
            SceneManager.LoadScene("Entryway");
        }
        GUILayout.Space(space);
        if (GUILayout.Button("Options"))
        {
            currentPage = Page.Options;
        }
        GUILayout.Space(space);
        if (GUILayout.Button("High Score"))
        {
            currentPage = Page.HighScore;
        }
        GUILayout.Space(space);
        if (GUILayout.Button("Quit"))
        {
            Application.Quit();
        }
        EndPage();
    }

    void showToolbar()
    {
        BeginPage(300, 200);
        toolbarInt = GUILayout.Toolbar(toolbarInt, toolbarstrings);
        switch (toolbarInt)
        {
            case 0: VolumeControl(); break;
            case 1: Qualities(); QualityControl(); break;
            case 2: ShowDevice(); break;
        }
        EndPage();
    }

    void showHighScore()
    {
        //Temporary until we can link the scoring to the High Scores Menu
        PlayerPrefs.SetString("num1Player", "Aaron Tolbert");
        PlayerPrefs.SetInt("num1Player_highscore", 200);
        PlayerPrefs.SetString("num2Player", "Joshua Agnes");
        PlayerPrefs.SetInt("num2Player_highscore", 175);
        PlayerPrefs.SetString("num3Player", "Leo Wack");
        PlayerPrefs.SetInt("num3Player_highscore", 160);
        PlayerPrefs.SetString("num4Player", "Paul Ross");
        PlayerPrefs.SetInt("num4Player_highscore", 120);
        PlayerPrefs.SetString("num5Player", "Ryan Bonisch");
        PlayerPrefs.SetInt("num5Player_highscore", 100);

        int num1Player_highscore = PlayerPrefs.GetInt("num1Player_highscore");
        string num1Player = PlayerPrefs.GetString("num1Player");
        int num2Player_highscore = PlayerPrefs.GetInt("num1Player_highscore");
        string num2Player = PlayerPrefs.GetString("num2Player");
        int num3Player_highscore = PlayerPrefs.GetInt("num3Player_highscore");
        string num3Player = PlayerPrefs.GetString("num3Player");
        int num4Player_highscore = PlayerPrefs.GetInt("num4Player_highscore");
        string num4Player = PlayerPrefs.GetString("num4Player");
        int num5Player_highscore = PlayerPrefs.GetInt("num5Player_highscore");
        string num5Player = PlayerPrefs.GetString("num5Player");
        int height = 25;
        int width = 150;
        

        BeginPage(300, 200);
        //Add highscore functionality
        GUI.Label(new Rect(0, 0, width, height), "\tHighscores");
        GUI.Label(new Rect(0, height, width, height), num1Player);
        GUI.Label(new Rect(width, height, width, height), num1Player_highscore.ToString());
        GUI.Label(new Rect(0, height*2, width, height), num2Player);
        GUI.Label(new Rect(width, height*2, width, height), num2Player_highscore.ToString());
        GUI.Label(new Rect(0, height*3, width, height), num3Player);
        GUI.Label(new Rect(width, height*3, width, height), num3Player_highscore.ToString());
        GUI.Label(new Rect(0, height*4, width, height), num4Player);
        GUI.Label(new Rect(width, height*4, width, height), num4Player_highscore.ToString());
        GUI.Label(new Rect(0, height*5, width, height), num5Player);
        GUI.Label(new Rect(width, height*5, width, height), num5Player_highscore.ToString());
        EndPage();
    }

    bool IsBeginning()
    {
        return (Time.time < startTime);
    }

    void ShowDevice()
    {
        GUILayout.Label("Unity player version " + Application.unityVersion);
        GUILayout.Label("Graphics: " + SystemInfo.graphicsDeviceName + " " +
            SystemInfo.graphicsMemorySize + "MB\n" +
            SystemInfo.graphicsDeviceVersion + "\n" +
            SystemInfo.graphicsDeviceVendor);
        GUILayout.Label("Shadows: " + SystemInfo.supportsShadows);
        GUILayout.Label("Image Effects: " + SystemInfo.supportsImageEffects);
        GUILayout.Label("Render Textures: " + SystemInfo.supportsRenderTextures);
    }

    void Qualities()
    {
        switch (QualitySettings.GetQualityLevel())
        {
            case 0:
                GUILayout.Label("Fastest");
                break;
            case 1:
                GUILayout.Label("Fast");
                break;
            case 2:
                GUILayout.Label("Simple");
                break;
            case 3:
                GUILayout.Label("Good");
                break;
            case 4:
                GUILayout.Label("Beautiful");
                break;
            default:
                GUILayout.Label("Fantastic");
                break;
        }
    }

    void QualityControl()
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Decrease"))
        {
            QualitySettings.DecreaseLevel();
        }
        if (GUILayout.Button("Increase"))
        {
            QualitySettings.IncreaseLevel();
        }
        GUILayout.EndHorizontal();
    }

    void VolumeControl()
    {
        GUILayout.Label("Volume");
        AudioListener.volume = GUILayout.HorizontalSlider(AudioListener.volume, 0, 1);
    }

    void BeginPage(int width, int height)
    {
        GUILayout.BeginArea(new Rect(
            ((Screen.width / 2) - width) / 2 + (Screen.width / 2),
            (Screen.height - height) / 2,
            width, height));
    }

    void EndPage()
    {
        GUILayout.EndArea();
        if (currentPage != Page.Main)
        {
            ShowBackButton();
        }
    }

    void ShowBackButton()
    {
        if (GUI.Button(new Rect(20, Screen.height - 50, 50, 20), "Back"))
        {
            currentPage = Page.Main;
        }
    }
}
