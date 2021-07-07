using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dataprofile : ScriptableObject
{
    public string profilename;


    public virtual void Save()
    {
    }//Saves profile to persistant data file
    public virtual void Load(string name)
    {
    }//loads object from persistant data file
}
