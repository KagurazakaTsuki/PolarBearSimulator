using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartingScreen : MonoBehaviour
{
    public static void StartGame()
    {
        SceneManager.LoadScene("Level1");
    }
}