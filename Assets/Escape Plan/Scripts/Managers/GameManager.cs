
/// <remarks>
/// Developed for Big Viking Games, London, Canada.
/// </remarks>
/// <summary>
/// The primary class that is responsible for the entire functionality of the game. 
/// </summary>
/// <description>
/// It translates input from the player to provide the appropritate functionlity. It looks after the player. Changes the state of the
/// game whenever required. All the other class are required to request this class for state changes. It is present on every scene, except the 
/// Main Menu. 
/// </description>

using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
	#region Public Variables that require external linkage.

	public PlayerMotion MyPlayer;
	public GameObject TransitionScene;
	public KeyCode[] AssignedKeys;
	public GameGUI EscapePlanGUI;

	#endregion

	#region Private Variables

	private GameObject currentSceneInstance;
	private CameraController myCamera;
	private Vector3 playerSpawnPoint = new Vector3(0,0,0);
	private bool detectINPUT = false;

	#endregion

	#region Delegates to implement Observer pattern 

	public delegate void PauseTheGame(bool toPAUSE);
	public static event PauseTheGame OnPauseTheGame;

	public delegate void ResetTheLevel();
	public static event ResetTheLevel OnReset;

	public delegate void CoinRotation(Quaternion _rot);
	public static event CoinRotation onRotation;

	private Quaternion rotationChips = Quaternion.identity;

	#endregion

	#region MonoBehaviour Methods - Start & End

	/// <summary>
	/// This is the very methods that is being called beyond any other classes. 
	/// </summary>
	private void Awake()
	{
		if(instance != null)
		{
			// Duplicate instance is destroyed - just in case.
			Destroy(this.gameObject);
		}
		instance = this;

		this.transform.name = "_GameManager";
		this.transform.tag = ConstantVariablesContainer.Manager;

		// Fetch user data and start from that level, else load the default level 1;
		DataManager.Instance.CurrentLevelNumber = 1;
		DataManager.Instance.LifeCount = 3;
		loadLevel(DataManager.Instance.CurrentLevelNumber, true);
		if(AssignedKeys.Length < 1)
		{
			AssignedKeys = new KeyCode[4];
			// Left Movement
			AssignedKeys[0] = KeyCode.LeftArrow;
			
			// Right Movement
			AssignedKeys[1] = KeyCode.RightArrow;
			
			// Jump 
			AssignedKeys[2] = KeyCode.UpArrow;
			
			// Fire a Bullet
			AssignedKeys[3] = KeyCode.Space;
		}
	}

	/// <summary>
	/// Called when this gameobject is being enabled.
	/// </summary>
	/// <description>
	/// Though the reference value types are cached earlier, this check it to prevent any disruptions. A fail safe.
	/// </description>
	private void OnEnable()
	{
		if(MyPlayer == null)
		{
			MyPlayer = GameObject.Find("MainPlayer").GetComponent<PlayerMotion>();
		}
		if(myCamera == null)
		{
			myCamera = Camera.main.GetComponent<CameraController>();
		}
	}

	/// <summary>
	/// Called after OnEnable() method when this gameobject is enabled. 
	/// </summary>
	private void Start()
	{
		detectINPUT = true;
		StartCoroutine( MyPlayer.SetPlayerProperties (true));
	}

	/// <summary>
	/// Called when the game object is being deleted. 
	/// </summary>
	/// <description>
	/// Happens when you load another scene. Derefencing reference types for the Garbage Collector to 
	/// pick-up.
	/// </description>
	private void OnDestroy()
	{
		instance = null;
		myCamera = null;
		MyPlayer = null;
		TransitionScene = null;
		EscapePlanGUI = null;
	}
	#endregion

	#region Singleton
	private static GameManager instance;
	public static GameManager Instance
	{
		get
		{
			if(instance == null)
			{	
				instance = UnityEngine.Object.FindObjectOfType(typeof(GameManager)) as GameManager;
				if (instance == null)
				{		
					GameObject go = GameObject.Find("_GameManager") as GameObject;
					if(go != null)
					{	instance = go.AddComponent<GameManager>();}
				}
				if (instance == null)
				{
					GameObject go = new GameObject("Base") as GameObject;
					instance = go.AddComponent<GameManager>();		
				}
			}
			return instance;
		}
	}
	#endregion
	
	#region Getters & Setters

	/// <summary>
	/// Gets the player position. A read-only type.
	/// </summary>
	/// <value>Contains the Vector3 value of the players position</value>
	internal Vector3 PlayerPosition
	{
		get
		{
			if(MyPlayer != null)
			{
				return MyPlayer.transform.position;
			}
			else 
			{
				MyPlayer = GameObject.Find("MainPlayer").GetComponent<PlayerMotion>();
				if(MyPlayer == null)
				{	return Vector3.zero;}
				return MyPlayer.transform.position;
			}
		}
	}

	#endregion

	#region Update
	
	/// <summary>
	/// This monobehaviour method is called every frame, as long as this game object is active.
	/// </summary>
	/// <description>
	/// Focussed on acting based on user's input.
	/// </description>
	private void Update () 
	{
		if(detectINPUT)
		{
			#region Left Movement
			if(Input.GetKeyDown( AssignedKeys[0]))
			{
				MyPlayer.InitiateLeftTurn ();
			}

			if(Input.GetKey( AssignedKeys[0]))
			{
				MyPlayer.MoveTowardsLeft ();
			}

			if(Input.GetKeyUp( AssignedKeys[0]))
			{
				MyPlayer.StopLeftMovement ();
			}
			#endregion

			#region Right Movement
			// Right Movement
			if(Input.GetKeyDown( AssignedKeys[1]))
			{
				MyPlayer.InitiateRightTurn ();
			}

			if(Input.GetKey( AssignedKeys[1]))
			{
				MyPlayer.MoveTowardsRight ();
			}
			
			if(Input.GetKeyUp( AssignedKeys[1]))
			{
				MyPlayer.StopRightMovement ();
			}
			#endregion

			// Jump Movement
			if(Input.GetKeyDown( AssignedKeys[2]))
			{
				MyPlayer.MakeThePlayerToJump ();
			}

			// Fire a projectile
			if(Input.GetKeyDown( AssignedKeys[3]))
			{
				if(DataManager.Instance.WeaponReadyStatus)
				{
					MyPlayer.FireAProjectile ();
				}
				else
				{
					EscapePlanGUI.UpdateInfoText("Find ammo");
				}
			}

			// Rotates the Chips. 
			if(onRotation != null)
			{
				rotationChips *= Quaternion.Euler (0, (Time.deltaTime * 100), 0);
				onRotation (rotationChips);	
			}
		}
		if(Input.GetKeyDown( KeyCode.Escape))
		{
			ToggleGameState (detectINPUT);
		}

		if((Input.GetKeyDown( KeyCode.D) && (DataManager.Instance.CurrentLevelNumber == 9)))
		{
			DataManager.Instance.CurrentLevelNumber = 7;
			DataManager.Instance.HackKit = true;
			OpenDoor ();
		}
	}
	#endregion

	#region Collectables

	/// <summary>
	/// When user collects a CHIP
	/// </summary>
	internal void GotAChip()
	{
		int chipCount = DataManager.Instance.ChipLootSac;
		chipCount = chipCount + ConstantVariablesContainer.CHIP_VALUE;
		DataManager.Instance.ChipLootSac = chipCount; 
		EscapePlanGUI.UpdateChipCount ();
		UpdateLifeBonusTracker ();
	} 

	/// <summary>
	/// When user gets the Hack kit.
	/// </summary>
	internal void GotHackKit()
	{
		DataManager.Instance.HackKit = true;
		EscapePlanGUI.UpdateInfoText ("Got the hack kit for the door");
	}

	/// <summary>
	/// When user the ammo to fire at an enemy
	/// </summary>
	internal void GotAmmo()
	{
		DataManager.Instance.WeaponReadyStatus = true;
	}

	#endregion

	#region Minor State Changes

	/// <summary>
	/// Toggles the Active state of the game - between Pause & UnPause.
	/// </summary>
	/// <param name="toPAUSE">If set to <c>true</c> Pauses the game.</param>
	internal void ToggleGameState(bool _toPAUSE)
	{
		detectINPUT = _toPAUSE;
		switch(detectINPUT)
		{
			// Pause the game
			case true:
				detectINPUT = false;
				Time.timeScale = 0.0001f;
				EscapePlanGUI.TogglePauseMenu (true);
				break;
				
			// Un Pause the game.
			case false:
				detectINPUT = true;
				Time.timeScale = 1f;
				EscapePlanGUI.TogglePauseMenu (false);
				break;
		}
		// The event is sent to all subscribers.
		if(OnPauseTheGame != null)
		{
			OnPauseTheGame (detectINPUT);
		}
	}

	/// <summary>
	/// Opens the door present at every level to progress to the next level.
	/// </summary>
	internal void OpenDoor()
	{
		if(DataManager.Instance.HackKit)
		{
			levelTransition (true);
		}
		else 
		{
			EscapePlanGUI.UpdateInfoText ("Find a hack kit");
		}
	}

	/// <summary>
	/// When the user enter a trap door instead of the other door.
	/// </summary>
	internal void TrapDoor()
	{
		if(DataManager.Instance.HackKit)
		{
			Destroy (currentSceneInstance);
			loadLevel(ConstantVariablesContainer.TRAP_DOOR_LEVEL,false);
			MyPlayer.TelePortPlayer(new Vector3(0,16,0));
			myCamera.SetCameraToThisPosition (ConstantVariablesContainer.DEFAULT_CAMERA_POSITION);
		}
		else 
		{
			EscapePlanGUI.UpdateInfoText("Find a hack kit");
		}
	}

	/// <summary>
	/// When a new level is to be loaded. Usualy is called from Transistion level.
	/// </summary>
	internal void EnterLevel()
	{
		Destroy (currentSceneInstance);
		loadLevel ((DataManager.Instance.CurrentLevelNumber + 1), true);
	}

	/// <summary>
	/// Resets the player to spawn point.
	/// </summary>
	/// <returns>Waits for the delay before it could reposition the player</returns>
	private IEnumerator ResetPlayerToSpawnPoint()
	{
		detectINPUT = false;
		yield return new WaitForSeconds(ConstantVariablesContainer.RESPAWN_DELAY);
		// an event to reposition the subscribers. 
		if(OnReset != null)
		{
			OnReset();
		}
		MyPlayer.TelePortPlayer(playerSpawnPoint);
		detectINPUT = true;
	}

	/// <summary>
	/// Adds an extra life upon reaching the optimal bonus requirements.
	/// </summary>
	/// <returns><c>true</c>, if life was added, <c>false</c> otherwise.</returns>
	private bool addLife()
	{
		if(DataManager.Instance.LifeCount < 3)
		{
			DataManager.Instance.LifeCount++;
			EscapePlanGUI.UpdatePlayerLife();
			return true;
		}
		else 
		{
			return false;
		}
	}

	/// <summary>
	/// Triggered when the player dies.
	/// </summary>
	internal void DeathForPlayer()
	{
		detectINPUT = false;
		NPCManager.Instance.PlayFireAnimation (PlayerPosition);
		DataManager.Instance.LifeCount--;
		EscapePlanGUI.UpdatePlayerLife();
		if(DataManager.Instance.LifeCount < 1)
		{
			Application.LoadLevel(0);
			DataManager.Instance.HackKit = false;
		}
		else
		{
			StartCoroutine( MyPlayer.SetPlayerProperties (false));
			StartCoroutine( ResetPlayerToSpawnPoint ());
		}
	}

	/// <summary>
	/// Toggles the user controls.
	/// </summary>
	/// <param name="toENABLE">If set to <c>true</c>enables the user control</param>
	internal void ToggleUserControls(bool _toENABLE)
	{
		detectINPUT = _toENABLE;
	}
	#endregion

	#region Scene transitition

	/// <summary>
	/// Moves the orthographic camera
	/// </summary>
	/// <param name="_toUP">If set to <c>true</c> move the camera on the positive Y axis</param>
	internal void MoveCameraUp(bool _toUP)
	{
		myCamera.MoveCamera(_toUP);
	}

	/// <summary>
	/// Loads the level.
	/// </summary>
	/// <param name="levelNumber">The index of the level to load</param>
	/// <param name="isTransitionRequired">If set to <c>true</c> the transitition scene is required</param>
	private void loadLevel(int _levelNumber, bool _isTRANSITIONREQUIRED)
	{
		if(_levelNumber > 10)
		{ 
			Application.LoadLevel(0);
		}
		currentSceneInstance = (GameObject)Instantiate(Resources.Load("Scenes/Scene_" + _levelNumber.ToString()));
		playerSpawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint").GetComponent<Transform>().position;
		DataManager.Instance.HackKit = false;
		DataManager.Instance.CurrentLevelNumber = _levelNumber;
		if(_isTRANSITIONREQUIRED)
		{
			levelTransition(false);
		}
	}

	/// <summary>
	/// Levels the transition.
	/// </summary>
	/// <param name="_hideLEVEL">If set to <c>true</c> hides transition level</param>
	private void levelTransition(bool _hideLEVEL)
	{
		if(_hideLEVEL)
		{
			currentSceneInstance.SetActive(!_hideLEVEL);
			EscapePlanGUI.gameObject.SetActive(!_hideLEVEL);
			TransitionScene.SetActive(_hideLEVEL);
			playerSpawnPoint = ConstantVariablesContainer.TRANSITION_SPAWNPOINT;
			myCamera.SetCameraToThisPosition (ConstantVariablesContainer.DEFAULT_CAMERA_POSITION);
		}
		else
		{
			TransitionScene.SetActive(_hideLEVEL);
			EscapePlanGUI.gameObject.SetActive(!_hideLEVEL);
			currentSceneInstance.SetActive(!_hideLEVEL);
		}
		MyPlayer.TelePortPlayer(playerSpawnPoint);
	}

	/// <summary>
	/// Updates the life bonus tracker.
	/// </summary>
	/// <description>
	/// Whenever the chip is being collected. This method is called. 
	/// For every 25 chips - one bonus life is supplied. If the player has 3 lives, it waits
	/// till the player loses one. Upon collection of the next chip - the player is awarded a bonus
	/// life and the bonus tracker is reset. 
	/// </description>
	private void UpdateLifeBonusTracker()
	{
		int BonusCounter = DataManager.Instance.BonusTrackerChipCount;
		BonusCounter++;
		if(BonusCounter >= ConstantVariablesContainer.BONUS_LIFE_TARGET)
		{
			if(addLife())
			{
				BonusCounter = 0;
			}
		}
		DataManager.Instance.BonusTrackerChipCount = BonusCounter;
	}
	#endregion
}
