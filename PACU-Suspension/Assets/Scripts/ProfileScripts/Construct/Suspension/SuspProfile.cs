using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathNet.Numerics.LinearAlgebra;

[CreateAssetMenu]
public class SuspProfile : ScriptableObject
{

    // COORDIANTE CONVENTIONS => X=LATERAL, Y=HEIGH, Z=LONGITUDINAL
  /*  #region inner classes
    [System.Serializable]
    public class Barlink
    {
        public Vector3 outerpoint, innerpoint;

        public Vector2 Extendline()
        {
            var linevector = (Vector2)(outerpoint - innerpoint);
            return linevector;
        }      
    }
    [System.Serializable]
    public class A_arm : Barlink
    {
        public Vector3 forepoint, aftpoint;

        public Vector2 Extendline()
        {
            innerpoint = (forepoint + aftpoint) / 2;
            var linevector = (Vector2)(outerpoint - innerpoint);
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
            var camber = Vector2.SignedAngle(WheelLine,Vector2.up);
            return camber;
        }
    }
    #endregion*/

    #region inputs
    // public axelprofile results;

    public Barlink SteeringBar;
    public A_arm Upper, Lower;
    public WheelGeometry wheel;
    #endregion

    #region outputs
    public Vector2 RC;
    private Vector2 IC;
    float S_camber,KPI, caster;// KPI positive in CCW front front view
    float S_toe, Sarm_Lenght, Sarm_Angle;
    #endregion

    #region inner geometry functions
    private void GetSArmValues()
    {
        Vector2 WC = new Vector2(wheel.WheelCenter.x, wheel.WheelCenter.z);
        Vector2 Sarm = new Vector2(SteeringBar.outerpoint.x, SteeringBar.outerpoint.z); // converts to 2d vector in the top down plane
        Vector2 Spindle = new Vector2(wheel.UprightCenter.x, wheel.UprightCenter.z);
        var ARMline = (Sarm - Spindle);
        var SpindleLine = (WC - Spindle);
        Sarm_Lenght = ARMline.magnitude;
        Sarm_Angle = Vector2.SignedAngle(ARMline, SpindleLine) +90;
        S_toe = Vector2.SignedAngle( Vector2.right, SpindleLine);
    }// calculates all topdown plane angles. Its kinda fucked 'cause angle sign convention was done by trial and error. may be fixed someday
    private void GetKPIandCaster()
    {
        // calcs in the frontal plane
        var kingpin = (Vector2)(-Lower.outerpoint + Upper.outerpoint);
        KPI = Vector2.SignedAngle(kingpin,Vector2.up) -S_camber;
        // calcs in the side plane
        kingpin = new Vector2(-Lower.outerpoint.z + Upper.outerpoint.z, -Lower.outerpoint.y + Upper.outerpoint.y);
        caster = Vector2.SignedAngle(kingpin, Vector2.up);
    }
    private void FindRC()
    {
        var upline = Upper.Extendline();
        var lowline = Lower.Extendline();
        IC = Intersect(upline, (Vector2)Upper.innerpoint, lowline, (Vector2)Lower.innerpoint);
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

    #region SET AXEL PROFILE RESULTS
    public void SetProfile(axelprofile results)
    {
        FindRC();
        S_camber= wheel.GetScamber();
        GetKPIandCaster();
        GetSArmValues();

        results.SArm_angle = Sarm_Angle;
        results.SteerArm = Sarm_Lenght;
        results.S_toe = S_toe;

        results.KPI = KPI;
        results.Caster = caster;
        results.S_Camber = S_camber;
        results.FVSA = IC.x - wheel.WheelCenter.x;
        results.CRH = RC.y;
        results.t = 2 * wheel.Contactpatch.x;
    }// set output values of the results container
    #endregion

    public void Clone(SuspProfile Target)
    {
        this.Upper = Target.Upper;
        this.Lower = Target.Lower;
        this.wheel = Target.wheel;
        this.SteeringBar = Target.SteeringBar;
    }

    #region ANGLE SET FUNCTIONS
    public void SetByKPI(float KPI)
    {
        var KPI_r = Mathf.Deg2Rad * KPI;
        var a = Upper.outerpoint.y - Lower.outerpoint.y;
        Upper.outerpoint.x = Lower.outerpoint.x + a * Mathf.Sin(KPI_r);
    }//recieves KPI in degrees
    public void SetByCamber(float Camber)
    {
        var Camber_r = Mathf.Deg2Rad * Camber;
        var a = (wheel.Contactpatch-wheel.WheelCenter).magnitude;
        wheel.WheelCenter.x = wheel.Contactpatch.x + a * Mathf.Sin(Camber_r);
        wheel.WheelCenter.y = wheel.Contactpatch.y + a * Mathf.Cos(Camber_r);
    }

    public void SetByCaster(float Caster)
    {
        var Caster_r = Mathf.Deg2Rad * Caster;
        var a = Upper.outerpoint.y - Lower.outerpoint.y;
        Upper.outerpoint.z = Lower.outerpoint.z + a * Mathf.Sin(Caster_r);
    }
    public void SetByRC()
    {

    }

    public void SetByTrack(float T)
    {
        T = T / 2;
        var a = T - wheel.Contactpatch.y;
        wheel.Contactpatch.y += a;
        wheel.UprightCenter.y += a;
        wheel.UprightCenter.y += a;
        Upper.outerpoint.y += a;
        Lower.outerpoint.y += a;
    }
    #endregion //NOTE: ONE OF THESE IS FUCKED, I dont know which tho
}
