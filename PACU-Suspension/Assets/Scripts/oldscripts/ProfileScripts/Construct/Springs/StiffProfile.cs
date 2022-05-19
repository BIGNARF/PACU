using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class StiffProfile : ScriptableObject
{
    #region inputs
    public CoilOverProfile fK, rK;
    //public float Kf,Kr;//spring stiffness coef, in N/m
    //public float Cf,Cr;// dampening coef, in Ns/m
    public float ratiof,ratior;// Coilover instalation ratio, Note: probably change it later for non coilover uses Note2: change it late as a calculatable thing inside the suspension geometry
    public float Karbf,Karbr; //anti-roll bar roll stiffness, in Nm/rad
    #endregion

    public float FKphi, RKphi, TKphi; // roll stiffnesses

    public void SetTyreandAxels(tyreprofile tyre, SymulationProfile target)
    {
        var RRF = SetRiderate(fK.stiffness, tyre.stiffness, ratiof);
        var RRR = SetRiderate(rK.stiffness, tyre.stiffness, ratior);
        FKphi = 0.5f * RRF / (target.front.t * target.front.t) + Karbf;
        RKphi = 0.5f * RRR / (target.rear.t * target.rear.t) + Karbr;
        TKphi = FKphi + RKphi;
        target.front.Kphi_total = TKphi;
        target.rear.Kphi_total = TKphi;
        target.front.Kphi_axel = FKphi;
        target.rear.Kphi_axel = RKphi;
        target.tyre.Clone(tyre);
    }

    private float SetRiderate(float K, float tK, float ratio)
    {
        var WCR = K * ratio;
        var RR = (WCR + tK) / (WCR * tK);
        return RR;
    }// calcs riderate

    public void Clone(StiffProfile target)
    {
        this.fK = target.fK;
        this.rK = target.rK;
        this.ratiof = target.ratiof;
        this.ratior = target.ratior;
        this.Karbf = target.Karbf;
        this.Karbr = target.Karbr;
    }
}
