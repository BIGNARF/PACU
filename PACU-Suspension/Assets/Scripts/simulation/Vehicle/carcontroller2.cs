using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carcontroller2 : MonoBehaviour    
{
    #region input area
    //user input stuff
    public Simulation_Input Command;
    // stuff for profiling
    public carprofile profile;
    public Vector3 front_force, rear_force; // in N

    public float Motor_force;// inpulsioning engine force, in N
    public float steer;//steering angle in deg

    #endregion region

    #region output area

    public Vector3 Acceleration; // in m/s2
    //public float Speed; // in m/s
    public float Yaw_angle; // in deg, positive in CCW direction from top view
    public float Yaw_moment; // in N.m
    public float SI, SM; // Manouvrability parameters
    public Simulation_Output graph;
    #endregion

    #region internal/ setup stuff
    public axel2 front, rear;   
    public Rigidbody body;


    private Vector3 prev_speed;//speed vector for acc calc
    private float prev_yaw;
    private float prev_ay;
    private float prev_steer;//memories for derivative calcs
    void Start()
    {
        Acceleration = Vector3.zero;
        prev_speed = Vector3.zero;
        prev_yaw = 0;
        prev_ay = 0;
        Command.steer_angle = 0;
        prev_steer = 0;
        Yaw_angle = 0;
        Yaw_moment = 0;
        SetRB();
        SetWheelbase();
        graph.ClearData();
    }

    private void SetRB()
    {
        body.mass = profile.Mass;
        var inertia = Vector3.one;
        inertia.y = profile.Rot_Inertia;
        body.inertiaTensor = inertia;
        // Debug.Log(inertia);
        // Debug.Log(body.inertiaTensor);
    }// note: this inertia thingy may fuck stuff up in the future. for the moment i'll let it be like this 'cause it aint harming anyone.

    private void SetWheelbase()
    {
        var x_position = profile.CGX / 1000;//mm to m convertion
        front.gameObject.transform.localPosition = new Vector3(0, 0, x_position);
        x_position = (profile.CGX - profile.wheelbase) / 1000;
        rear.gameObject.transform.localPosition = new Vector3(0, 0, x_position);
    }

    #endregion
    void FixedUpdate()
    {
        //geting user input
        var acc=Command.GetAccelarationInput();
        body.AddRelativeForce(acc * Vector3.forward);
        steer = Command.GetSteerInput();
        front.steer = steer*profile.bar_ratio;

        //inner simulation stuff
        front_force = front.local_force;
        rear_force = rear.local_force;
        YAWmanager();
        ForceManager();
        OutputManager();
        front.ay = Acceleration.x;
        rear.ay = Acceleration.x;


        // output data stuff
        CalcParameters();
        datapoint point = new datapoint(body.velocity.magnitude, Acceleration.z, Acceleration.x, Command.steer_angle, Yaw_angle, SI);
        graph.GetData(point);
        Debug.Log("   v2:" + graph.data[(graph.data.Count)-1].v);
    }

    private void YAWmanager()
    {
        var YAW = Vector3.Cross( front.gameObject.transform.localPosition, front_force);
        YAW += Vector3.Cross( rear.gameObject.transform.localPosition,rear_force);
        body.AddRelativeTorque(YAW);
        Yaw_moment = YAW.magnitude;
        
    }// calcs and applies yaw moment onto the rigid body

    private void ForceManager()
    {
        var total_force = front_force + rear_force;
        if(total_force.magnitude<300f)// maybe change the tolerances for something more reasonable
        {
            total_force = Vector3.zero;
        }
        body.AddRelativeForce(total_force);
    }// calcs and applies forces onto the rigid body

    private void OutputManager()
    {
        var acc = (body.velocity - prev_speed) / Time.fixedDeltaTime;
        prev_speed = body.velocity;

        Acceleration = gameObject.transform.InverseTransformDirection(acc);//acceleration vector in local coordinates
        Yaw_angle = Vector3.SignedAngle(gameObject.transform.forward, body.velocity, gameObject.transform.up);
        
    }

    private void CalcParameters()
    {
        var day = Acceleration.x - prev_ay;
        prev_ay = Acceleration.x;
        var dyaw = Yaw_moment - prev_yaw;
        prev_yaw = Yaw_moment;
        var dsteer = Command.steer_angle - prev_steer;
        prev_steer = Command.steer_angle;
        //var dt = Time.fixedDeltaTime;
        if (Mathf.Abs(dyaw) < 0.01)
        { SI = 0; }
            else
        { SI = (day / dyaw) / (profile.Mass * profile.wheelbase); }
    }

}

