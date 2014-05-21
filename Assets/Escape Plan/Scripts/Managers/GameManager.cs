using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
	public PlayerMotion MyPlayer;

	[Range (1,7)]
	public int LevelNumber = 0;
	public GameObject TransitionScene;
	public KeyCode[] AssignedKeys;
	public GameGUI EscapePlanGUI;
	
	private GameObject currentSceneInstance;
	private CameraController myCamera;
	private Vector3 playerSpawnPoint = new Vector3(0,0,0);
	private bool detectINPUT = false;

	//PAUSE the game - Observer Pattern. 
	public delegate void PauseTheGame(bool toPAUSE);
	public static event PauseTheGame OnPauseTheGame;

	public delegate void ResetTheLevel();
	public static event ResetTheLevel OnReset;

	public delegate void CoinRotation(Quaternion _rot);
	public static event CoinRotation onRotation;

	private Quaternion rotationCoin = Quaternion.identity;


	#region MonoBehaviour Methods - Start & End
	private void Awake()
	{
		if(instance != null)
		{
			// Duplicate instance
			Destroy(this.gameObject);
		}

		instance = this;
		this.transform.name = "_GameManager";

		// Fetch user data and start from that level, else load the default level 0;
		LoadLevel(LevelNumber);

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

	private void Start()
	{
		detectINPUT = true;
		StartCoroutine( MyPlayer.SetPlayerProperties (true));
	}

	private void OnDestroy()
	{
		instance = null;
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
	// Update is called once per frame
	private void Update () 
	{
		if(detectINPUT)
		{
			#region Left Movement
			if(Input.GetKeyDown(AssignedKeys[0]))
			{
				MyPlayer.InitiateLeftTurn();
			}

			if(Input.GetKey(AssignedKeys[0]))
			{
				MyPlayer.MoveTowardsLeft();
			}

			if(Input.GetKeyUp(AssignedKeys[0]))
			{
				MyPlayer.StopLeftMovement();
			}
			#endregion

			#region Right Movement
			// Right Movement
			if(Input.GetKeyDown(AssignedKeys[1]))
			{
				MyPlayer.InitiateRightTurn();
			}

			if(Input.GetKey(AssignedKeys[1]))
			{
				MyPlayer.MoveTowardsRight();
			}
			
			if(Input.GetKeyUp(AssignedKeys[1]))
			{
				MyPlayer.StopRightMovement();
			}
			#endregion

			// Jump Movement
			if(Input.GetKeyDown(AssignedKeys[2]))
			{
				MyPlayer.MakeThePlayerToJump();
			}

			// Fire a projectile
			if(Input.GetKeyDown(AssignedKeys[3]))
			{
				if(DataManager.Instance.WeaponReadyStatus)
				{
					MyPlayer.FireAProjectile();
				}
				else
				{
					EscapePlanGUI.UpdateInfoText("Find ammo");
				}
			}

			if(onRotation != null)
			{
				rotationCoin *= Quaternion.Euler (0, (Time.deltaTime * 100), 0);
				onRotation(rotationCoin);	
			}
		}

		if(Input.GetKeyDown(KeyCode.Escape))
		{
			ToggleGameState(detectINPUT);
		}
	}
	#endregion

	#region Collectables
	internal void GotAChip()
	{
		int chipCount = DataManager.Instance.ChipLootSac;
		chipCount = chipCount + StaticVariablesContainer.CHIP_VALUE;
		DataManager.Instance.ChipLootSac = chipCount; 
		EscapePlanGUI.UpdateChipCount();
		UpdateLifeBonusTracker();
	}

	internal void GotHackKit()
	{
		DataManager.Instance.HackKit = true;
		EscapePlanGUI.UpdateInfoText("Got the hack kit for the door");
	}

	internal void GotAmmo()
	{
		DataManager.Instance.WeaponReadyStatus = true;
	}
	#endregion

	#region Minor State Changes
	internal void ToggleGameState(bool toPAUSE)
	{
		detectINPUT = toPAUSE;
		switch(detectINPUT)
		{
			// Pause the game
		case true:
			detectINPUT = false;
			Time.timeScale = 0.0001f;
			EscapePlanGUI.TogglePauseMenu(true);
			break;
			
			// Un Pause the game.
		case false:
			detectINPUT = true;
			Time.timeScale = 1f;
			EscapePlanGUI.TogglePauseMenu(false);
			break;
		}
		if(OnPauseTheGame != null)
		{
			OnPauseTheGame(detectINPUT);
		}
	}

	internal void OpenDoor()
	{
		if(DataManager.Instance.HackKit)
		{
			LevelTransition(true);
		}
		else 
		{
			EscapePlanGUI.UpdateInfoText("Find a hack kit");
		}
	}

	internal void EnterLevel()
	{
		Destroy (currentSceneInstance);
		LoadLevel (LevelNumber + 1);
	}

	private IEnumerator ResetPlayerToSpawnPoint()
	{
		detectINPUT = false;
		yield return new WaitForSeconds(StaticVariablesContainer.RESPAWN_DELAY);
		if(OnReset != null)
		{
			OnReset();
		}
		MyPlayer.TelePortPlayer(playerSpawnPoint);
		detectINPUT = true;
	}
	
	internal bool addLife()
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

	internal void DeathForPlayer()
	{
		detectINPUT = false;
		NPCManager.Instance.PlayFireAnimation (PlayerPosition);
	//	PlayFireAnimation(PlayerPosition);
		DataManager.Instance.LifeCount--;
		EscapePlanGUI.UpdatePlayerLife();
		if(DataManager.Instance.LifeCount == 0)
		{
			Application.LoadLevel(0);
			DataManager.Instance.HackKit = false;
		}
		else
		{
		//	MyPlayer.PlayDeathAnimation();
			StartCoroutine( MyPlayer.SetPlayerProperties (false));
			StartCoroutine( ResetPlayerToSpawnPoint ());
		}
	}
	internal void ToggleUserControls(bool toENABLE)
	{
		detectINPUT = toENABLE;
	}
	#endregion

	#region Scene transitition

	internal void MoveCameraUp(bool _toUP)
	{
		//CameraController.Instance.MoveCamera(_toUP);
		myCamera.MoveCamera(_toUP);
	}

	private void LoadLevel(int levelNumber)
	{
		currentSceneInstance = (GameObject)Instantiate(Resources.Load("Scenes/Scene_" + levelNumber.ToString()));
		playerSpawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint").GetComponent<Transform>().position;
		DataManager.Instance.HackKit = false;
		DataManager.Instance.CurrentLevelNumber = levelNumber;
		LevelTransition(false);
	}

	private void LevelTransition(bool _hideLEVEL)
	{
		if(_hideLEVEL)
		{
			currentSceneInstance.SetActive(!_hideLEVEL);
			EscapePlanGUI.gameObject.SetActive(!_hideLEVEL);
			TransitionScene.SetActive(_hideLEVEL);
			playerSpawnPoint = StaticVariablesContainer.TRANSITION_SPAWNPOINT;
			myCamera.SetCameraToThisPosition (StaticVariablesContainer.DEFAULT_CAMERA_POSITION);
		}
		else
		{
			TransitionScene.SetActive(_hideLEVEL);
			EscapePlanGUI.gameObject.SetActive(!_hideLEVEL);
			currentSceneInstance.SetActive(!_hideLEVEL);
		}
		MyPlayer.TelePortPlayer(playerSpawnPoint);
	}

	private void UpdateLifeBonusTracker()
	{
		int BonusCounter = DataManager.Instance.BonusTrackerChipCount;
		BonusCounter++;
		if(BonusCounter >= StaticVariablesContainer.BONUS_LIFE_TARGET)
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
