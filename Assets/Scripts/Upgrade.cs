using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

[RequireComponent(typeof(CanvasGroup))]
public class Upgrade : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] slotTexts;
    private CanvasGroup group;
    private PowerUpType[] slots;
    private int time;
    private bool isOpen;

    public enum PowerUpType
    {
        Attack, Health, BCooldown, BDamage, Regeneration, Points
    }

    private void Update()
    {
        if (isOpen)
        {
            Time.timeScale = 0;
        }
    }

    private void Awake()
    {
        group = GetComponent<CanvasGroup>();
        slots = new PowerUpType[3];
    }

    public void OpenScreen()
    {
        isOpen = true;
        Time.timeScale = 0;
        group.alpha = 1f;
        group.interactable = group.blocksRaycasts = true;
        List<int> numbers = new List<int> {0, 1, 2, 3, 4, 5};
        for (int i = 0; i < 3; i++)
        {
            int index = Random.Range(0, numbers.Count);
            slots[i] = (PowerUpType)numbers[index]; 
            slotTexts[i].text = numbers[index].ToString();
            numbers.RemoveAt(index);
        }
    }

    public void SlotOnePressed()
    {
        GameManager.Instance.PowerUps(slots[0]);
        CloseScreen();
    }
    
    public void SlotTwoPressed()
    {
        GameManager.Instance.PowerUps(slots[1]);
        CloseScreen();
    }
    
    public void SlotThreePressed()
    {
        GameManager.Instance.PowerUps(slots[2]);
        CloseScreen();
    }
    
    private void CloseScreen()
    {
        isOpen = false;
        GameManager.Instance.SaveHP();
        SceneManager.LoadScene(2);
        Time.timeScale = PlayerPrefs.GetFloat(Options.speedKey, 1f);
        group.alpha = 0f;
        group.interactable = group.blocksRaycasts = false;
    }
}
