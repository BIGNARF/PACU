using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;


//[CreateAssetMenu]
[System.Serializable]
public class ConstructionProfile : ScriptableObject
{
    //public SuspProfile front, rear;
    //public StiffProfile springs;
    //public SymulationProfile Sym;
    public tyreprofile tyre;//maybe remove this tbh
    public CoilOverProfile front, rear;


    public void SetupSym(SymulationProfile Sym)
    {
        //front.SetProfile(Sym.front);
        //rear.SetProfile(Sym.rear);
        //springs.SetTyreandAxels(tyre, Sym);
    }//sets up symulation useful date from constructive data

    public void Clone(ConstructionProfile target)
    {
        //front.Clone(target.front);
        //rear.Clone(target.rear);
        //springs.Clone(target.springs);
        tyre.Clone(target.tyre);
        front.Clone(target.front);
        rear.Clone(target.rear);
        // continue for other profiles
    }


}
