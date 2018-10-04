using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace webs
{
	public class Manager_App : MonoBehaviour {

		public AppState currentState;
		//En algun momento me tenfgo que asegurar de que el estado cambie conforme al estado del juego

		void Awake()
		{
			Manager_Static.appManager = this;
		}

		public void SetPause()
		{
			Manager_Static.appManager.currentState = AppState.pause_menu;
			Time.timeScale = 0.0f;
		}

		public void  setPlay()
		{
			Manager_Static.appManager.currentState = AppState.gameplay;
			Time.timeScale = 1;
		}
	}
}
