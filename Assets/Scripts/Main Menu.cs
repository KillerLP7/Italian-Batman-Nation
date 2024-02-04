using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
   public const string endlessModeKey = "Endless";
   [SerializeField] private bool start;
   [SerializeField] private bool endless;
   [SerializeField] private bool options;
   [SerializeField] private bool quit;
   [SerializeField] private bool endlessBlock;
   [SerializeField] private GameObject arrow;
   [SerializeField] private GameObject endlessText;
   public AudioSource src;
   static public bool skipIntro;
   static public bool skipOutro;
   [SerializeField] private bool intro;
   [SerializeField] private bool outro;
   private int unlockEndless;

   private void Update()
   {
      if (intro || outro)
      {
         src.volume = PlayerPrefs.GetFloat(Options.audioKey, 1) * 2;
      }
      
      if (start)
      {
         src.volume = PlayerPrefs.GetFloat(Options.audioKey, 1) / 2;
      }
      
      if (SceneManager.GetActiveScene().buildIndex > 3)
      {
         if (skipIntro && intro)
         {
            SceneManager.LoadScene(1);
         }

         if (skipOutro && outro)
         {
            SceneManager.LoadScene(0);
         }
      }

      if (endlessBlock)
      {
         unlockEndless =  PlayerPrefs.GetInt(endlessModeKey, 0);
         if (unlockEndless == 1)
         {
            arrow.SetActive(true);
            endlessText.SetActive(true);
            Destroy(gameObject);
         }

         if (unlockEndless == 0)
         {
            arrow.SetActive(false);
            endlessText.SetActive(false);
         }
      }
      
     if (Input.GetKeyDown(KeyCode.Return))
     {
        if (intro)
        {
           SceneManager.LoadScene(1);
           skipIntro = true;
        }

        if (outro)
        {
           SceneManager.LoadScene(0);
           skipOutro = true;
        }
     }
      
   }

   private void OnTriggerEnter2D(Collider2D collision)
   {
      if (start && collision.CompareTag("Player"))
      {
         SceneManager.LoadScene(4);
      }
      if (endless && collision.CompareTag("Player"))
      {
         GameManager.Instance.ResetEndlessWave();
         SceneManager.LoadScene(2);
      }
      if (options && collision.CompareTag("Player"))
      {
         SceneManager.LoadScene(3);
      }
      if (quit && collision.CompareTag("Player"))
      {
         Application.Quit();
      }
   }
}
