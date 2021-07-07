using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region inner classes
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
        var camber = Vector2.SignedAngle(WheelLine, Vector2.up);
        return camber;
    }
}
#endregion
