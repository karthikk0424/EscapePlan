
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
	public GameObject TargetObject;
	public TriggerActionType OnEnterAction;
	public TriggerActionType OnExitAction;

	private void OnTriggerEnter2D(Collider2D hit) // ISSUE : If the NPCManager is with the Scene prefab then the instance is null. Once 
	{				
		// make a correction - should have a if loop to check null - in this loop. 

		if(hit.CompareTag("Player"))
		{
			NPCManager.Instance.EnterTrigger(TargetObject, OnEnterAction);
		}
	}

	private void OnTriggerExit2D(Collider2D hit)
	{
		if(hit.CompareTag("Player"))
		{
			NPCManager.Instance.ExitTrigger(TargetObject, OnExitAction);
		}
	}
}
