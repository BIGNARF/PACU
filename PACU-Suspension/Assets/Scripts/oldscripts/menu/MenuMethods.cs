using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MenuMethods :MonoBehaviour
{
    public GeneralProfile profile;
    public Dropdown tyrename;
    public void End()
    {
        Debug.Log("QUITTED");
        Application.Quit();
    }

    public void LaunchSym()// launches real time symulation
    {
        SceneManager.LoadScene(1);
    }

    public void LaunchSusp()// launches suspension project screen
    {
        SceneManager.LoadScene(2);
    }

    public void LaunchMenu()// launches suspension project screen
    {
        SceneManager.LoadScene(0);
    }

}
