
/// <remarks>
/// Developed for Big Viking Games, London, Canada.
/// </remarks>
/// <summary>
/// Responsisble for co-ordinating the NPC.
/// </summary>
/// <description>
/// This class is responsible for controlling non playable characters and fire animations. 
/// </description>


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCManager : MonoBehaviour
{
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

	#region Variables

	public GameObject[] FireAnimation;
	public string CurrentPlayerLevel = ConstantVariablesContainer.Level1;
	private TriggerActionType lastKnowAction;

	#endregion

	#region Upon Triggers from NotifyOnTrigger Class

	/// <summary>
	/// When trigger is detected and it is first entered. 
	/// </summary>
	/// <param name="sourceObject">The source GameObject.</param>
	/// <param name="triggerType">The type of Trigger.</param>
	/// <description> 
	/// Created this module for the flexibilty during agile development to incubate various
	/// other element that have to be interacted in a fatal way with the player
	/// </description>
	internal void EnterTrigger(GameObject sourceObject, TriggerActionType triggerType)
	{
		switch(triggerType)
		{ 
			case TriggerActionType.DeathTrap:
				GameManager.Instance.DeathForPlayer();
				break;
		}
		lastKnowAction = triggerType;
	}

	/// <summary>
	/// When trigger is detected and it is exited. 
	/// </summary>
	/// <param name="sourceObject">The source game object</param>
	/// <param name="type">The type of Trigger.</param>
	/// <description> 
	/// Created this module for the flexibilty during agile development to incubate various
	/// other element that have to be interacted in a fatal way with the player
	/// </description>
	internal void ExitTrigger(GameObject sourceObject, TriggerActionType type)
	{
		switch(type)
		{ 
			case TriggerActionType.DeathTrap:
				break;
		}
	}

	#endregion

	#region Fire Animation

	/// <summary>
	/// Plays a fire animation.
	/// </summary>
	/// <param name="_worldCoordinates"> The co-ordinates where the fire has to be played</param>
	internal void PlayFireAnimation(Vector3 _worldCoordinates)
	{
		int i = 0;
		while(i < FireAnimation.Length)
		{
			if(FireAnimation[i].activeSelf == false)
			{
				FireAnimation[i].transform.position = _worldCoordinates;
				FireAnimation[i].SetActive (true);
				this.StartCoroutine( playFire (i));
				break;
			}
			else { i++;}
		}
	}

	/// <summary>
	/// Deactivates the fire animation when required. 
	/// </summary>
	/// <returns>The fire.</returns>
	/// <param name="_index">the child that has to play the fire.</param>
	private IEnumerator playFire(int _index)
	{
		yield return new WaitForSeconds(1f);
		FireAnimation[_index].SetActive(false);
	}
	#endregion

}
	