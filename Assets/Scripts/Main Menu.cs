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

   private void Update()
   {
      
   }

   private void OnTriggerEnter2D(Collider2D collision)
   {
      if (start && collision.CompareTag("Player"))
      {
         SceneManager.LoadScene(1);
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
