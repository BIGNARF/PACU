using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class carprofile : ScriptableObject // Contains general mass and size data for the car. Also contains methods for applying total acceleration and yaw moment
{
    public float Mass;// in kg
    public float Rot_Inertia;// in kg.m2
    public float wheelbase;// in mm
    public float CGX;// from front axel, mm
    public bool Steertag; // false: front wheel drive, true: four wheel drive
    public float bar_ratio;//steering bar ratio in mm/deg

    public void Clone(carprofile Target)
    {
        #region MAMA MIA ITS-A THE SPAGHETTI!
        this.Mass= Target.Mass;
        this.Rot_Inertia= Target.Rot_Inertia;
        this.wheelbase= Target.wheelbase;
        this.CGX= Target.CGX;
        this.Steertag= Target.Steertag;
        this.bar_ratio= Target.bar_ratio;
        #endregion
    }//clones Target into current profile.
}
