using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Simulation_Output : ScriptableObject
{


    public List<datapoint> data= new List<datapoint>();

    public void ClearData()
    {
        data.Clear();
    }//resets stored data points
    public void GetData(datapoint current)
    {
        data.Add(current);
        var i = data.Count - 1;
    }
}


