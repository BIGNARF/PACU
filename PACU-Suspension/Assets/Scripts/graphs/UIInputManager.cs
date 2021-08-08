using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInputManager : MonoBehaviour
{
    public Simulation_Output raw;

        private void Start()
    {
        raw.SortByVelocity();

        foreach (datapoint i in raw.data)
        {
            Debug.Log("VELOCIDADE:" + i.v + "SI:" + i.SI);
        }
    }
}
