using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
	public PlayerMotion MyPlayer;
	[Range (1,10)]
	public int LevelNumber = 0;
	public GameObject TransitionScene,UIEscapPlan;
	public GameObject[]  FireAnimation;
	public KeyCode[] AssignedKeys;
	public GameGUI EscapePlanGUI;

	private int totalChipsThisScene = 0;
	private GameObject currentSceneInstance;
	private GameObject PlayerSpawnPoint;
	private bool detectINPUT = false;

	//PAUSE the game - Observer Pattern. 
	public delegate void PauseTheGame(bool toPAUSE);
	public static event PauseTheGame OnPauseTheGame;

	public delegate void ResetTheLevel();
	public static event ResetTheLevel OnReset;

	public delegate void CoinRotation(Quaternion _rot);
	public static event CoinRotation onRotation;

	private Quaternion rotationCoin = Quaternion.identity;

	private void Awake()
	{
		if(instance != null)
		{
			// Duplicate
			Destroy(this.gameObject);
		}

		instance = this;
		this.transform.name = "_GameManager";

		// Fetch user data and start from that level, else load the default level 0;
		LoadLevel(LevelNumber);

		if((AssignedKeys[0] != null) || (AssignedKeys[0] == KeyCode.None))
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
	}

	private void Start()
	{
		detectINPUT = true;
		StartCoroutine(MyPlayer.SetPlayerProperties(true));
	}

	private void OnDestroy()
	{
		instance = null;
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

	// Update is called once per frame
	private void Update () 
	{
		if(detectINPUT)
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

			if(onRotation != null)
			{
				rotationCoin *= Quaternion.Euler (0, 0, (Time.deltaTime * 100));
				onRotation(rotationCoin);	
			}
		}

		if(Input.GetKeyDown(KeyCode.Escape))
		{
			ToggleGameState(detectINPUT);
		}
	}

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
		EscapePlanGUI.UpdateInfoText("Got the hack kit for the door");
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
		else 
		{
			EscapePlanGUI.UpdateInfoText("Need a hack kit");
		}
	}
	internal void EnterLevel()
	{
		Destroy(currentSceneInstance);
		LoadLevel(LevelNumber + 1);
	}

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

	internal void DeathForPlayer()
	{
	//	int lifeCount = DataManager.Instance.LifeCount;
	//	lifeCount = lifeCount - 1;
	//	DataManager.Instance.LifeCount = lifeCount;
		detectINPUT = false;
		PlayFireAnimation(PlayerPosition);
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
			StartCoroutine(MyPlayer.SetPlayerProperties(false));
			StartCoroutine(ResetPlayerToSpawnPoint());
		}
	}

	private IEnumerator ResetPlayerToSpawnPoint()
	{
		detectINPUT = false;
		yield return new WaitForSeconds(StaticVariablesContainer.RESPAWN_DELAY);
		if(OnReset != null)
		{
			OnReset();
		}
		MyPlayer.TelePortPlayer(FetchSpawnPoint(PlayerSpawnPoint));
//		CameraController.Instance.ChangeCameraToLevel(StaticVariablesContainer.Level0, true);
	//	NPCManager.Instance.ResetPlayerLevel();
		detectINPUT = true;
	}

	internal void AddLife()
	{
		if(DataManager.Instance.LifeCount < 3)
		{
			DataManager.Instance.LifeCount = DataManager.Instance.LifeCount + 1;
			EscapePlanGUI.UpdatePlayerLife();
		}
	}

	internal void ToggleUserControls(bool toENABLE)
	{
		detectINPUT = toENABLE;
	}
	#endregion

	#region Scene transion

	internal void MoveCameraUp(bool _toUP)
	{
		CameraController.Instance.MoveCamera(_toUP);
	}
	private void LoadLevel(int levelNumber)
	{
		currentSceneInstance = (GameObject)Instantiate(Resources.Load("Scenes/Scene_" + levelNumber.ToString()));
		PlayerSpawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint");
		DataManager.Instance.HackKit = false;
		LevelTransition(false);
	}

	private void LevelTransition(bool _hideLEVEL)
	{
		Vector2 playerPosition = FetchSpawnPoint(PlayerSpawnPoint);
		if(_hideLEVEL)
		{
			currentSceneInstance.SetActive(!_hideLEVEL);
			UIEscapPlan.SetActive(!_hideLEVEL);
			TransitionScene.SetActive(_hideLEVEL);
			playerPosition = StaticVariablesContainer.TRANSITION_SPAWNPOINT;
		}
		else
		{
			TransitionScene.SetActive(_hideLEVEL);
			UIEscapPlan.SetActive(!_hideLEVEL);
			currentSceneInstance.SetActive(!_hideLEVEL);
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
		if(BonusCounter == StaticVariablesContainer.BONUS_LIFE_TARGET)
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
