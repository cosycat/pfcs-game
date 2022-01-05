using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour {

    public static AudioManager Instance { get; private set; }

    [SerializeField]
    private Sound[] sounds;


    private void Awake() {
        if (Instance != null) {
            Destroy(Instance.gameObject);
        }
        Instance = this;

        foreach (Sound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    private void Start() {
        ScoreTracker.Instance.GameStatusChanged += OnGameStatusChanged;
    }

    private void OnDestroy()
    {
        ScoreTracker.Instance.GameStatusChanged -= OnGameStatusChanged;
    }

    private void OnGameStatusChanged(object sender, GameStatusEventArgs e) {
        if (e.NewStatus == GameStatus.PLAYING && e.OldStatus == GameStatus.MENU) {
            // PlaySound("LevelMusic");
        }
        if (e.NewStatus != GameStatus.PLAYING && e.NewStatus != GameStatus.GAMEOVER && e.NewStatus != GameStatus.HIGHSCORE) {
            // StopSound("LevelMusic");
        }
    }

    public void PlaySound(string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) {
            Debug.LogWarning("Sound " + name + " not found!");
            return;
        }
        s.source.Play(0);
        Debug.Log($"PlaySound: {name}");
    }

    public void StopSound(string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) {
            Debug.LogWarning("Sound " + name + " not found!");
            return;
        }
        s.source.Stop();
        Debug.Log($"StopSound: {name}");
    }
}