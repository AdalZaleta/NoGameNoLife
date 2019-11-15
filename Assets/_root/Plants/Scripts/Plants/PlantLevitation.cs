using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlantLeviation", menuName = "Plants/Levitation", order = 7)]

public class PlantLevitation : PlantSO
{
    public float levitationTime;

    public override void Use(GameObject _target, int _multiplier)
    {
        Debug.Log("+" + levitationTime * _multiplier + " levitation time given to " + _target.name);
    }
}
