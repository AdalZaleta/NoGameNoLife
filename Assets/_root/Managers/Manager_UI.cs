using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace webs
{
	public class Manager_UI : MonoBehaviour {
		public GameObject invetoryCanvas;
		public GameObject mapCanvas;

		void Awake()
		{
			Manager_Static.uiManager = this;
		}

		public void ToggleInventory(bool _state)
		{
			invetoryCanvas.SetActive(_state);
		}

		public void ToggleMap(bool _state)
		{
			mapCanvas?.SetActive(_state);
		}
	}
}
