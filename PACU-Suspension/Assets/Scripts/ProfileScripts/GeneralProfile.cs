using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

[CreateAssetMenu]
public class GeneralProfile : ScriptableObject
{
    public string FileName;

    #region nested profiles
    public SymulationProfile Sym;
    public ConstructionProfile construct;
    #endregion

    #region Data dealing methods
    public void Translate()
    {
        construct.SetupSym(Sym);
        Sym.DataValidate(); 
    }

    public void Load()
    {
        //var databuffer = new SymulationProfile();
        XmlSerializer serializer = new XmlSerializer(typeof(GeneralProfile));
        FileStream stream = new FileStream(Application.dataPath + "/databases/" + FileName + ".xml", FileMode.Open);
        var databuffer = serializer.Deserialize(stream) as GeneralProfile;
        stream.Close();
        this.Clone(databuffer);
        Sym.DataValidate();
    }
    public void Save()
    {
        //Debug.Log(Application.persistentDataPath);
        XmlSerializer serializer = new XmlSerializer(typeof(GeneralProfile));
        FileStream stream = new FileStream(Application.dataPath + "/databases/" + FileName + ".xml", FileMode.Create);
        serializer.Serialize(stream, this);
        stream.Close();
    }

    public void Clone(GeneralProfile target)
    {
        Sym.Clone(target.Sym);//this one is okay
        construct.Clone(target.construct);// this one needs work
    }
    #endregion
}//GENERAL SYSTEM WIDE PROFILE. IS SAVED TO PERMANENT STORAGE (xlm)
