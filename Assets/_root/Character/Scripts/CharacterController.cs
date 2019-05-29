using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace webs
{
	public class CharacterController : MonoBehaviour {

		public GameObject Personaje;
		public Camera camara;
		public float speedMove;
		Rigidbody rg;

		public Vector3[] positionsCamera;
		int actualPosCamera = 0; 

		void Awake() {
			Manager_Static.controllerCharacter = this;
		}

		void Start()
		{
			rg = Personaje.transform.GetComponent<Rigidbody>();
		}

		public void MoveCharacter(float _x, float _y)
		{
			rg.velocity = (Personaje.transform.forward * speedMove * _y ) + (Personaje.transform.right * speedMove * _x);
		}

		public void ChangeCameraPosition(int _dir)
		{
			if(_dir > 0)
			{
				actualPosCamera++;
				if(actualPosCamera > 3)
				{
					actualPosCamera = 0;
				}
				camara.transform.LookAt(Personaje.transform);
				Personaje.transform.Rotate(Vector3.up, 90.0f);
				//Personaje.transform.Rotate(new Vector3(0, 90 * actualPosCamera, 0));
			}
			else
			{
				actualPosCamera--;
				if(actualPosCamera < 0)
				{
					actualPosCamera = 3;
				}
				camara.transform.LookAt(Personaje.transform);
				Personaje.transform.Rotate(Vector3.up, -90.0f);
				//Personaje.transform.Rotate(new Vector3(0, 90 * actualPosCamera, 0));
			}
			Debug.Log("Pos Actual: " + actualPosCamera);
		}
	}
}