
/// <summary>
/// Utility Class which notifies NPCManager along with the gameObject reference when trggered
/// </summary>

using UnityEngine;
using System.Collections;

public enum TriggerActionType
{
	None,
	TweenPosition,
	SwitchCamera
}

public class NotifyOnTrigger : MonoBehaviour 
{
	public GameObject TargetObject;
	public TriggerActionType TriggerType;

	private void OnTriggerEnter2D(Collider2D hit)
	{
		NPCManager.Instance.EnterTrigger(TargetObject, TriggerType);
	}
}
