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
	public GameObject[] FireAnimation;
	public string CurrentPlayerLevel = StaticVariablesContainer.Level1;
	private TriggerActionType lastKnowAction;

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

	internal void ExitTrigger(GameObject sourceObject, TriggerActionType type)
	{
		switch(type)
		{ 
			case TriggerActionType.DeathTrap:
				break;
		}
	}

	#region Fire Animation
	internal void PlayFireAnimation(Vector3 _worldCoordinates)
	{
		int i = 0;
		while(i < FireAnimation.Length)
		{
			if(FireAnimation[i].activeSelf == false)
			{
				FireAnimation[i].transform.position = _worldCoordinates;
				FireAnimation[i].SetActive(true);
				this.StartCoroutine(playFire(i));
				break;
			}
			else { i++;}
		}
	}
	
	private IEnumerator playFire(int index)
	{
		yield return new WaitForSeconds(1f);
		FireAnimation[index].SetActive(false);
	}
	#endregion

}
	