
/// <summary>
/// Utility Class which notifies NPCManager along with the gameObject reference when trggered
/// </summary>

using UnityEngine;
using System.Collections;

public enum TriggerActionType
{
	None,
	TweenPosition,
	SwitchCamera,
	MoveElevator,
	DeathTrap,
	DeactivateElevator,
	PlayerLevel
}

public class NotifyOnTrigger : MonoBehaviour 
{
	#region Variables

	public GameObject TargetObject;
	public TriggerActionType OnEnterAction;
	public TriggerActionType OnExitAction;

	#endregion

	#region Trigger Methods

	/// <summary>
	/// When it triggers with the player on entering.
	/// </summary>
	/// <param name="hit">info about the hit</param>
	private void OnTriggerEnter2D(Collider2D hit) // ISSUE : If the NPCManager is with the Scene prefab then the instance is null. Once 
	{				
		// make a correction - should have a if loop to check null - in this loop. 

		if(hit.CompareTag(ConstantVariablesContainer.MainPlayer))
		{
			NPCManager.Instance.EnterTrigger(TargetObject, OnEnterAction);
		}
	}

	/// <summary>
	/// When it triggers with the player on exiting.
	/// </summary>
	/// <param name="hit">info about the hit</param>
	private void OnTriggerExit2D(Collider2D hit)
	{
		if(hit.CompareTag(ConstantVariablesContainer.MainPlayer))
		{
			NPCManager.Instance.ExitTrigger(TargetObject, OnExitAction);
		}
	}

	#endregion
}
