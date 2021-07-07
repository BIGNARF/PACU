using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;


[CreateAssetMenu]
public class SymulationProfile : ScriptableObject
{
    #region file condition stuff
    public string FileName;//name of the file to open
    public bool valid; //sets if the profile has been validated for simulation
    #endregion

    #region internal logic stuff

    //private SymulationProfile loadedprofile;
    [SerializeField]
    public carprofile car;
    [SerializeField]
    public axelprofile front, rear;
    [SerializeField]
    public tyreprofile tyre;
    #endregion

   // #region XML READ/SAVE STUFF
    public void Save()
    {
        Debug.Log(Application.persistentDataPath);
        XmlSerializer serializer = new XmlSerializer(typeof(SymulationProfile));
        FileStream stream = new FileStream(Application.persistentDataPath + "/" + FileName + ".xml", FileMode.Create);
        serializer.Serialize(stream, this);
        stream.Close();
    }// saves date from unity objects into xml file
        public void Load()
        {
            var databuffer = new SymulationProfile();
            XmlSerializer serializer = new XmlSerializer(typeof(SymulationProfile));
            FileStream stream = new FileStream(Application.persistentDataPath + "/" + FileName + ".xml", FileMode.Open);
            databuffer = serializer.Deserialize(stream) as SymulationProfile;
            stream.Close();
            DataValidate();
        }//loads data from xml file into unity objects

        public void Clone(SymulationProfile loadedprofile)
        {
            car.Clone(loadedprofile.car);
            front.Clone(loadedprofile.front);
            rear.Clone(loadedprofile.rear);
            tyre.Clone(loadedprofile.tyre);
        }// copies the data loaded on the buffer into the individual component profiles

      //  #endregion

        public void DataValidate()
        {
            if (this.car.Mass < 1f)// checks that mass is positive/ non zero
            this.valid = false;
            if (this.car.Rot_Inertia < 1f)// checks that inertia is positive/nonzero
            this.valid = false;
            if (this.car.wheelbase < this.car.CGX)// checks if CG is between the two axels
            this.valid = false;
            if (Mathf.Abs(this.front.S_CGH - this.rear.S_CGH) > 1f)// checks if CG of suspended mass is the same on both axels
            this.valid = false;
            if (Mathf.Abs(this.front.Kphi_total - this.rear.Kphi_total) > 10f)// checks if both total Kphi are equal
            this.valid = false;
            if (Mathf.Abs(this.front.Kphi_axel + this.rear.Kphi_axel - this.rear.Kphi_total) > 10f)// checks if the Kphis add up
            this.valid = false;
            var fmass = this.front.US_mass + this.front.S_mass;
            var rmass = this.rear.US_mass + this.rear.S_mass;
            if (Mathf.Abs(fmass + rmass - this.car.Mass) > 10f)// checks if both axel masses add up
            this.valid = false;
            var aux = fmass * this.car.CGX - rmass * (this.car.wheelbase - this.car.CGX);
            if (Mathf.Abs(aux) > 10f)// checks compatibility between mass partition and CG position
            this.valid = false;
        this.valid = true;
        }//Checks consistency of data in the loaded profile */
}