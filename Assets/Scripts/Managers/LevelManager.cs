/// <summary>
/// Class that triggers events in the level
/// Game Manager notifies level manager anthing related to level happens
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class LevelManager
{
	#region Singleton created on access
	private static LevelManager instance = null;
	private LevelManager() {}
	
	public static LevelManager Instance
	{
		get
		{
			if(instance == null)
			{
				instance = new LevelManager();
			}
			return instance;
		}
	}
	#endregion

	private List<GameObject> TweenerList = new List<GameObject>();

	public void RegisterToPlayAnimation(GameObject item)
	{
		TweenerList.Add(item);
		Debug.Log("+++++ Registered gameObject is " + item.gameObject.name );
	}

	public void ClearEventList()
	{
		TweenerList.Clear();
	}

	//This method receives state updates from Scenemanager
	public void CheckForLevelEvents()
	{
		if(TweenerList.Count > 0)
		{
			foreach(GameObject go in TweenerList)
			{
				Debug.Log(go.activeSelf);
				if(go.activeSelf)
				{
					go.GetComponent<PositionTweener>().PlayAnimation();
				}
			}
		}
	}
}
