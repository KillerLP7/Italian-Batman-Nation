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
   static public bool skip;

   private void Update()
   {

      if (SceneManager.GetActiveScene().buildIndex > 3)
      {
         if (skip)
         {
            SceneManager.LoadScene(1);
         }
      }
      
     if (Input.GetKeyDown(KeyCode.Return))
     {
        SceneManager.LoadScene(1);
        skip = true;
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
