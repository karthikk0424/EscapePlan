using UnityEngine;
using System.Collections;

public class UIInput : MonoBehaviour 
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
				switch(Application.loadedLevel)
				{
					case 0:
						Application.LoadLevel(1);
						break;

					case 1:
						GameManager.Instance.ToggleGameState(false);
						break;
				}

				break;

			case TypeOfButton.ExitGame:
				switch(Application.loadedLevel)
				{
					case 0:
						Application.Quit();
						break;
						
					case 1:
						Application.LoadLevel(0);
						break;
				}

				break;
		}
	}
}
