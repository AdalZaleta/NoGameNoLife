using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlantHeal", menuName = "Plants/Heal", order = 14)]

public class PlantHeal : PlantSO
{
    public override void Use(GameObject _target, int _multiplier)
    {
        Debug.Log("Remove all status effects from " + _target.name);
    }
}
