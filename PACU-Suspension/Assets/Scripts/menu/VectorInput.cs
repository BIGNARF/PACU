using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using System.Reflection;
using System;

public class VectorInput : MonoBehaviour
{
    public ScriptableObject target;
    public string VarName;
    public InputField x, y, z;
    public string structname;



    void Start()
    {
        VarName = gameObject.name;
        VectorDisplay();
    }
    public void UpdateValue()
    {
        //note:this code is absolute thrash, maybe its better to change later, but Im sure i fucking wont.
        var Value = new Vector3(float.Parse(x.text), float.Parse(y.text), float.Parse(z.text));
        Type type = target.GetType().GetField(structname).GetValue(target).GetType();
        var temp=Convert.ChangeType(target.GetType().GetField(structname).GetValue(target),type);
        temp.GetType().GetField(VarName).SetValue(temp, Value);
        }

    public void VectorDisplay()
    {
        Type type = target.GetType().GetField(structname).GetValue(target).GetType();
        var temp = Convert.ChangeType(target.GetType().GetField(structname).GetValue(target), type);
        var data = temp.GetType().GetField(VarName).GetValue(temp);
        var value=(Vector3)data;
        x.text = string.Format("{0:N2}", value.x);
        y.text = string.Format("{0:N2}", value.y);
        z.text = string.Format("{0:N2}", value.z);
    }

    public void ChangeTarget(ScriptableObject newtarget)
    {
        target = newtarget;
        this.Start();
    }
}
