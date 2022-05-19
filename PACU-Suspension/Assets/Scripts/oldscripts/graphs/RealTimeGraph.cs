using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class RealTimeGraph : MonoBehaviour
{
    public Simulation_Output raw;
    public Text ymax, ymin, xmax;
    public UILineRenderer line;
    public UIZeroRenderer zero;
    public float range,offset;
    //public Vector2 scale; // x scale and y scale

    public float height, lenght;

    private List<float> yvalues= new List<float>();
    private List<float> xvalues = new List<float>();

    private void Start()
    {
        raw.SortByVelocity();
        
        //



        /*foreach (datapoint i in raw.data)
        {
            Debug.Log("VELOCIDADE:" + i.v + "SI:" + i.SI);
        }*/
    }

    private void DefineY(List<float> channel)
    {
        var max=channel.Max(z => z);
        var min = channel.Min(z => z);
        if (min>0f)
        {
            min = 0f;
        }
        range = max - min;
        offset = Mathf.Abs(min);
        var scale = height / range;
        yvalues.Clear();
        foreach (float i in channel)
            yvalues.Add(i * scale);

    }

    private void DefineX(List<float> channel)
    {
        var max = channel.Max(z => z);
        /*var min = channel.Min(z => z);
        if (min > 0f)
        {
            min = 0f;
        }*/
        range = max - 0f;
        //offset = Mathf.Abs(min);
        var scale = height / range;
        yvalues.Clear();
        foreach (float i in channel)
            yvalues.Add(i * scale);

    }
}
