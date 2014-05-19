/// <summary>
/// 
/// </summary>


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum LevelEnum
{
	Level0,
	Level1,
	Level2
}

public class NPCManager : MonoBehaviour
{
	// Responsibilities
	/*
		1. Co-ordinates between GameManager & Enemies in a particular scene
		2. 
	 */
	#region Singleton
	private static NPCManager instance;
	public static NPCManager Instance
	{
		get{	
			if (instance == null)
			{
				GameObject go = GameObject.Find("NPCManager") as GameObject;
				if(go != null)
				{	
					instance = go.GetComponent<NPCManager>();
				}
			}
			return instance;
		}
	}
	#endregion


	public GameObject[] EnemyUnits;

	private Evlevator currentElevator;
	private TriggerActionType lastKnowAction;

	internal void EnterTrigger(GameObject sourceObject, TriggerActionType triggerType)
	{
		switch(triggerType)
		{
			case TriggerActionType.TweenPosition:
				PlayAnimation(sourceObject);
				break;
			case TriggerActionType.SwitchCamera:
				CameraManager.Instance.ChangeCameraToLevel(sourceObject.name);
				break;
			case TriggerActionType.MoveElevator:
				currentElevator = sourceObject.GetComponent<Evlevator>();
				break;

		}
		lastKnowAction = triggerType;
	}

	internal void OnCompleteAction(GameObject sourceObject)
	{

		switch(lastKnowAction)
		{
			case TriggerActionType.MoveElevator:
				currentElevator.ElevatorSwitch(true);
				break;
		}
		Debug.Log(lastKnowAction);
	}

	internal void PlayAnimation(GameObject item)
	{
		item.GetComponent<PositionTweener>().PlayAnimation();
	}

	internal void StopAnimation(GameObject item)
	{

	}

	#region Elevetor Mechnism

	public void MoveElevator()
	{
		currentElevator.StartElevator();
	}


	public void StopElevatorMovement()
	{
		StopAnimation(currentElevator.gameObject);
	}

	#endregion
}
	