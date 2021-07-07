using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class axel2 : MonoBehaviour
{
    #region inputs area
    //set profile stuff
    public axelprofile profile;

    //simulation stuff
    public float ay; // positive toward right
    public float steer; //angle in degrees
    #endregion

    #region outputs area

    public Vector3 local_force;
    #endregion

    #region internal/ setup stuff
    //code bulshit
    public tyre2 left, right;
    private Vector3 force_sum;
    
    void Start()
    {
        ay = 0;
        setTrack();
    }

    private void setTrack()
    {
        var y_position = 0.5f * profile.t / 1000f; // cut it in half and mm to m convertion
        left.gameObject.transform.localPosition = new Vector3(-y_position, 0, 0);
        right.gameObject.transform.localPosition = new Vector3(y_position, 0, 0);
    }
    #endregion
    void FixedUpdate()
    {
        if(Mathf.Abs(ay)<0.5f)
        {
            ay = 0;
        }
        profile.WeightTransfer(ay);
        WeightTransfer();
        WheelSteer();
        SetCamber();
        force_sum = left.Lateral_Force + right.Lateral_Force;
        local_force = gameObject.transform.InverseTransformDirection(force_sum);
    }

    private void WeightTransfer()
    {
        left.Normal_force = profile.SetNormalForce(true);
        right.Normal_force = profile.SetNormalForce(false);
    }//calcs weight transfer based on current ay
    private void WheelSteer()
    {
        //wsteer_left = profile.Steering(steer,true);
        //left.wsteer = wsteer_left;
        left.wsteer = profile.Steering(steer, true);

        //wsteer_right = profile.Steering(steer,false);
        //right.wsteer = wsteer_right;
        right.wsteer = profile.Steering(steer, false);
    }//sets wheel angle steering position
    private void SetCamber()
    {
        var steer = left.wsteer;
        left.camber = profile.SetCamber(steer, true);
        steer = right.wsteer;
        right.camber = profile.SetCamber(steer, false);
    }//set wheel camber to each wheel 
}
