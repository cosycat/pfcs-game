using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum GameStatus {
    MENU,
    PLAYING,
    HIGHSCORE,
    GAMEOVER
}

public class ScoreTracker : MonoBehaviour
{
    
    public static ScoreTracker Instance { get; private set; }
    
    public int mainMenuScene = 0;
    public int firstPlayScene = 1;
    public int lastPlayScene = 2;
    public int highscoreScene = 3;
    public int gameOverScene = -1;
    
    private GameStatus gameStatus = GameStatus.MENU;
    public GameStatus GameStatus {
        get => gameStatus; set {
            if (gameStatus == value) {
                return;
            }
            GameStatus oldValue = gameStatus;
            gameStatus = value;
            OnGameStatusChanged(oldValue, value);
        }
    }
    
    public float DelayedStartPerLevel => 0.5f;
    
    public void LoadNextLevel() {
        try {
            Debug.Log(SceneManager.GetSceneByBuildIndex(SceneManager.GetActiveScene().buildIndex + 1));
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        } catch (Exception ex) {
            SceneManager.LoadScene(0);
        }
    }

	public void OpenMainMenu()
    {
        GameStatus = GameStatus.MENU;
        try {
            SceneManager.LoadScene(mainMenuScene);
        } catch (Exception ex) {
            SceneManager.LoadScene(0);
        }
	}

    public void LoadGameOver()
    {
        try {
            Debug.Log(SceneManager.GetSceneByBuildIndex(gameOverScene));
            SceneManager.LoadScene(gameOverScene);
        } catch (Exception ex) {
            SceneManager.LoadScene(0);
        }
    }
    
    /* TIMER */

    [SerializeField] private Canvas HUD;
    [SerializeField] private TextMeshProUGUI timerText;

    public float ScoreTimer { get; private set; }
    public string ScoreTimerAsText {
        get {
            if (ScoreTimer >= 60) {
                if (ScoreTimer >= 10 * 60) {
                    return TimeSpan.FromSeconds(ScoreTimer).ToString(@"hh\:mm\:ss\:ff");
                }
                return TimeSpan.FromSeconds(ScoreTimer).ToString(@"m\:ss\:ff");
            }
            return TimeSpan.FromSeconds(ScoreTimer).ToString(@"ss\:ff");
        }
    }

    public static string GetTimeInNiceFormat(float seconds) {
        if (seconds >= 60) {
            if (seconds >= 10 * 60) {
                return TimeSpan.FromSeconds(seconds).ToString(@"hh\:mm\:ss\:ff");
            }
            return TimeSpan.FromSeconds(seconds).ToString(@"m\:ss\:ff");
        }
        return TimeSpan.FromSeconds(seconds).ToString(@"ss\:ff");
    }

    private bool timerIsRunning = false;


    public void ResetTimer() {
        timerIsRunning = false;
        ScoreTimer = 0;
    }

    public void StartTimer() {
        timerIsRunning = true;
    }

    public void PauseTimer() {
        timerIsRunning = false;
    }
    
    
    /* HIGHSCORE */

    public float Higscore {
        get {
            return PlayerPrefs.GetFloat("Highscore", -1);
        }
        set {
            PlayerPrefs.SetFloat("Highscore", value);
        }
    }

    
    /* START */
    
    private void Awake() {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("ScoreTracker");

        if (objs.Length > 1) {
            Destroy(this.gameObject);
        } else {
            DontDestroyOnLoad(this.gameObject);
            Instance = this;
            HUD.gameObject.SetActive(false);
        }


    }

    void Start() {
        Debug.Log("ScoreTracker Start");
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }

    void Update() {
        if (timerIsRunning) {
            ScoreTimer += Time.deltaTime;
            timerText.text = "<mark=#a1a1a1aa>Time: " + ScoreTimerAsText + "</mark>"; // TODO: better formatting
        } else {
            if (GameStatus == GameStatus.PLAYING) {
                if (Time.timeSinceLevelLoad > DelayedStartPerLevel && (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)) {
                    Debug.Log("ScoreTracker: ButtonDown");
                    StartTimer();
                }
            } else if (GameStatus == GameStatus.MENU) {
                timerText.text = "<mark=#a1a1a1aa>Time: 00:00</mark>"; // TODO: better formatting
            }
        }

        if (GameStatus == GameStatus.PLAYING)
        {
            if (Input.GetButtonDown("Cancel"))
            {
                OpenMainMenu();
            }
        }
    }
    
    /* EVENTS */
    
    public event EventHandler<GameStatusEventArgs> GameStatusChanged;

    protected virtual void OnGameStatusChanged(GameStatus oldStatus, GameStatus newStatus) {
        GameStatusChanged?.Invoke(this, new GameStatusEventArgs(oldStatus, newStatus));

        Debug.Log("OnGameStatusChanged: " + oldStatus + ", " + newStatus);

        if (newStatus == GameStatus.MENU) {
            ResetTimer();
        }
        if (newStatus == GameStatus.PLAYING) {
            HUD.gameObject.SetActive(true);
        }
        if (newStatus != GameStatus.PLAYING) {
            HUD.gameObject.SetActive(false);
        }
        // if (newStatus == GameStatus.HIGHSCORE) {
        //     ShowHighscore();
        // }
        // if (newStatus != GameStatus.HIGHSCORE) {
        //     HideHighscore();
        // }
    }

    private void OnActiveSceneChanged(Scene arg0, Scene arg1) {
        Debug.Log("OnActiveSceneChanged: " + arg0.buildIndex + ", " + arg1.buildIndex);
        if (arg1.buildIndex == mainMenuScene) {
            GameStatus = GameStatus.MENU;
        } else if (arg1.buildIndex >= firstPlayScene && arg1.buildIndex <= lastPlayScene) {
            GameStatus = GameStatus.PLAYING;
        } else if (arg1.buildIndex == highscoreScene) {
            GameStatus = GameStatus.HIGHSCORE;
        } else if (arg1.buildIndex == gameOverScene) {
            GameStatus = GameStatus.GAMEOVER;
        } else {
            Debug.Log("Scene index error: " + arg1.buildIndex);
        }

        if (GameStatus == GameStatus.PLAYING) {
            PauseTimer();
        }
    }
    
}



public class GameStatusEventArgs {
    public GameStatusEventArgs(GameStatus oldStatus, GameStatus newStatus) {
        this.OldStatus = oldStatus;
        this.NewStatus = newStatus;
    }

    public GameStatus NewStatus { get; }

    public GameStatus OldStatus { get; }
}