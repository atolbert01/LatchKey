using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class Score : MonoBehaviour {

    public static int[] lvlScores = new int[3];
    int currentScene;
    int totalScore;


    /**
        **** Scoring Table ****
        Level Score = Points - Elapsed Time
        Max points possible (assumes 0 elapsed time)      

        Level 1: 50
            5 boblins x 10 points = 50
        
        Level 2: 45
            2 boblins x 10 points = 20
            1 lobster x 25 points = 25
        
        Level 3: 105
            3 boblins x 10 points = 30
            3 lobsters x 25 points = 75

        Total = 200
    */

    void OnGUI() {
        GUI.Label(new Rect(10, 10, 100, 20), "Score: " + PlayerController.lvlScore.ToString());
        GUI.Label(new Rect(110, 10, 100, 20), "Life: " + PlayerController.lifeCount.ToString());
        GUI.Label(new Rect(210, 10, 100, 20), "Time: " + Mathf.Floor(PlayerController.time).ToString());

        currentScene = SceneManager.GetActiveScene().buildIndex;

        if (currentScene > 0)
        {
            int j = 10;
            for(int i = 0; i < currentScene; i++) {
                
                GUI.Label(new Rect(Screen.width - 150, j, 100, 20), "Level " + (i+1)
                    + ": " + lvlScores[i].ToString());
                j += 20;
            }
            if (currentScene == lvlScores.Length-1 && PlayerController.lvlComplete)
            {
                GUI.Label(new Rect(Screen.width - 150, j, 100, 20), "Level " + (currentScene + 1)
                    + ": " + lvlScores[currentScene].ToString());

                j += 20;
                       
                string rank = "";

                totalScore = PlayerController.totalScore;

                if (totalScore < 30)
                    rank = "Lame!";
                else if (totalScore >= 30 && totalScore < 80)
                    rank = "Rookie";             
                else if (totalScore >= 80 && totalScore < 150)
                    rank = "Semi-Pro";               
                else if (totalScore >= 150)
                    rank = "Pro!";

                j += 20;
                GUI.Label(new Rect(Screen.width - 150, j, 100, 20), "Total Score: " + totalScore.ToString());
                j += 20;
                GUI.Label(new Rect(Screen.width - 150, j, 100, 20), "Rank: " + rank);
            }

        }
    }
}
