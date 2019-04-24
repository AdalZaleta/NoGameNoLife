using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace webs
{
	public enum AppState
	{
		main_menu,
		gameplay,
		pause_menu,
		end_game,
		credits,
		inventory,
	}

	public enum ControllerSide
	{
		player1 = 0,
		player2 = 1
	}

	public static class Manager_Static 
	{
		public static Manager_Input inputManager;
		public static Manager_App appManager;
		public static Manager_Scene sceneManager;
		public static Manager_UI uiManager;
		public static CharacterController controllerCharacter;
	}
}
