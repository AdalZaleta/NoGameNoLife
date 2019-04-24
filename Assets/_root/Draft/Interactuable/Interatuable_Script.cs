using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interatuable_Script : MonoBehaviour {

	private void OnTriggerStay(Collider other) {
		//Debug.Log("Estoy en el trigger de: " + other.gameObject + "con la etiqueta: " + other.tag);
		if(Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.JoystickButton1))
		{
			Interaction();
		}
	}

	public void Interaction()
	{
		Debug.Log("Has interactuado: " + gameObject.name);
	}
}
