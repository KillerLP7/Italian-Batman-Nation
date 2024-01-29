using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(CanvasGroup))]
public class Upgrade : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] slotTexts;
    private CanvasGroup group;
    private PowerUpType[] slots;

    public enum PowerUpType
    {
        Attack, Health, BCooldown, BDamage, Regeneration, Points
    }


    private void Awake()
    {
        group = GetComponent<CanvasGroup>();
        slots = new PowerUpType[3];
    }

    public void OpenScreen()
    {
        Time.timeScale = 0f;
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
        Time.timeScale = PlayerPrefs.GetFloat(Options.speedKey, 1f);
        group.alpha = 0f;
        group.interactable = group.blocksRaycasts = false;
    }
}
