using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System;
using System.Reflection;

public class GetfromName : MonoBehaviour
{
    private List<string> options;
    public string Namelist, profilelist;
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
        XmlSerializer serializer = new XmlSerializer(typeof(List<string>));
        FileStream stream = new FileStream(Application.dataPath + "/databases/" + Namelist + ".xml", FileMode.OpenOrCreate);
        options = serializer.Deserialize(stream) as List<string>;
        stream.Close();
        
    }


    public void GetFromFile()
    {
        var element = menu.options[menu.value].text;
        profile.profilename = element;
        profile.Load();// menu.captionText.text);
    }
}
