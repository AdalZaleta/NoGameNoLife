using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Rewired;
using System;

namespace webs
{
	public class Manager_Input : MonoBehaviour {

		bool D_VinUse = false;
		bool D_HinUse = false;
        bool Inventory = false;
        // The Rewired player id of this character
        public int playerId = 0;
        private Player player; // The Rewired Player

        [HideInInspector]
        public Vector2 moveVector;

        void Awake()
		{
			Manager_Static.inputManager = this;

            // Get the Rewired Player object for this player and keep it for the duration of the character's lifetime
            player = ReInput.players.GetPlayer(playerId);
        }

		void Update()
		{
            GetInput();
            ProcessInput();
		}

        private void ProcessInput()
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
                Manager_Static.controllerCharacter.MoveCharacter(moveVector);
                if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.JoystickButton4))
                {
                    Manager_Static.controllerCharacter.ChangeCameraPosition(1);
                }
                if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton5))
                {
                    Manager_Static.controllerCharacter.ChangeCameraPosition(-1);
                }
                if (Inventory)
                { 
                    Manager_Static.uiManager.ToggleInventory(true);
                    Manager_Static.appManager.currentState = AppState.inventory;
                }
                if (Input.GetKey(KeyCode.M) || Input.GetAxis("D_Vertical") > 0.0f)
                {
                    Manager_Static.uiManager.ToggleMap(true);
                }
                if (Input.GetAxis("D_Vertical") == 0.0f)
                {
                    D_VinUse = false;
                    if (!Input.GetKey(KeyCode.M))
                        Manager_Static.uiManager.ToggleMap(false);
                }
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0))
                {
                    Manager_Static.controllerCharacter.JumpCharacter();
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
                Manager_Static.controllerCharacter.MoveCharacter(Vector2.zero);
                if (Inventory)
                {
                    Manager_Static.uiManager.ToggleInventory(false);
                    Manager_Static.appManager.currentState = AppState.gameplay;
                }
            }
        }

        private void GetInput()
        {
            moveVector.x = player.GetAxis("Move Horizontal"); // get input by name or action id
            moveVector.y = player.GetAxis("Move Vertical");

            Inventory = player.GetButtonDown("Inventory");
        }
    }
}
