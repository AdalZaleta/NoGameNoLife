using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlantBury", menuName = "Plants/Bury", order = 12)]

public class PlantBury : PlantSO
{
    public override void Use(GameObject _target, int _multiplier)
    {
        Debug.Log("Buried " + _target.name);
    }
}
