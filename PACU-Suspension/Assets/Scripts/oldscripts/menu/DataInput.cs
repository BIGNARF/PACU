using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using System.Reflection;
using System;

public class DataInput : MonoBehaviour
{
    public ScriptableObject target;//car data object, I think
    public string VarName;
    public bool Number;
    // Start is called before the first frame update
    void Start()
    {
        VarName = gameObject.name;
        Type type = gameObject.GetComponent<InputField>().text.GetType(); 
        var value = Convert.ChangeType(target.GetType().GetField(VarName).GetValue(target), type);
        gameObject.GetComponent<InputField>().text = string.Format("{0:N2}", (string)value);
    }
    public void UpdateValue()
    {
        //string newVar = (string)tyre.GetType().GetField("tyrename").GetValue(tyre);
        if (Number)
        {
            var Value = float.Parse(gameObject.GetComponent<InputField>().text);
            target.GetType().GetField(VarName).SetValue(target, Value);
        }
        else 
        {
            var Value = gameObject.GetComponent<InputField>().text;
            target.GetType().GetField(VarName).SetValue(target, Value);
        }
        
    }
}
