using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace webs
{
	public class Manager_Input : MonoBehaviour {

		void Awake()
		{
			Manager_Static.inputManager = this;
		}

		void Update()
		{
			//CODIGO DE LOS INPUTS DEPENDIENDO DEL ESTADO DEL JUEGO
			if (Manager_Static.appManager.currentState == AppState.pause_menu) 
			{
			}
			else if (Manager_Static.appManager.currentState == AppState.main_menu) 
			{
			}
			else if (Manager_Static.appManager.currentState == AppState.gameplay) 
			{
				if(Input.GetAxis("Horizontal") == 0.0f && Input.GetAxis("Vertical") == 0.0f) 
				{
					Manager_Static.controllerCharacter.MoveCharacter(0.0f, 0.0f);
				}
				if(Input.GetAxis("Horizontal") != 0.0f || Input.GetAxis("Vertical") != 0.0f) 
				{
					Manager_Static.controllerCharacter.MoveCharacter(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
				}
				if(Input.GetKeyDown(KeyCode.Q))
				{
					Manager_Static.controllerCharacter.ChangeCameraPosition(1);
				}
				if(Input.GetKeyDown(KeyCode.E))
				{
					Manager_Static.controllerCharacter.ChangeCameraPosition(-1);
				}
			}
			else if (Manager_Static.appManager.currentState == AppState.end_game) 
			{
			}
			else if (Manager_Static.appManager.currentState == AppState.credits) 
			{
			}
		}
	}
}
