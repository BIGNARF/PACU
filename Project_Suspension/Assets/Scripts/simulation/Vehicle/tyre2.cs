using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tyre2 : MonoBehaviour //Uses the selected tyre profile to calc lateral force inside simulation
{
    #region input area
    //stuff for profiling
    public tyreprofile tyre;

    //stuff for simulating
    public float wsteer,camber, Normal_force;
    #endregion

    #region output area
    //output
    public Vector3 Lateral_Force;
    #endregion

    #region internal/ setup
    //internal stuff
    //public bool left;
    private Transform body;
    private Vector3 pos1;
    public Vector3 speed;

    void Start()
    {
        body = gameObject.transform;
        Lateral_Force = Vector3.zero;
        speed = Vector3.zero;
        pos1 = body.position;
        wsteer = 0;
    }// setup of code bullshit
    #endregion

    void FixedUpdate()
    {
        WheelSteering();
        speedcalc();
        var camber_r = Mathf.Deg2Rad * camber;
        var slip = Vector3.SignedAngle(body.forward, speed,body.up);
        //Debug.Log(slip);
        var slip_r = Mathf.Deg2Rad * slip;

        Lateral_Force = body.transform.right * tyre.LatForce(slip_r, camber_r, Normal_force);

    }// calcs speed vector, updates the slip angle and calcs the lateral force

    private  void WheelSteering()
    {
        body.transform.localEulerAngles = (new Vector3(0,wsteer,0));
    }//sets relative angle of wheel to chassis

    private void speedcalc()
    {
        var pos2 = body.position;
        speed = (1 / Time.fixedDeltaTime) * (pos2 - pos1);
        pos1 = pos2;
    }// calcs speed vector
}
