using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;using Update = UnityEngine.PlayerLoop.Update;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    private Vector3 playerPos;
    // Start is called before the first frame update
    void Awake()
    {
        if (_instance is not null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;

        if (SceneManager.GetActiveScene().buildIndex != 1)
        {
            DontDestroyOnLoad(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 Chase(bool asked)
    {
        if (asked)
        {
            return playerPos;
        }
        return playerPos;
    }

    public void PlayerPos(Vector3 pos)
    {
        playerPos = pos;
    }
}
