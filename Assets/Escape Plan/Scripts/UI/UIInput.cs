
/// <summary>
/// Present in the button for the Main Menu & In Game Pause screen. 
/// </summary>
/// 
using UnityEngine;
using System.Collections;

public class UIInput : MonoBehaviour 
{
	#region Variables

	public enum TypeOfButton
	{
		None,
		EnterGame, 
		ExitGame
	}
	public TypeOfButton ThisButton;

	#endregion

	#region Upon Clicking the button

	/// <summary>
	/// When left click is pressed on the collider, this event is raised.
	/// </summary>
	private void OnMouseDown()
	{
		switch (ThisButton)
		{
			case TypeOfButton.EnterGame:
				switch (Application.loadedLevel)
				{
					case 0:
						Application.LoadLevel (1);
						break;

					case 1:
						GameManager.Instance.ToggleGameState (false);
						break;
				}

				break;

			case TypeOfButton.ExitGame:
				switch (Application.loadedLevel)
				{
					case 0:
						Application.Quit ();
						break;
						
					case 1:
						Application.LoadLevel (0);
						break;
				}

				break;
		}
	}

	#endregion
}
