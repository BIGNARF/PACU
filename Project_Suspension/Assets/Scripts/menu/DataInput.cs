using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using System.Reflection;

public class DataInput : MonoBehaviour
{
    public ScriptableObject target;
    public string VarName;
    public bool Number;
    // Start is called before the first frame update
    void Start()
    {
        VarName = gameObject.name;
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
