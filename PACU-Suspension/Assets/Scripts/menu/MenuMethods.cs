using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuMethods :MonoBehaviour
{
    public void End()
    {
        Debug.Log("QUITTED");
        Application.Quit();
    }

    public void LaunchSym()
    {
        SceneManager.LoadScene(1);
    }
    
}
