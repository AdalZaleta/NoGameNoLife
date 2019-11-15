using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlantFire", menuName = "Plants/Fire", order = 9)]

public class PlantFire : PlantSO
{
    public float dmg;
    private Transform pivot;
    private bool isDay = true;

    public override void Use(GameObject _target, int _multiplier)
    {
        if (isDay)
        {
            Debug.Log("Holding Fire plant at " + _target.transform.position);
            Debug.Log("Fire plant is dealing " + dmg * _multiplier);
        } else
        {
            Debug.Log("Can't use plant");
        }
    }
}
