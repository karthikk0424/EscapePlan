/// <summary>
/// 
/// </summary>


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

	internal void EnterTrigger(GameObject sourceObject, TriggerActionType triggerType)
	{
		switch(triggerType)
		{
			case TriggerActionType.TweenPosition:
				PlayAnimation(sourceObject);
				break;
			case TriggerActionType.SwitchCamera:
			Debug.Log(sourceObject);
				CameraManager.Instance.ChangeCameraToLevel(sourceObject.name);
				break;
		}
	}
	internal void PlayAnimation(GameObject item)
	{
		item.GetComponent<PositionTweener>().PlayAnimation();
	}
}
	