using UnityEngine;
using System.Collections;

public class MenuInteraction : MonoBehaviour
{

	private PlayerMotion MyPlayer;

	private void OnEnable()
	{
		if(MyPlayer == null)
		{
			MyPlayer = GameObject.FindGameObjectWithTag(StaticVariablesContainer.MainPlayer).GetComponent<PlayerMotion>();
		}
	}

	private void Update()
	{
		#region Left Movement
		if(Input.GetKeyDown(KeyCode.LeftArrow))
		{
			MyPlayer.InitiateLeftTurn();
		}
		
		if(Input.GetKey(KeyCode.LeftArrow))
		{
			MyPlayer.MoveTowardsLeft();
		}
		
		if(Input.GetKeyUp(KeyCode.LeftArrow))
		{
			MyPlayer.StopLeftMovement();
		}
		#endregion
		
		#region Right Movement
		// Right Movement
		if(Input.GetKeyDown(KeyCode.RightArrow))
		{
			MyPlayer.InitiateRightTurn();
		}
		
		if(Input.GetKey(KeyCode.RightArrow))
		{
			MyPlayer.MoveTowardsRight();
		}
		
		if(Input.GetKeyUp(KeyCode.RightArrow))
		{
			MyPlayer.StopRightMovement();
		}
		#endregion
		
		// Jump Movement
		if(Input.GetKeyDown(KeyCode.UpArrow))
		{
			MyPlayer.MakeThePlayerToJump();
		}
	}
}
