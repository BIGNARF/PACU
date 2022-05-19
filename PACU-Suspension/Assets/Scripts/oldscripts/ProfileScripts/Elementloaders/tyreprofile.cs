using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

//[CreateAssetMenu]
[System.Serializable]
public class tyreprofile : dataprofile // Contains MF coefs and methods for calculation lateral force
{
    public float stiffness; // N/m

    #region TTC base date
    public float FNOM = 2378.134018F;
    public float PCY1 = 0.9539170F;
    public float PDY1 = -1.1368650F;
    public float PDY2 = 0.4689010f;
    public float PDY3 = -5.2737330f;
    public float PEY1 = -0.2999010f;
    public float PEY2 = 0.8782960f;
    public float PEY3 = -0.2283970f;
    public float PEY4 = -7.3951333f;
    public float PKY1 = -11.8621944f;
    public float PKY2 = 1.2972245f;
    public float PKY3 = 0.1856351f;

    public float PHY1 = 0.0045202f;
    public float PHY2 = -0.0007918f;
    public float PHY3 = 0.0438000f;
    public float PVY1 = 0.1338810f;
    public float PVY2 = -0.1655041f;
    public float PVY3 = -0.0526262f;
    public float PVY4 = -0.8147694f;
    #endregion
    public float LatForce(float slip, float camber, float Fz) //note: MF may or may not be fucked. Idk sometimes work.
    {
        float y;
        float B, C, D, E;//eq coefs
        float slip_1, SHY, SVY;
        if (Fz < 0.1f || Mathf.Abs(slip) < 0.05f)
        {
            y = 0f;
        }
        else
        {
            Coefs();//calc the coefs for the MF
            y = D * Mathf.Sin(C * Mathf.Atan(B * slip_1 - E * (B * slip_1 - Mathf.Atan(B * slip_1)))) + SVY; //maybe change again
        }
        return y;
        void Coefs()
        {
            var FNOM_1 = FNOM * 1;
            var dfz = (Fz - FNOM_1) / FNOM_1;
            var camber_1 = camber * 1f;

            SHY = (PHY1 + PHY2 * dfz) * 1 + PHY3 * camber_1 * 1 + (1 - 1);
            SVY = Fz * ((PVY1 + PVY2 * dfz) * 1 + (PVY3 + PVY4 * dfz) * 1) * 1 * 1;
            slip_1 = slip + SHY; // slip angle adjustment

            var MUy = (PDY1 + PDY2 * dfz) * (1f - PDY3 * camber_1 * camber_1) * 1;
            var Ky0 = PKY1 * FNOM * Mathf.Sin(2 * Mathf.Atan(Fz / (PKY2 * FNOM * 1))) * 1 * 1;//check for deg/rad shit
            var Ky = Ky0 * (1 - PKY3 * Mathf.Abs(camber_1)) * 1;

            C = PCY1 * 1;
            D = Fz * MUy;
            E = (PEY1 + PEY2 * dfz) * (1 - (PEY3 + PEY4 * camber_1) * Mathf.Sign(slip_1)) * camber_1;
            B = Ky / (C * D);
        }//calc the coefs for the MF, all the *1 multiplications are placements of experimental fitting coefs, I may implement someday idk
    }// return: lateral force(N). Enter: Fz(N) camber/slip(rad) 


    public void Clone(tyreprofile Target)
    {
        #region MAMA MIA ITS-A THE SPAGHETTI!
        // finish this sometime
        Debug.Log(Target.profilename);
        this.profilename = Target.profilename;
        this.stiffness = Target.stiffness;
        this.FNOM = Target.FNOM;
        this.PCY1 = Target.PCY1;
        this.PDY1 = Target.PDY1;
        this.PDY2 = Target.PDY2;
        this.PDY3 = Target.PDY3;
        this.PEY1 = Target.PEY1;
        this.PEY2 = Target.PEY2;
        this.PEY3 = Target.PEY3;
        this.PEY4 = Target.PEY4;
        this.PKY1 = Target.PKY1;
        this.PKY2 = Target.PKY2;
        this.PKY3 = Target.PKY3;
        this.PHY1 = Target.PHY1;
        this.PHY2 = Target.PHY2;
        this.PHY3 = Target.PHY3;
        this.PVY1 = Target.PVY1;
        this.PVY2 = Target.PVY2;
        this.PVY3 = Target.PVY3;
        this.PVY4 = Target.PVY4;
        #endregion
    }//clones Target into current profile.


    public override void Save()
    {
        Saveprofile();
        Savename();
    }
    private void Saveprofile()
    {
        //Debug.Log(Application.persistentDataPath);
        string FileName="tyredatabase";
        var databuffer = new List<tyreprofile>();
        XmlSerializer serializer = new XmlSerializer(typeof(List<tyreprofile>));
        FileStream stream = new FileStream(Application.dataPath + "/databases/" + FileName + ".xml", FileMode.OpenOrCreate);
        databuffer = serializer.Deserialize(stream) as List<tyreprofile>;
        databuffer.Add(this);
        stream.Close();
        stream = new FileStream(Application.dataPath + "/databases/" + FileName + ".xml", FileMode.Create);
        serializer.Serialize(stream, databuffer);
        stream.Close();
    }//saves current profile to tyrelist xml file

    private void Savename()
    {
       //Debug.Log(Application.persistentDataPath);
        string FileName = "tyrelist";
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
        string FileName = "tyredatabase";
        var databuffer = new List<tyreprofile>();
        XmlSerializer serializer = new XmlSerializer(typeof(List<tyreprofile>));
        FileStream stream = new FileStream(Application.dataPath + "/databases/" + FileName + ".xml", FileMode.Open);
        databuffer = serializer.Deserialize(stream) as List<tyreprofile>;
        stream.Close();
        var result =databuffer.Find(x => x.profilename.Equals(this.profilename));
        this.Clone(result);
    }//loads data from tyre list xml file based on tyre name 
}
