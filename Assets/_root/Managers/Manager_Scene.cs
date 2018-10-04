using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace webs
{
	public class Manager_Scene : MonoBehaviour {

		public int currentScene;

		void Awake()
		{
			Manager_Static.sceneManager = this;
		}

		public void LoadScene(int _id, bool _isAditive)
		{
			if (_isAditive)
				SceneManager.LoadScene (_id, LoadSceneMode.Additive);
			if (!_isAditive)
				SceneManager.LoadScene (_id);
		}

		public void LoadSceneName(string _name)
		{
			SceneManager.LoadScene (_name);
		}

		public void ExitApplication()
		{
			Application.Quit ();
		}
	}
}
