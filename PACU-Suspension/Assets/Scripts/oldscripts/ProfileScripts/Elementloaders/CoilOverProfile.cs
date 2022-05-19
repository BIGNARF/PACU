using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

[CreateAssetMenu]
public class CoilOverProfile : dataprofile
{
    public float stiffness; // N/m
    public float Maxlenght, Minlenght; //mm
    public float damp;// damper coef in Ns/m

    public void Clone(CoilOverProfile Target)
    {
        #region MAMA MIA ITS-A THE SPAGHETTI!
        // finish this sometime
        this.profilename = Target.profilename;
        this.stiffness = Target.stiffness;
        this.Maxlenght = Target.Maxlenght;
        this.Minlenght = Target.Minlenght;
        this.damp = Target.damp;
        #endregion
    }//clones Target into current profile.


    public override void Save()
    {
        Saveprofile();
        Savename();
    }
    private void Saveprofile()
    {
        string FileName = "coiloverdatabase";
        var databuffer = new List<CoilOverProfile>();
        XmlSerializer serializer = new XmlSerializer(typeof(List<CoilOverProfile>));
        FileStream stream = new FileStream(Application.dataPath + "/databases/" + FileName + ".xml", FileMode.OpenOrCreate);
        databuffer = serializer.Deserialize(stream) as List<CoilOverProfile>;
        databuffer.Add(this);
        stream.Close();
        stream = new FileStream(Application.dataPath + "/databases/" + FileName + ".xml", FileMode.Create);
        serializer.Serialize(stream, databuffer);
        stream.Close();
    }//saves current profile to tyrelist xml file

    private void Savename()
    {
        string FileName = "coiloverlist";
        var databuffer = new List<string>();
        XmlSerializer serializer = new XmlSerializer(typeof(List<string>));
        FileStream stream = new FileStream(Application.dataPath + "/databases/" + FileName + ".xml", FileMode.OpenOrCreate);
        databuffer = serializer.Deserialize(stream) as List<string>;
        databuffer.Add(this.profilename);
        stream.Close();
        stream = new FileStream(Application.dataPath + "/databases/" + FileName + ".xml", FileMode.Create);
        serializer.Serialize(stream, databuffer);
        stream.Close();
    }//saves tyrename to tyre namelist for research later

    public override void Load()//string name)
    {
        string FileName = "coiloverdatabase";
        var databuffer = new List<CoilOverProfile>();
        XmlSerializer serializer = new XmlSerializer(typeof(List<CoilOverProfile>));
        FileStream stream = new FileStream(Application.dataPath + "/databases/" + FileName + ".xml", FileMode.Open);
        databuffer = serializer.Deserialize(stream) as List<CoilOverProfile>;
        stream.Close();
        var result = databuffer.Find(x => x.profilename.Equals(profilename));
        this.Clone(result);
    }
}
