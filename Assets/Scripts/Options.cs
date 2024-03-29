using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    public const string diffKey = "Difficulty";
    public const string audioKey = "Audio";
    public const string speedKey = "Speed";
    public const string endlessModeKey = "Endless";
    
    [SerializeField] private Slider ASlider;
    [SerializeField] private Slider DSlider;
    [SerializeField] private Slider SSlider;
    [SerializeField] private GameObject endlessButton;
    [SerializeField] private TextMeshProUGUI percent;
    [SerializeField] private TextMeshProUGUI diff;
    [SerializeField] private TextMeshProUGUI speed;
    public AudioSource src;
    private static int difficulty;
    private static int timeScale;
    private static float audioValue;
    private static int endless;

    private void Awake()
    {
        src.volume = PlayerPrefs.GetFloat(Options.audioKey, 1) / 2;
        audioValue =  PlayerPrefs.GetFloat(audioKey, 1);
        difficulty =  PlayerPrefs.GetInt(diffKey, 0);
        timeScale =  PlayerPrefs.GetInt(speedKey, 2);
        endless = PlayerPrefs.GetInt(endlessModeKey, 0);
        if (endless == 0)
        {
            endlessButton.SetActive(false);
        }
        else if (endless == 1)
        {
            endlessButton.SetActive(true);
        }
        ASlider.value = audioValue;
        DSlider.value = difficulty;
        SSlider.value = timeScale;
    }

    private void Update()
    {
        src.volume = ASlider.value / 2;
    }

    public void Audio()
    {
        percent.text = ASlider.value.ToString("0%");
        audioValue = ASlider.value;
        //GameManager.Instance.SFX(3, slider.value);
    }
    
    public void Difficulty()
    {
        difficulty = (int) DSlider.value;
        switch (difficulty)
        {
            case 0:
                diff.text = "Easy";
                break;
            case 1:
                diff.text = "Normal";
                break;
            case 2:
                diff.text = "Hard";
                break;
        }
    }
    
    public void GameSpeed()
    {
        timeScale = (int) SSlider.value;
        switch (SSlider.value)
        {
            case 0:
                speed.text = "0.5";
                Time.timeScale = 0.5f;
                break;
            case 1:
                speed.text = "0.75";
                Time.timeScale = 0.75f;
                break;
            case 2:
                speed.text = "1";
                Time.timeScale = 1;
                break;
            case 3:
                speed.text = "1.5";
                Time.timeScale = 1.5f;
                break;
            case 4:
                speed.text = "2";
                Time.timeScale = 2;
                break;
        }
    }
    
    public static void OnExit()
    {
        PlayerPrefs.SetFloat(audioKey, audioValue);
        PlayerPrefs.SetInt(diffKey, difficulty);
        PlayerPrefs.SetInt(speedKey, timeScale);
    }
}
