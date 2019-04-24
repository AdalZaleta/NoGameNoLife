using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace webs
{
	public class Manager_Input : MonoBehaviour {

		bool D_VinUse = false;
		bool D_HinUse = false;

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
				if(Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.JoystickButton4))
				{
					Manager_Static.controllerCharacter.ChangeCameraPosition(1);
				}
				if(Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton5))
				{
					Manager_Static.controllerCharacter.ChangeCameraPosition(-1);
				}
				if(Input.GetKeyDown(KeyCode.I) || Input.GetAxis("D_Vertical") < 0.0f)
				{
					if(!D_VinUse)
					{
						Manager_Static.uiManager.ToggleInventory(true);
						Manager_Static.appManager.currentState = AppState.inventory;
						D_VinUse = true;
					}
				}
				if(Input.GetKey(KeyCode.M) || Input.GetAxis("D_Vertical") > 0.0f)
				{
					Manager_Static.uiManager.ToggleMap(true);
				}
				if(Input.GetAxis("D_Vertical") == 0.0f)
				{
					D_VinUse = false;
					if(!Input.GetKey(KeyCode.M))
						Manager_Static.uiManager.ToggleMap(false);
				}
			}
			else if (Manager_Static.appManager.currentState == AppState.end_game) 
			{
			}
			else if (Manager_Static.appManager.currentState == AppState.credits) 
			{
			}
			else if (Manager_Static.appManager.currentState == AppState.inventory) 
			{
				if(Input.GetKeyDown(KeyCode.I) || Input.GetAxis("D_Vertical") < 0.0f)
				{
					if(!D_VinUse)
					{
						Manager_Static.uiManager.ToggleInventory(false);
						Manager_Static.appManager.currentState = AppState.gameplay;
						D_VinUse = true;
					}
				}
				if(Input.GetAxis("D_Vertical") == 0.0f)
				{
					D_VinUse = false;
				}
			}
		}

		void OnTriggerEnter(Collider other)
		{
			//CODIGO DE LOS INPUTS DEPENDIENDO DEL ESTADO DEL JUEGO
			if (Manager_Static.appManager.currentState == AppState.gameplay) 
			{
				Debug.Log("Estoy en el trigger de: " + other.gameObject + "con la etiqueta: " + other.tag);
				if(other.CompareTag("Interactuable"))
				{
					if(Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.JoystickButton1))
					{
						other.GetComponent<Interatuable_Script>().Interaction();
					}
				}
			}
		}
	}
}
