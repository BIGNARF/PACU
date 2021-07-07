using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Simulation_Output : ScriptableObject
{
    public List<Vector2> Speedxacc;

    public void ClearData()
    {
        Speedxacc.Clear();
    }//resets stored data points
    public void GetData(float Speed, float acc)
    {
        Speedxacc.Add(new Vector2(Speed, acc));
    }
}
