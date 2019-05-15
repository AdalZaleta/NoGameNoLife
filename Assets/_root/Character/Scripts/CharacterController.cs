using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace webs
{
	public class CharacterController : MonoBehaviour {

		public GameObject Personaje;
		public Camera camara;
		public float speedMove;
		public float potecniaSalto;
		private bool isHiting;

		public Vector3[] positionsCamera;
		int actualPosCamera = 0; 

		void Awake() {
			Manager_Static.controllerCharacter = this;
		}

		public void MoveCharacter(float _x, float _y)
		{
			Personaje.transform.GetComponent<Rigidbody>().velocity = new Vector3(_x * speedMove, Personaje.transform.GetComponent<Rigidbody>().velocity.y, _y * speedMove);
		}

		public void JumpCharacter()
		{
			RaycastHit hit;
			isHiting = Physics.Raycast(Personaje.transform.position, Personaje.transform.TransformDirection(Vector3.down), out hit, 1.1f);
			Debug.Log("Entre a la funcion de salto");
			if (isHiting) {
				Debug.Log("Estoy dentro del IF");
				Personaje.transform.GetComponent<Rigidbody>().velocity = Vector3.up * potecniaSalto;
			}
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
				camara.transform.localPosition = positionsCamera[actualPosCamera];
				camara.transform.LookAt(Personaje.transform);
				//Personaje.transform.Rotate(new Vector3(0, 90 * actualPosCamera, 0));
			}
			else
			{
				actualPosCamera--;
				if(actualPosCamera < 0)
				{
					actualPosCamera = 3;
				}
				camara.transform.localPosition = positionsCamera[actualPosCamera];
				camara.transform.LookAt(Personaje.transform);
				//Personaje.transform.Rotate(new Vector3(0, 90 * actualPosCamera, 0));
			}
		}
	}
}