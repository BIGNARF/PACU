using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class axelprofile : ScriptableObject // Contains MF coefs and methods for calculation lateral force
{
    #region Weight transfer stuff
    public float S_mass, US_mass; // kg
    public float S_CGH, US_CGH;  //mm
    public float t; // mm
    public float Kphi_axel, Kphi_total; // Nm/rad
    public float max_steer; // max steer bar travel, in mm;

    #endregion

    #region Geometry values
    public float KPI, Caster, S_Camber, S_toe, SArm_angle;//all angles in deg, toe in = positive (KPI=DONE, S_Camber = DONE, S_toe =DONE, S_arm_angle= DONE)
    public float FVSA, SteerArm, CRH; //all distances in mm (ALL DONE)
    //public float bar_ratio;//steering bar ratio in mm/deg
    #endregion


    #region internal stuff
    private float Transfer;
    private float roll;// positive for +ay roll result, as in: CW rotation from front view
    #endregion

    public void WeightTransfer(float Ay)
    {
        Transfer = Ay * US_mass * US_CGH / t; // unsprung mass component
        Transfer += Ay * S_mass * CRH / t; // geometric component
        var Hs = Mathf.Abs(S_CGH - CRH); // moment arm
        Transfer += Ay * (Kphi_axel / Kphi_total) * (S_mass + US_mass) * Hs / t;// elastic component
        roll = Transfer * t / (Kphi_total);

    }// calculates weight transfer moment, recieves Ay(m/s2), needs to be set up before SetNormalForce or SetCamber


    public float SetNormalForce(bool left)
    {
        float NormalForce;
        var S_Weight = S_mass * 9.8066;
        var US_Weight = US_mass * 9.8066;
        var Static_Weight = (US_Weight + S_Weight) / 2;


        if (left)
        {
            NormalForce = (float)Static_Weight + Transfer;
        }
        else
        {
            NormalForce = (float)Static_Weight - Transfer;
        }
        return NormalForce;
    }// Returns: Normal force (N), Recieves: lateral acceleration (m/s2) , left tag

    public float SetCamber(float wsteer, bool left)
    {
        //convertions
        var wsteer_r = wsteer * Mathf.Deg2Rad;
        var KPI_r = Mathf.Deg2Rad * KPI;
        var Caster_r = Mathf.Deg2Rad * Caster;
        float dh = 0;
        if (left)
        {
            dh = Mathf.Deg2Rad * roll * t / 2;// upward displacement of wheel with given roll angle
        }
        else
        {
            dh = -Mathf.Deg2Rad * roll * t / 2;// upward displacement of wheel with given roll angle
        }
        var S_camber_r = Mathf.Deg2Rad * S_Camber;
        var dcamber_kpi = KPI_r + Mathf.Acos(Mathf.Cos(wsteer_r) * Mathf.Sin(KPI_r)) - Mathf.Deg2Rad * 90; //KPI CAMBER COMPONENT
        var dcamber_ctr = Mathf.Acos(Mathf.Sin(wsteer_r) * Mathf.Sin(Caster_r)) - Mathf.Deg2Rad * 90;//caster camber component
        var dcamber_roll = Mathf.Atan(1 / FVSA) * dh;//rolling camber component
        var camber = Mathf.Rad2Deg * (S_camber_r + dcamber_kpi + dcamber_ctr + dcamber_roll); //sum of all camber factors
        return camber;
    }//Receives: wheel steer angle (deg) and roll angle(deg), returns: camber(deg) 

    public float Steering(float dl, bool left)
    {
        //note: maybe change this and block the steering input at the controller lever, idk it works for now.
        if (Mathf.Abs(dl) > max_steer)
            dl = Mathf.Sign(dl) * max_steer;
        // note: does NOT include effects of roll/bump steering. may be added in the future (probably not)
        float theta = 0;
        float theta_r;
        var SArm_angle_r = Mathf.Deg2Rad * SArm_angle;
        var l1 = ((Mathf.Sin(SArm_angle_r) * SteerArm) - dl) / SteerArm;

        #region temporary fix for calculating the steering angle. A better mathematical study of this shit may be warrenty
        if (l1 > 1)
        {
            l1 = l1 - 1;
            theta_r = SArm_angle_r - (Mathf.Deg2Rad * 90 + Mathf.Asin(l1));
        }
        else
        {
            theta_r = SArm_angle_r - Mathf.Asin(l1);
        }
        #endregion

        if (left)
        {
            theta = Mathf.Rad2Deg * theta_r - S_toe;
        }
        else
        {
            theta = Mathf.Rad2Deg * theta_r + S_toe;
        }
        return theta;
    }//Returns: tyre steer angle (deg), recieves: steer angle(deg, positive toward righthand turn) and tag left

    public void Clone(axelprofile Target)
    {
        #region MAMA MIA ITS-A THE SPAGHETTI!
        this.S_mass = Target.S_mass;
        this.US_mass = Target.US_mass;
        this.S_CGH = Target.S_CGH;
        this.US_CGH = Target.US_CGH;
        this.CRH = Target.CRH;
        this.t = Target.t;
        this.Kphi_axel = Target.Kphi_axel;
        this.Kphi_total = Target.Kphi_total;
        this.max_steer = Target.max_steer;
        this.KPI = Target.KPI;
        this.Caster = Target.Caster;
        this.S_Camber = Target.S_Camber;
        this.S_toe = Target.S_toe;
        this.SArm_angle=Target.SArm_angle;
        this.FVSA = Target.FVSA;
        this.SteerArm=Target.SteerArm;
        #endregion
    }//clones Target into current profile.

}

