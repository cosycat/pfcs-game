using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public int firstSceneBuildIndex;

    public void PlayGame() {
        SceneManager.LoadScene(firstSceneBuildIndex);
    }

    public void QuitGame() {
        Debug.Log("Application Quit!");
        Application.Quit();
    }


    public void ResetHighscore()
    {
        Debug.Log("ResetHighscore");
        ScoreTracker.Instance.Higscore = -1;
    }
    

    private void Update() {
        if (Input.GetButtonDown("Submit")) {
            PlayGame();
        } else if (Input.GetButtonDown("Cancel")) {
            QuitGame();
        }
    }

}
