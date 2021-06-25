using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InstractionButtonBind : MonoBehaviour
{
    public void BackToStart()
    {
        Debug.Log("Back to open Screen");
        SceneManager.LoadScene("Welcome");
    }
}
