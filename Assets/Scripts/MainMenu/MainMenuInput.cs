using UnityEngine;
using System.Collections;

public class MainMenuInput : MonoBehaviour 
{
	public enum TypeOfButton
	{
		None,
		EnterGame, 
		ExitGame
	}
	public TypeOfButton ThisButton;

	private void OnMouseDown()
	{
		switch(ThisButton)
		{
			case TypeOfButton.EnterGame:
				Application.LoadLevel(1);
				break;

			case TypeOfButton.ExitGame:
				Application.Quit();
				break;
		}
	}
}
