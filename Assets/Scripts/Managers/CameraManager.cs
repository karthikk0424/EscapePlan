/// <summary>
/// Class that triggers events in the level
/// Game Manager notifies Camera manager if anthing related to camera happens
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraManager : MonoBehaviour
{
	#region Singleton created on access
	private static CameraManager instance = null;
	private CameraManager() {}
	public static CameraManager Instance
	{
		get
		{
			if(instance == null)
			{
				instance = Camera.main.GetComponent<CameraManager>();
			}
			return instance;
		}
	}
	#endregion

	//Move to game Manager
	private LevelEnum currentLevel = LevelEnum.Level1;

	public void ChangeCameraToLevel(string levelName, bool reset)
	{
		Vector3 cameraPosition = StaticVariablesContainer.DEFAULT_CAMERA_POSITION;
		if(!reset)
		{
			LevelEnum currentPlayerLevel =  GameManager.Instance.CurrentPlayerLevel;
			LevelEnum levelTrigger = (LevelEnum) System.Enum.Parse (typeof(LevelEnum), levelName);

			if(currentPlayerLevel != LevelEnum.Level0 && levelTrigger == LevelEnum.Level0)
			{
				cameraPosition = new Vector3 ( 0, -20, -10 );
			}
		}

//		switch(level)
//		{
//			case LevelEnum.Level0:
//				cameraPosition = new Vector3 ( 0, -20, -10 );
//				break;
//
//			case LevelEnum.Level2:
//				cameraPosition = new Vector3 ( 0, 20, -10 );
//				break;
//		}
		transform.position = cameraPosition;
	}

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
}
