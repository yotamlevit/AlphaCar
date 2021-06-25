using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitButtonHeandler : MonoBehaviour
{
    public void Quit()
    {
        Debug.Log("Back to open Screen");
        SceneManager.LoadScene("Welcome");
    }
}
