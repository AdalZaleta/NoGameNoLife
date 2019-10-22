using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Rewired;
using System;

namespace webs
{
	public class Manager_Input : MonoBehaviour {

        public CharacterMovement movement;

        bool Inventory = false;
        bool Jump = false;
        bool Map = false;
        bool rotateL = false;
        bool rotateR = false;

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
            //ProcessInput();
		}

        private void FixedUpdate()
        {
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
                movement.Move(moveVector.x, moveVector.y);

                //Manager_Static.controllerCharacter.MoveCharacter(moveVector);
                if (rotateL)
                {
                    movement.ChangeCameraPosition(1);
                    Manager_Static.controllerCharacter.ChangeCameraPosition(1);
                }
                if (rotateR)
                {
                    movement.ChangeCameraPosition(-1);
                    Manager_Static.controllerCharacter.ChangeCameraPosition(-1);
                }
                if (Jump)
                {
                    movement.Jump();
                    //Manager_Static.controllerCharacter.JumpCharacter();
                }
                if (Inventory)
                { 
                    Manager_Static.uiManager.ToggleInventory(true);
                    Manager_Static.appManager.currentState = AppState.inventory;
                }
                if (Map)
                {
                    Manager_Static.uiManager.ToggleMap(true);
                }
                if (!Map)
                {
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
            Jump = player.GetButtonDown("Jump");
            Map = player.GetButtonSinglePressHold("Map");
            rotateL = player.GetButtonDown("LRotate Camera");
            rotateR = player.GetButtonDown("RRotate Camera");
        }
    }
}
