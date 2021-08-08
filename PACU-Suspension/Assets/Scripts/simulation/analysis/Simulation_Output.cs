using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Simulation_Output : ScriptableObject
{

    public List<datapoint> data;//= new List<datapoint>();
    public void ClearData()
    {
        data.Clear();
    }//resets stored data points
    public void GetData(datapoint current)
    {
        data.Add(current);
        var i = data.Count - 1;
    }

    public void SortByVelocity()
    {
        GFG gg = new GFG();
        data.Sort(gg);
    }

    class GFG : IComparer<datapoint>
    {
        public int Compare(datapoint x, datapoint y)
        {
            if (x.v == 0 || y.v == 0)
            {
                return 0;
            }

            // CompareTo() method
            return x.v.CompareTo(y.v);

        }
    }
}


[System.Serializable]
public class datapoint
{
    public float v, ax, ay, theta, beta, beta_d, SI;//, SM;

    public datapoint(float _v, float _ax, float _ay, float _theta, float _beta, float _SI)//, float _SM)
    {
        v = _v;
        ax = _ax;
        ay = _ay;
        theta = _theta;
        beta = _beta;
        SI = _SI;
        // SM = _SM;
    }
}//perhaps add a force rms later, and create a new class based on frequency instead of velocity


