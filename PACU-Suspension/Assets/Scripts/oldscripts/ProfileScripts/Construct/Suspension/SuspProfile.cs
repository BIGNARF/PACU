using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathNet.Numerics.LinearAlgebra;

[CreateAssetMenu]
public class SuspProfile : ScriptableObject
{
    #region components
    public Barlink SteeringBar;
    public A_arm Upper, Lower;
    public WheelGeometry wheel;
    #endregion

    #region values
    public Vector2 RC;
    private Vector2 IC;

    public float S_toe, Sarm_Lenght, Sarm_Angle;
    #endregion

    #region GET geometry functions
    private void GetSArmValues()
    {
        Vector2 WC = new Vector2(wheel.WheelCenter.x, wheel.WheelCenter.z);
        Vector2 Sarm = new Vector2(SteeringBar.endpoint.x, SteeringBar.endpoint.z); // converts to 2d vector in the top down plane
        Vector2 Spindle = new Vector2(wheel.UprightCenter.x, wheel.UprightCenter.z);
        var ARMline = (Sarm - Spindle);
        var SpindleLine = (WC - Spindle);
        Sarm_Lenght = ARMline.magnitude;
        Sarm_Angle = Vector2.SignedAngle(ARMline, SpindleLine) +90;
        S_toe = Vector2.SignedAngle( Vector2.right, SpindleLine);
    }// calculates all topdown plane angles. Its kinda fucked 'cause angle sign convention was done by trial and error. may be fixed someday
    
    private void FindRC()
    {
        var upline = Upper.Extendline();
        var lowline = Lower.Extendline();
        IC = Intersect(upline, (Vector2)Upper.startpoint, lowline, (Vector2)Lower.startpoint);
        wheel.GroundWheel();
        var otherwheel = wheel.Otherwheel();
        var ICLine = IC - otherwheel;
        RC = Intersect(ICLine, IC, Vector2.up, Vector2.zero);
    }
    private Vector2 Intersect(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2)
    {
        Matrix<float> mat = Matrix<float>.Build.Dense(2, 2);
        Vector<float> vec = Vector<float>.Build.Dense(2);
        mat[0, 0] = a1.x;
        mat[1, 0] = a1.y;
        mat[0, 1] = -b1.x;
        mat[1, 1] = -b1.y;
        vec[0] = b2.x - a2.x;
        vec[1] = b2.y - a2.y;
        Vector<float> res = mat.Solve(vec);
        Vector2 Response = res[0] * a1 + a2;
        return Response;
    }// Returns: Vector x,y of intersection point of two lines, Recieves: Line direction vectors a1 and b1, and line origin vectors a2 and b2
    #endregion

    #region SET functions and setable variables

    public float KPI;// KPI positive in CCW front front view
    public void SetKPI(float angle)//recieves KPI in degrees
    {
        var angle_r = Mathf.Deg2Rad * angle;
        var a = Upper.outerpoint.y - Lower.outerpoint.y;
        Upper.outerpoint.x = Lower.outerpoint.x + a * Mathf.Sin(angle_r);
        GetKPI();
    }

    private void GetKPI()//calculates KPI and loads the value to KPI
    {
        // calcs in the frontal plane
        var kingpin = (Vector2)(-Lower.outerpoint + Upper.outerpoint);
        KPI = Vector2.SignedAngle(kingpin, Vector2.up)-StaticCamber;
    }

    public float caster;
    public void SetCaster(float angle)//recieves Caster in degrees
    {
        var angle_r = Mathf.Deg2Rad * angle;
        var a = Upper.outerpoint.y - Lower.outerpoint.y;
        Upper.outerpoint.z = Lower.outerpoint.z + a * Mathf.Sin(angle_r);
        GetCaster();
    }

    private void GetCaster()//calculates Caster and loads the value to caster
    {
        //var kingpin = (Vector2)(-Lower.outerpoint + Upper.outerpoint);
        // calcs in the side plane
        var kingpin = new Vector2(-Lower.outerpoint.z + Upper.outerpoint.z, -Lower.outerpoint.y + Upper.outerpoint.y);
        caster = Vector2.SignedAngle(kingpin, Vector2.up);
    }

