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
	public string CurrentPlayerLevel = StaticVariablesContainer.Level1;
	private Elevator currentElevator;
	private TriggerActionType lastKnowAction;

	internal void EnterTrigger(GameObject sourceObject, TriggerActionType triggerType)
	{
		switch(triggerType)
		{
			case TriggerActionType.TweenPosition:
	
				break;

			case TriggerActionType.MoveElevator:
				currentElevator = sourceObject.GetComponent<Elevator>();
				break;

			case TriggerActionType.DeathTrap:
				GameManager.Instance.DeathForPlayer();
				break;
		}
		lastKnowAction = triggerType;
	}

	internal void ExitTrigger(GameObject sourceObject, TriggerActionType type)
	{

		switch(type)
		{
			case TriggerActionType.DeactivateElevator:
				currentElevator = null;
				break;

			case TriggerActionType.SwitchCamera:
						
				string currentLevel = CurrentPlayerLevel;
				if( (sourceObject.name == StaticVariablesContainer.Level0) && (currentLevel == StaticVariablesContainer.Level1))
				{
					currentLevel = StaticVariablesContainer.Level0; // Level 0 
				}
				else if((sourceObject.name == StaticVariablesContainer.Level0) && (currentLevel == StaticVariablesContainer.Level0))
				{
					currentLevel = StaticVariablesContainer.Level1;
				}
				
				else if((sourceObject.name == StaticVariablesContainer.Level2) && (currentLevel == StaticVariablesContainer.Level0))
				{
					currentLevel = StaticVariablesContainer.Level2;// Level 2 
				}
					
				else if((sourceObject.name == StaticVariablesContainer.Level2) && (currentLevel == StaticVariablesContainer.Level2))
				{
					currentLevel = StaticVariablesContainer.Level0;
				}
				CurrentPlayerLevel = currentLevel;
//				CameraController.Instance.ChangeCameraToLevel(currentLevel, false);
				break;
		}
	}
}
	