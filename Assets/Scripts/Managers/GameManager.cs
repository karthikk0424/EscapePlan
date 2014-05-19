using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
	public PlayerMotion MyPlayer;
	public int LevelNumber = 0;
	public GameObject TransitionScene;
	public GameObject UIEscapPlan, FireAnimation;
	public KeyCode[] AssignedKeys;
	public LevelEnum CurrentPlayerLevel = LevelEnum.Level1;
	public GameGUI EscapePlanGUI;
	private int totalChipsThisScene = 0;
	private GameObject currentSceneInstance;
	private GameObject PlayerSpawnPoint;

	//PAUSE the game - Observer Pattern. 
	public delegate void PauseTheGame(bool toPAUSE);
	public static event PauseTheGame OnPauseTheGame;

	private void Awake()
	{
		instance = this;
		this.transform.name = "_GameManager";
		/*
		 * Fetch user data and start from that level, else load the default level 0;
		 */
		LoadLevel(LevelNumber);

		if((AssignedKeys[0] != null) || (AssignedKeys[0] == KeyCode.None))
		{
			AssignedKeys = new KeyCode[6];
			// Left Movement
			AssignedKeys[0] = KeyCode.LeftArrow;
			
			// Right Movement
			AssignedKeys[1] = KeyCode.RightArrow;
			
			// Jump 
			AssignedKeys[2] = KeyCode.UpArrow;
			
			// Fire a Bullet
			AssignedKeys[3] = KeyCode.Space;

			// Move Elevator UP
			AssignedKeys[4] = KeyCode.W;

			// Move Elevator Down
			AssignedKeys[5] = KeyCode.S;
		}
	}

	private void OnEnable()
	{
		if(MyPlayer == null)
		{
			MyPlayer = GameObject.Find("MainPlayer").GetComponent<PlayerMotion>();
		}
	}
	
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
	internal int TotalChipsCollected
	{
		get
		{
			return totalChipsThisScene;
		}
	}
	#endregion

	// Update is called once per frame
	private void Update () 
	{
		// Left Movement
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
		}

		// Move Elevator
		if(Input.GetKeyDown(AssignedKeys[4])) //DEBUG
		{
			NPCManager.Instance.MoveElevator();
		}
	}

	#region External calls
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
	}

	internal void GotAmmo()
	{
		DataManager.Instance.WeaponReadyStatus = true;
	}

	internal void OpenDoor()
	{
		if(DataManager.Instance.HackKit)
		{
			LevelTransition(true);
		}
	}
	internal void EnterLevel()
	{
		Destroy(currentSceneInstance);
		LoadLevel(LevelNumber + 1);
	}

	internal void PlayFireAnimation(Vector3 _worldCoordinates)
	{
		if(FireAnimation.activeSelf)
		{
			return;
		}
		FireAnimation.transform.position = _worldCoordinates;
		FireAnimation.SetActive(true);
		this.StartCoroutine(playFire());
	}
	
	private IEnumerator playFire()
	{
		yield return new WaitForSeconds(1f);
		FireAnimation.SetActive(false);
	}

	internal void DeathByTrap()
	{
		int lifeCount = DataManager.Instance.LifeCount;
		lifeCount = lifeCount - 1;
		DataManager.Instance.LifeCount = lifeCount;
		EscapePlanGUI.UpdatePlayerLife();
		if(DataManager.Instance.LifeCount == 0)
		{
			//Game Over
		}
		else
		{
			StartCoroutine(ResetPlayerToSpawnPoint());
		}
	}

	private IEnumerator ResetPlayerToSpawnPoint()
	{
		yield return new WaitForSeconds(StaticVariablesContainer.RESPAWN_DELAY);
		MyPlayer.TelePortPlayer(FetchSpawnPoint(PlayerSpawnPoint));
		CameraManager.Instance.ChangeCameraToLevel(StaticVariablesContainer.Level0, true);
	}

	internal void AddLife()
	{
		if(DataManager.Instance.LifeCount < 3)
		{
			DataManager.Instance.LifeCount = DataManager.Instance.LifeCount + 1;
			EscapePlanGUI.UpdatePlayerLife();
		}
	}

	#endregion

	#region Scene transion
	private void LoadLevel(int levelNumber)
	{
		currentSceneInstance = (GameObject)Instantiate(Resources.Load("Scenes/Scene_" + levelNumber.ToString()));
		PlayerSpawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint");
		LevelTransition(false);
	}

	private void LevelTransition(bool hideLevel)
	{
		Vector2 playerPosition = FetchSpawnPoint(PlayerSpawnPoint);
		if(hideLevel)
		{
			currentSceneInstance.SetActive(!hideLevel);
			UIEscapPlan.SetActive(!hideLevel);
			TransitionScene.SetActive(hideLevel);
			playerPosition = StaticVariablesContainer.TRANSITION_SPAWNPOINT;

		}
		else
		{
			TransitionScene.SetActive(hideLevel);
			UIEscapPlan.SetActive(!hideLevel);
			currentSceneInstance.SetActive(!hideLevel);
		}
		MyPlayer.TelePortPlayer(playerPosition);
	}

	private void ToggleScene(bool hide)
	{
		currentSceneInstance.SetActive(hide);
	}

	private void ToggleUI(bool hide)
	{
		UIEscapPlan.SetActive(hide);
	}

	private void ToggleTransitionScene(bool hide)
	{
		TransitionScene.SetActive(hide);
	}

	private void UpdateLifeBonusTracker()
	{
		int BonusCounter = DataManager.Instance.BonusTrackerChipCount;
		BonusCounter = BonusCounter + 1;
		if(BonusCounter == StaticVariablesContainer.CHIP_VALUE)
		{
			AddLife();
			BonusCounter = 0;
		}
		DataManager.Instance.BonusTrackerChipCount = BonusCounter;
	}
	#endregion

	#region Get Information from Scene

	internal Vector2 FetchSpawnPoint(GameObject spawnPoint)
	{
		 return spawnPoint.transform.localPosition;
	}
	#endregion
}
