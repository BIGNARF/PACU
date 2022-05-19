using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class lineshow : MonoBehaviour
{
    public Transform point1, point2;
    private LineRenderer line;

    // Update is called once per frame
    private void Start()
    {
        line = gameObject.GetComponent<LineRenderer>();
    }
    void Update()
    { 
        var points = new Vector3[2];
        points[0] = point1.position;
        points[1] = point2.position;
        line.SetPositions(points);
    }
}
