using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moo : AnimalBase
{
    protected override void SpecialAnimalUpdate()
    {
        Debug.Log("Imma cow yo");
    }
}
