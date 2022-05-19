using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

[CreateAssetMenu]
public class GeneralProfile : dataprofile
{
    //public string TEST;
    
    #region nested profiles
    //public SymulationProfile Sym;
    public ConstructionProfile construct;
    #endregion
    /*
    #region Data dealing methods
    public void Translate()
    {
        construct.SetupSym(Sym);
        Sym.DataValidate(); 
    }
    */
    public override void Load()
    {
        //var databuffer = new SymulationProfile();
        //FileName = name;
        XmlSerializer serializer = new XmlSerializer(typeof(GeneralProfile));
        FileStream stream = new FileStream(Application.dataPath + "/Vehicles/" + profilename + ".xml", FileMode.Open);
        var databuffer = serializer.Deserialize(stream) as GeneralProfile;
        stream.Close();
        this.Clone(databuffer);
        //Sym.DataValidate();
    }
    public override void Save()
    {
        //Debug.Log(Application.dataPath);
        if (construct == null)
            construct = new ConstructionProfile(); // CRY SOME MORE UNITY
        XmlSerializer serializer = new XmlSerializer(typeof(GeneralProfile));
        FileStream stream = new FileStream(Application.dataPath + "/Vehicles/" + profilename + ".xml", FileMode.Create);
        serializer.Serialize(stream, this);
        stream.Close();
    }

    public void Clone(GeneralProfile target)
    {
        //this.TEST = target.TEST;
        //Debug.Log(TEST);
        //construct = new ConstructionProfile();
        construct.Clone(target.construct);// this one needs work
        //Sym.Clone(target.Sym);//this one is okay

    }
    //#endregion
}//GENERAL SYSTEM WIDE PROFILE. IS SAVED TO PERMANENT STORAGE (xlm)
