
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
	DeactivateElevator
}

public class NotifyOnTrigger : MonoBehaviour 
{
	public GameObject TargetObject;
	public TriggerActionType OnEnterAction;
	public TriggerActionType OnExitAction;

	private void OnTriggerEnter2D(Collider2D hit)
	{
		NPCManager.Instance.EnterTrigger(TargetObject, OnEnterAction);
	}

	private void OnTriggerExit2D(Collider2D hit)
	{
		NPCManager.Instance.ExitTrigger(TargetObject, OnExitAction);
	}
}
