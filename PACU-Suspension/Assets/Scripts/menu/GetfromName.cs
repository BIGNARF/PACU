using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class GetfromName : MonoBehaviour
{
    private List<string> options;
    public string FileName;
    public dataprofile profile;
    private Dropdown menu;
    void Start()
    {
        menu = gameObject.GetComponent<Dropdown>();
        GetList();
        menu.ClearOptions();
        menu.AddOptions(options);
    }

    private void GetList()
    {
        //string FileName = "tyrelist";
        XmlSerializer serializer = new XmlSerializer(typeof(List<string>));
        FileStream stream = new FileStream(Application.persistentDataPath + "/" + FileName + ".xml", FileMode.OpenOrCreate);
        options = serializer.Deserialize(stream) as List<string>;
        stream.Close();
    }


    public void GetFromFile()
    {
        //Debug.Log(menu.captionText.text);
        profile.Load(menu.captionText.text);
    }
}
