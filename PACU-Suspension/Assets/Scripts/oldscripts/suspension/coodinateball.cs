using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

public class coodinateball : MonoBehaviour
{
    public SuspProfile target;
    public Transform ball;
    public string structname;
    public string VarName;
    public Vector3 currentposition;

    private void Start()
    {
        ball = gameObject.GetComponent<Transform>();
        VarName = gameObject.name;
        currentposition = ball.position;
    }

    private void Update()
    {
        Type type = target.GetType().GetField(structname).GetValue(target).GetType();
        var temp = Convert.ChangeType(target.GetType().GetField(structname).GetValue(target), type);
        var data = temp.GetType().GetField(VarName).GetValue(temp);
        var value = (Vector3)data;
        //Debug.Log(data);
        if(ChangeTest(value))
        {
            Updateposition(value);
        } 
    }

    private bool ChangeTest(Vector3 newpos)
    {
        if (currentposition == newpos)
            return false;
        else
            return true;
    }
    public void Updateposition(Vector3 value)
    {
        ball.position = value;///10;
    }
}
