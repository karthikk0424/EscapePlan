/// <summary>
/// Interaction with the player in the Main Menu.
/// </summary>

using UnityEngine;
using System.Collections;

public class MenuInteraction : MonoBehaviour
{
	#region Player Cache
	private PlayerMotion MyPlayer;

	/// <summary>
	/// When this game object is enabled. It caches the Player info.
	/// </summary>
	private void OnEnable()
	{
		if(MyPlayer == null)
		{
			MyPlayer = GameObject.FindGameObjectWithTag (ConstantVariablesContainer.MainPlayer).GetComponent<PlayerMotion>();
		}

		if(MyPlayer == null) 
		{
			Destroy (this.gameObject);
		}
	}
	#endregion

	#region Input Orientation

	/// <summary>
	/// Called during every frame. It catches the player input. 
	/// </summary>
	private void Update()
	{
		#region Left Movement
		if(Input.GetKeyDown(KeyCode.LeftArrow))
		{
			MyPlayer.InitiateLeftTurn ();
		}
		
		if(Input.GetKey(KeyCode.LeftArrow))
		{
			MyPlayer.MoveTowardsLeft ();
		}
		
		if(Input.GetKeyUp(KeyCode.LeftArrow))
		{
			MyPlayer.StopLeftMovement ();
		}
		#endregion
		
		#region Right Movement
		// Right Movement
		if(Input.GetKeyDown(KeyCode.RightArrow))
		{
			MyPlayer.InitiateRightTurn ();
		}
		
		if(Input.GetKey(KeyCode.RightArrow))
		{
			MyPlayer.MoveTowardsRight ();
		}
		
		if(Input.GetKeyUp(KeyCode.RightArrow))
		{
			MyPlayer.StopRightMovement ();
		}
		#endregion
		
		// Jump Movement
		if(Input.GetKeyDown(KeyCode.UpArrow))
		{
			MyPlayer.MakeThePlayerToJump ();
		}
	}

	#endregion
}
