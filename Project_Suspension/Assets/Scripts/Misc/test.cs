using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    //public SuspProfile lol;
    public SymulationProfile lol;
    private void Start()
    {
       // lol.Save();
        lol.Load();
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.Return))
        {

        }
    }
}
