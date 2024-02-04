using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void GoBack()
    {
        SceneManager.LoadScene(4);
    }

    public void GoEndless()
    {
        SceneManager.LoadScene(2);
    }
}
