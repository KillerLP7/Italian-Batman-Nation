using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
   [SerializeField] private bool start;
   [SerializeField] private bool endless;
   [SerializeField] private bool options;
   [SerializeField] private bool quit;
   static public bool skipIntro;
   static public bool skipOutro;
   [SerializeField] private bool intro;
   [SerializeField] private bool outro;

   private void Update()
   {

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
