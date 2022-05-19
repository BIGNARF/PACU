using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using System.Reflection;
using System;



public class GeometrySetter : MonoBehaviour
{
    public GameObject popup;
    public string Function;
    public string variable;
    public SuspProfile profile;
    public Text valueshow;

    private void Update()
    {
        var value = profile.GetType().GetField(variable).GetValue(profile);
        //string varValue = profile.GetType().GetField(variable).GetValue(profile);
        float v2 = (float)value;
        valueshow.text = string.Format("{0:N2}", v2);
    }
    public void OpenPopUp()// creates a popup prefab and passes the function that is should run.
    {
        var instance=Instantiate(popup);
        instance.GetComponent<SetterPopUp>().function = Function;
        instance.GetComponent<SetterPopUp>().profile = profile;
    }
}
