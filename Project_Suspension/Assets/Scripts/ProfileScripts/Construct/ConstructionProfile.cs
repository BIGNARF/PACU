using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;


[CreateAssetMenu]
public class ConstructionProfile : ScriptableObject
{
    public SuspProfile front, rear;
    public StiffProfile springs;
    public tyreprofile tyre;//maybe remove this tbh
    //public SymulationProfile Sym;

    public void SetupSym(SymulationProfile Sym)
    {
        front.SetProfile(Sym.front);
        rear.SetProfile(Sym.rear);
        springs.SetTyreandAxels(tyre, Sym);
    }//sets up symulation useful date from constructive data

    public void Clone(ConstructionProfile target)
    {
        front.Clone(target.front);
        rear.Clone(target.rear);
        springs.Clone(target.springs);
        tyre.Clone(target.tyre);
        // continue for other profiles
    }
}
