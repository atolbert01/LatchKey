using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOver : MonoBehaviour {

    public GUIStyle guiStyle;

    private Color color;

    private int playerScore;

    private string stringToEdit;

    public enum Page
    {
        PlayerScore, Quit
    }

    private Page currentPage;

    // Use this for initialization
    void Start ()
    {
        playerScore = PlayerController.totalScore;
        color = Color.white;
        currentPage = Page.PlayerScore;
        stringToEdit = "Enter Your Name Here";
    }
	
	// Update is called once per frame
    /*
	void Update ()
    {
	}*/

    void OnGUI()
    {
        GUI.color = color;
        switch (currentPage)
        {
            case Page.PlayerScore: displayText(); break;
            case Page.Quit: quitPage(); break;
        }
    }

    void displayText()
    {
        int width = 600;
        int height = 60;

        GUI.Label(new Rect((Screen.width - width) / 2, 120, width, height), "Final Score: " + playerScore, guiStyle);
        if (highScorer())
        {
            GUI.Label(new Rect((Screen.width - width) / 2, 200, width, height), "NEW HIGH SCORE!", guiStyle);
            GUI.Box(new Rect((Screen.width - width) / 2, 400, width, height), "");
            stringToEdit = GUI.TextField(new Rect((Screen.width - width) / 2, 400, width, height), stringToEdit, 20, guiStyle);
        }
        else
            GUI.Label(new Rect((Screen.width - width) / 2, 200, width, height), "Bummer!", guiStyle);


        if (GUI.Button(new Rect(Screen.width - 100, Screen.height - 100, 50, 25), "Next"))
        {
            currentPage = Page.Quit;
            newHighScorer();
        }

    }

    void newHighScorer()
    {
        int tempNum = 0;
        string tempString = "";

        if (playerScore > PlayerPrefs.GetInt("num1Player_highscore"))
        {
            tempNum = PlayerPrefs.GetInt("num4Player_highscore");
            tempString = PlayerPrefs.GetString("num4Player");
            PlayerPrefs.SetInt("num5Player_highscore", tempNum);
            PlayerPrefs.SetString("num5Player", tempString);

            tempNum = PlayerPrefs.GetInt("num3Player_highscore");
            tempString = PlayerPrefs.GetString("num3Player");
            PlayerPrefs.SetInt("num4Player_highscore", tempNum);
            PlayerPrefs.SetString("num4Player", tempString);

            tempNum = PlayerPrefs.GetInt("num2Player_highscore");
            tempString = PlayerPrefs.GetString("num2Player");
            PlayerPrefs.SetInt("num3Player_highscore", tempNum);
            PlayerPrefs.SetString("num3Player", tempString);

            tempNum = PlayerPrefs.GetInt("num1Player_highscore");
            tempString = PlayerPrefs.GetString("num1Player");
            PlayerPrefs.SetInt("num2Player_highscore", tempNum);
            PlayerPrefs.SetString("num2Player", tempString);

            PlayerPrefs.SetInt("num1Player_highscore", playerScore);
            PlayerPrefs.SetString("num1Player", stringToEdit);
        }
        else if (playerScore > PlayerPrefs.GetInt("num2Player_highscore"))
        {

            tempNum = PlayerPrefs.GetInt("num4Player_highscore");
            tempString = PlayerPrefs.GetString("num4Player");
            PlayerPrefs.SetInt("num5Player_highscore", tempNum);
            PlayerPrefs.SetString("num5Player", tempString);

            tempNum = PlayerPrefs.GetInt("num3Player_highscore");
            tempString = PlayerPrefs.GetString("num3Player");
            PlayerPrefs.SetInt("num4Player_highscore", tempNum);
            PlayerPrefs.SetString("num4Player", tempString);

            tempNum = PlayerPrefs.GetInt("num2Player_highscore");
            tempString = PlayerPrefs.GetString("num2Player");
            PlayerPrefs.SetInt("num3Player_highscore", tempNum);
            PlayerPrefs.SetString("num3Player", tempString);

            PlayerPrefs.SetInt("num2Player_highscore", playerScore);
            PlayerPrefs.SetString("num2Player", stringToEdit);
        }
        else if (playerScore > PlayerPrefs.GetInt("num3Player_highscore"))
        {

            tempNum = PlayerPrefs.GetInt("num4Player_highscore");
            tempString = PlayerPrefs.GetString("num4Player");
            PlayerPrefs.SetInt("num5Player_highscore", tempNum);
            PlayerPrefs.SetString("num5Player", tempString);

            tempNum = PlayerPrefs.GetInt("num3Player_highscore");
            tempString = PlayerPrefs.GetString("num3Player");
            PlayerPrefs.SetInt("num4Player_highscore", tempNum);
            PlayerPrefs.SetString("num4Player", tempString);

            PlayerPrefs.SetInt("num3Player_highscore", playerScore);
            PlayerPrefs.SetString("num3Player", stringToEdit);
        }
        else if (playerScore > PlayerPrefs.GetInt("num4Player_highscore"))
        {

            tempNum = PlayerPrefs.GetInt("num4Player_highscore");
            tempString = PlayerPrefs.GetString("num4Player");
            PlayerPrefs.SetInt("num5Player_highscore", tempNum);
            PlayerPrefs.SetString("num5Player", tempString);

            PlayerPrefs.SetInt("num4Player_highscore", playerScore);
            PlayerPrefs.SetString("num4Player", stringToEdit);
        }
        else
        {
            PlayerPrefs.SetInt("num5Player_highscore", playerScore);
            PlayerPrefs.SetString("num5Player", stringToEdit);
        }
    }

    void quitPage()
    { 
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
        int width = 125;
        int height = 60;
        int posy = 125;
        int posx = (Screen.width - width) / 2;

        GUI.Label(new Rect(posx, posy, width, height), "Highscores");
        GUI.Label(new Rect(posx - width, posy + height, width, height), num1Player);
        GUI.Label(new Rect(posx + width, posy + height, width, height), num1Player_highscore.ToString());
        GUI.Label(new Rect(posx - width, posy + (height * 2), width, height), num2Player);
        GUI.Label(new Rect(posx + width, posy + (height * 2), width, height), num2Player_highscore.ToString());
        GUI.Label(new Rect(posx - width, posy + (height * 3), width, height), num3Player);
        GUI.Label(new Rect(posx + width, posy + (height * 3), width, height), num3Player_highscore.ToString());
        GUI.Label(new Rect(posx - width, posy + (height * 4), width, height), num4Player);
        GUI.Label(new Rect(posx + width, posy + (height * 4), width, height), num4Player_highscore.ToString());
        GUI.Label(new Rect(posx - width, posy + (height * 5), width, height), num5Player);
        GUI.Label(new Rect(posx + width, posy + (height * 5), width, height), num5Player_highscore.ToString());


        if (GUI.Button(new Rect(25, Screen.height - 100, 75, 25), "Main Menu"))
        {
            SceneManager.LoadScene("Intro Scene");
        }
        if (GUI.Button(new Rect(Screen.width - 100, Screen.height - 100, 75, 25), "Quit"))
        {
            Application.Quit();
        }
    }

    bool highScorer()
    {
        if(PlayerPrefs.HasKey("num5Player_highscore"))
        {
            if (playerScore > PlayerPrefs.GetInt("num5Player_highscore"))
            {
                return true;
            }
            else
                return false;
        }
        else
            return false;
    }
}
