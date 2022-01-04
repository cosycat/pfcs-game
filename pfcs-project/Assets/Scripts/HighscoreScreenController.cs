using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using TMPro;

public class HighscoreScreenController : MonoBehaviour {


    /* HIGHSCORE */

    [SerializeField] private Canvas highscoreScreen;
    [SerializeField] private TextMeshProUGUI yourScoreText;
    [SerializeField] private TextMeshProUGUI totalHighscoreText;


    public string CurrentScoreAsText {
        get => ScoreTracker.Instance.ScoreTimerAsText;
    }

    public float CurrentScore {
        get => ScoreTracker.Instance.ScoreTimer;
    }

    public void ShowHighscore() {
        yourScoreText.text = "Your Time:\n" + CurrentScoreAsText;
        Debug.Log("ScoreTracker.Instance.GameStatus = " + ScoreTracker.Instance.GameStatus);
        Debug.Log("CurrentScore = " + CurrentScore);
        
        float lastHighscore = ScoreTracker.Instance.Higscore;
        Debug.Log("lastHighscore = " + lastHighscore);

        if (lastHighscore <= 0) {
            // First time played
            ScoreTracker.Instance.Higscore = CurrentScore;
            totalHighscoreText.text = "NEW HIGHSCORE!";
        } else if (lastHighscore > CurrentScore) {
            // Beat last highscore
            totalHighscoreText.text = "NEW HIGHSCORE!\nPrevious Highscore: " + ScoreTracker.GetTimeInNiceFormat(lastHighscore);
            ScoreTracker.Instance.Higscore = CurrentScore;
        } else {
            // No new Highscore
            totalHighscoreText.text = "Best Time:\n" + ScoreTracker.GetTimeInNiceFormat(lastHighscore);
        }
        highscoreScreen.gameObject.SetActive(true);
    }


    // Start is called before the first frame update
    void Start() {
        ShowHighscore();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetButtonDown("Submit") || Input.GetButtonDown("Cancel")) {
            SceneManager.LoadScene(ScoreTracker.Instance.mainMenuScene);
        }
    }
}
