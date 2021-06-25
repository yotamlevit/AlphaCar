using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WelcomeButtonHeandler : MonoBehaviour
{
    public void StartGame()
    {
        Debug.Log("Start Sim");
        SceneManager.LoadScene("SandBox");
    }

    public void Quit()
    {
        Debug.Log("Quit app");
        Application.Quit();
    }

    public void Instraction()
    {
        Debug.Log("Instractions");
        SceneManager.LoadScene("Instractions");
    }

    public void AboutTheProgram()
    {
        Debug.Log("About The Program");
        SceneManager.LoadScene("AboutAlphaCar");
    }
}
