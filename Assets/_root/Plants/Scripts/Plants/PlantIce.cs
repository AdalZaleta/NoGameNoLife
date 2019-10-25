using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlantCure", menuName = "Plants/Ice", order = 10)]

public class PlantIce : PlantSO
{
    public float dmg;
    private Transform pivot;
    private bool isNight = true;

    public override void Use(GameObject _target, int _multiplier)
    {
        if (isNight)
        {
            Debug.Log("Holding Ice plant at " + _target.transform.position);
            Debug.Log("Ice plant is dealing " + dmg * _multiplier);
        }
        else
        {
            Debug.Log("Can't use plant");
        }
    }
}
