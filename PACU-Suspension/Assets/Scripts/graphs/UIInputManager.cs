using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIInputManager : MonoBehaviour
{
    public Simulation_Output raw;
    public Text ymax, ymin, xmax;
    public UILineRenderer line;
    public UIZeroRenderer zero;
    public float range,offset,scale;

        private void Start()
    {
        raw.SortByVelocity();
        
        /*foreach (datapoint i in raw.data)
        {
            Debug.Log("VELOCIDADE:" + i.v + "SI:" + i.SI);
        }*/
    }

    private void DefineUnitValue(List<float> channel)
    {
        var max=channel.Max(z => z);
        var min = channel.Min(z => z);
        if (min>0f)
        {
            min = 0f;
        }
        range = max - min;
        offset = Mathf.Abs(min);
    }
}