    public float StaticCamber;
    public void SetStaticCamber(float angle)
    {
        var angle_r = Mathf.Deg2Rad * angle;
        var a = (wheel.Contactpatch - wheel.WheelCenter).magnitude;
        wheel.WheelCenter.x = wheel.Contactpatch.x + a * Mathf.Sin(angle_r);
        wheel.WheelCenter.y = wheel.Contactpatch.y + a * Mathf.Cos(angle_r);
        GetStaticCamber();
    }

    private void GetStaticCamber()//calculates Caster and loads the value to caster
    {
        var tyre = (Vector2)(-wheel.Contactpatch+wheel.WheelCenter);
        StaticCamber = Vector2.SignedAngle(tyre, Vector2.up);
    }

    public float Track;
    public void SetTrack(float T)// recieves full track in mm, moves the entire wheel side assembly
    {
        T = T / 2;
        var a = T - wheel.Contactpatch.y;
        wheel.Contactpatch.y += a;
        wheel.UprightCenter.y += a;
        wheel.WheelCenter.y += a;
        Upper.outerpoint.y += a;
        Lower.outerpoint.y += a;
        GetTrack();
    }
    public void GetTrack()
    {
        Track = wheel.Contactpatch.y * 2;
    }

    public float FVSA;//in mm
    public void SetFVSA(float value)//TEST LATTER
    {
        var deltax = value - FVSA;
        var upline = Upper.Extendline();
        var deltay1=0.5*deltax*(upline.y/upline.x);
        Upper.forepoint.y += (float)deltay1;
        Upper.aftpoint.y += (float)deltay1;
        Upper.outerpoint.y += (float)deltay1;
        var lowline = Lower.Extendline();
        var deltay2 = 0.5 * deltax * (lowline.y / lowline.x);
        Lower.forepoint.y += (float)deltay2;
        Lower.aftpoint.y += (float)deltay2;
        Lower.outerpoint.y += (float)deltay2;
        GetFVSA();
    }
    private void GetFVSA()
    {
        var upline = Upper.Extendline();
        var lowline = Lower.Extendline();
        IC = Intersect(upline, (Vector2)Upper.startpoint, lowline, (Vector2)Lower.startpoint);
        FVSA = (IC.x - wheel.Contactpatch.x);
    }
    #endregion
    /* EDIT LATER
    public float RCH;// Roll center height, in mm
    public void SetRCH(float value)
    {
        //IDK
        GetRCH();
    }
    private void GetRCH()
    {
        var upline = Upper.Extendline();
        var lowline = Lower.Extendline();
        IC = Intersect(upline, (Vector2)Upper.startpoint, lowline, (Vector2)Lower.startpoint);
        wheel.GroundWheel();
        var otherwheel = wheel.Otherwheel();
        var ICLine = IC - otherwheel;
        RC = Intersect(ICLine, IC, Vector2.up, Vector2.zero);
    }*/

    public void Clone(SuspProfile Target)// clones the target to the current profile
    {
        this.Upper = Target.Upper;
        this.Lower = Target.Lower;
        this.wheel = Target.wheel;
        this.SteeringBar = Target.SteeringBar;
    }
}

#region inner components classes

// COORDIANTE CONVENTIONS => X=LATERAL, Y=HEIGH, Z=LONGITUDINAL
[System.Serializable]
public class Barlink
{
    public Vector3 startpoint, endpoint;

    public virtual Vector2 Extendline()
    {
        var linevector = (Vector2)(endpoint - startpoint);
        return linevector;
    }
} //generic class for extending the lines of components
[System.Serializable]
public class A_arm : Barlink
{
    public Vector3 forepoint, aftpoint, outerpoint;

    public override Vector2 Extendline()
    {
        startpoint = (forepoint + aftpoint) / 2;
        endpoint = outerpoint;
        var linevector = (Vector2)(endpoint - startpoint);
        linevector.Normalize();
        return linevector;
    }
}
[System.Serializable]
public class WheelGeometry
{
    public Vector3 Contactpatch, WheelCenter, UprightCenter;

    public void GroundWheel()
    {
        Contactpatch.y = 0;
    }// sets the contact pactch to y=0, as in, touching the ground exactly.
    public Vector2 Otherwheel()
    {
        var Contact = (Vector2)Contactpatch;
        Contact.x = -Contact.x;
        return Contact;
    }// return the 2d coordinates of the other hand contact patch

    public float GetScamber()
    {
        var WheelLine = (Vector2)(WheelCenter - Contactpatch);
        var camber = Vector2.SignedAngle(WheelLine, Vector2.up);
        return camber;
    }
}
#endregion     
