/// <summary>
/// Class that triggers events in the level
/// Game Manager notifies Camera manager if anthing related to camera happens
/// </summary>

using UnityEngine;
using System.Collections;
//using System.Collections.Generic;

public class CameraController : MonoBehaviour
{
	#region Singleton created on access
	private static CameraController instance = null;
	public static CameraController Instance
	{
		get
		{
			if(instance == null)
			{
				instance = Camera.main.GetComponent<CameraController>();
			}
			return instance;
		}
	}
	#endregion
	private void Awake()
	{
		if(instance != null)
		{
			Debug.LogError("<color=red>ATTENTION</color> : Two instances of Camera");
			Destroy(this.gameObject);
		}
		instance = this;
	}

	private void OnEnable()
	{
		GameManager.OnReset += HandleOnReset;
	}

	private void HandleOnReset ()
	{
		this.transform.localPosition = startPosition;
	}

	private void OnDisable()
	{
		GameManager.OnReset -= HandleOnReset;
	}

	private Vector3 defaultMoviableUnits = new Vector3(0, 20, 0);
	private Vector3 startPosition = new Vector3(0, 0, -10);

	internal void MoveCamera(bool _moveUP)
	{
		switch(_moveUP)
		{
			case true:
				this.transform.localPosition += defaultMoviableUnits;
				break;


			case false:
				this.transform.localPosition -= defaultMoviableUnits;
				break;
		}
	}

	internal void SetCameraToThisPosition(Vector3 _pos)
	{
		this.transform.localPosition = _pos;
	}


	//Move to game Manager
//	private LevelEnum currentLevel = LevelEnum.Level1;
/*
	public void ChangeCameraToLevel(string levelName, bool reset)
	{
		Vector3 cameraPosition = StaticVariablesContainer.DEFAULT_CAMERA_POSITION;
		if(!reset)
		{
			LevelEnum levelTrigger = (LevelEnum) System.Enum.Parse (typeof(LevelEnum), levelName);

			switch(levelTrigger)
			{
				case LevelEnum.Level0:
					cameraPosition = new Vector3 (0, -20, -10 );
					break;
	
				case LevelEnum.Level2:
					cameraPosition = new Vector3 (0, -40, -10 );
					break;
			}
		}
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
	*/
}
