using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlantCure", menuName = "Plants/Teleport", order = 5)]

public class PlantTeleport : PlantSO
{
    private Vector3 pos;

    public override void Use(GameObject _target, int _multiplier)
    {
        Debug.Log("Yeeting Plant ...");
        Debug.Log("Teleported " + _target.name + " to " + pos);
    }
}
