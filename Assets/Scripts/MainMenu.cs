using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	private void OnGUI() 
	{
		if (GUI.Button(new Rect(200, 100, 150, 50), "Ready To Escape"))
		{
			Application.LoadLevel(1);
		}
	}
}
