using System;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private float startTime;

    private float savedTimeScale;

    public GameObject start;

    public Color statColor;

	public string[] credits = { "Team Dungeon Masters" , "Team Lead:\tAaron Tolbert" , "Team Members:\tJoshua Agnes, Leo Wack," , "\t\tPaul Ross, Ryan Bonisch" };

    public enum Page
    {
        None, Main, Options, Credits
    }

    private Page currentPage;

    private int toolbarInt;

    private string[] toolbarstrings = { "Audio", "Graphics", "Stats"};

    private GUIStyle currentStyle;

    void Start()
    {
        Time.timeScale = 1;
        statColor = Color.white;
        startTime = 0.1f;
        toolbarInt = 0;
        currentStyle = null;
    }

    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch (currentPage)
            {
                case Page.None:
                    PauseGame();
                    break;

                case Page.Main:
                    if (!IsBeginning())
                        UnPauseGame();
                    break;

                default:
                    currentPage = Page.Main;
                    break;
            }
        }
    }

    private void InitStyles()
    {
        if (currentStyle == null)
        {
            currentStyle = new GUIStyle(GUI.skin.box);
            currentStyle.normal.background = MakeTex(2, 2, new Color(0.5f, 0.5f, 0.5f));
        }
    }

    private Texture2D MakeTex(int width, int height, Color col)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; ++i)
        {
            pix[i] = col;
        }
        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }

    void OnGUI()
    {
        if (IsGamePaused())
        {
            InitStyles();
            GUI.Box(new Rect((Screen.width - 325) / 2, (Screen.height - 325) / 2, 325, 200), "", currentStyle);
            GUI.color = statColor;
            switch (currentPage)
            {
                case Page.Main: MainPauseMenu(); break;
                case Page.Options: ShowToolbar(); break;
                case Page.Credits: ShowCredits(); break;
            }
        }
    }
    
    void ShowToolbar()
    {
        BeginPage(300, 300);
        toolbarInt = GUILayout.Toolbar(toolbarInt, toolbarstrings);
        switch (toolbarInt)
        {
            case 0: VolumeControl(); break;
            case 1: Qualities(); QualityControl(); break;
            case 2: ShowDevice(); break;
        }
        EndPage();
    }

    void ShowCredits()
    {
        BeginPage(300, 300);
        foreach (string credit in credits)
        {
			 GUILayout.Label(credit);
        }
        EndPage();
    }

    void ShowBackButton()
    {
        if (GUI.Button(new Rect(20, Screen.height - 50, 50, 20), "Back"))
        {
            currentPage = Page.Main;
        }
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
        GUILayout.BeginArea(new Rect((Screen.width - width) / 2, (Screen.height - height) / 2, width, height));
    }

    void EndPage()
    {
        GUILayout.EndArea();
        if (currentPage != Page.Main)
        {
            ShowBackButton();
        }
    }
    
    bool IsBeginning()
    {
        return (Time.time < startTime);
    }

    void MainPauseMenu()
    {
        BeginPage(200, 200);
        if (GUILayout.Button(IsBeginning() ? "Play" : "Continue"))
        {
            UnPauseGame();
        }
        if (GUILayout.Button("Options"))
        {
            currentPage = Page.Options;
        }
        if (GUILayout.Button("Credits"))
        {
            currentPage = Page.Credits;
        }
        if (GUILayout.Button("Quit"))
        {
            Application.Quit();
        }
        EndPage();
    }
    
    void PauseGame()
    {
        savedTimeScale = Time.timeScale;
        Time.timeScale = 0;
        AudioListener.pause = true;

        currentPage = Page.Main;
    }

    void UnPauseGame()
    {
        Time.timeScale = savedTimeScale;
        AudioListener.pause = false;
        currentPage = Page.None;

        if (IsBeginning() && start != null)
        {
            start.SetActive(true);
        }
    }

    bool IsGamePaused()
    {
        return (Time.timeScale == 0);
    }

    void OnApplicationPause(bool pause)
    {
        if (IsGamePaused())
        {
            AudioListener.pause = true;
        }
    }
}
