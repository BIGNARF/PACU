using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeAxel : MonoBehaviour
{

    // DEPRECATED
    /*public SuspProfile front, rear,current;

    public VectorInput fp1, ap1, op1, fp2, ap2, op2, ip3, op3, UC, WC, CP;


    public InputField KPI;
    public InputField Camber;
    public InputField Caster;
    public InputField Track;

    public bool Isfront;
    public void Change(bool axel)
    {
        Isfront = axel;
        if(Isfront)
        { 
            current = front;
        }
        else
        {
            current = rear;
        }
        fp1.ChangeTarget(current);
        ap1.ChangeTarget(current);
        op1.ChangeTarget(current);
        fp2.ChangeTarget(current);
        ap2.ChangeTarget(current);
        op2.ChangeTarget(current);
        ip3.ChangeTarget(current);
        op3.ChangeTarget(current);
        UC.ChangeTarget(current);
        WC.ChangeTarget(current);
        CP.ChangeTarget(current);
    }

    public void SetValues()
    {
        KPISetGeneral();
        CamberSetGeneral();
        CasterSetGeneral();
        TrackSetGeneral();
        RefreshDisplay();
    }//SETS THE VALUES OF ALL ANGLED INPUTS

    private void KPISetGeneral()
    {
        float aux = float.Parse(KPI.text);
        current.SetByKPI(aux);
    }

    private void CamberSetGeneral()
    {
        float aux = float.Parse(Camber.text);
        current.SetByCamber(aux);
    }

    private void CasterSetGeneral()
    {
        float aux = float.Parse(Caster.text);
        current.SetByCaster(aux);
    }

    private void TrackSetGeneral()
    {
        float aux = float.Parse(Track.text);
        current.SetByTrack(aux);
    }

    private void RefreshDisplay()
    {
        fp1.VectorDisplay();
        ap1.VectorDisplay();
        op1.VectorDisplay();
        fp2.VectorDisplay();
        ap2.VectorDisplay();
        op2.VectorDisplay();
        ip3.VectorDisplay();
        op3.VectorDisplay();
        UC.VectorDisplay();
        WC.VectorDisplay();
        CP.VectorDisplay();
    }
    */
}
