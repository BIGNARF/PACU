using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using System.Reflection;
using System;

public class SetterPopUp : MonoBehaviour
{
    public string function;
    public InputField value;
    public SuspProfile profile;




    public void SetValue()//called by buttom
    {
        var method= profile.GetType().GetMethod(function, new[] { typeof(float) });
        string v2 = value.text;
        var number = float.Parse(v2);
        method.Invoke(profile, new object[] { number });
        Close();
    }

    public void Close()
    {
        //Debug.Log("Destroying");
        Destroy(this.gameObject);
    }
}
