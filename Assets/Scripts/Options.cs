using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    [SerializeField] private Slider ASlider = null;
    [SerializeField] private Slider DSlider = null;
    [SerializeField] private Slider SSlider = null;
    [SerializeField] private TextMeshProUGUI percent;
    [SerializeField] private TextMeshProUGUI diff;
    [SerializeField] private TextMeshProUGUI speed;
    public void Audio()
    {
        percent.text = ASlider.value.ToString("0%");
        //GameManager.Instance.SFX(3, slider.value);
    }

    public void Difficulty()
    {
        switch (DSlider.value)
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
}
