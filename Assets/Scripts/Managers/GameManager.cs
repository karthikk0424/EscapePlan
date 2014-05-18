using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
	public PlayerMotion MyPlayer;
	public int LevelNumber;

	public KeyCode[] AssignedKeys;

	private int totalChipsThisScene = 0;
	private GameObject currentSceneInstance;

	private void Awake()
	{
		instance = this;
		this.transform.name = "_GameManager";

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
	
	#region Singleton
	private static GameManager instance;
	public static GameManager Instance
	{
		get{
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
			MyPlayer.FireAProjectile();
		}
	}
	private void LoadLevel(int levelNumber)
	{
		currentSceneInstance = (GameObject)Instantiate(Resources.Load("Scenes/Scene_" + levelNumber.ToString()));
	}

	internal void GotAChip()
	{
		DataManager.Instance.ChipLootSac = StaticVariablesContainer.CHIP_VALUE;
		//totalChipsThisScene++;
		//Debug.Log("Chips this scene = " + totalChipsThisScene);
	}

	internal void GotHackKit()
	{
		DataManager.Instance.HackKit = true;
	}

	internal void OpenDoor()
	{
		if(DataManager.Instance.HackKit)
		{
			Debug.Log("Level Complete");
		}
	}
	internal void EnterLevel()
	{

	}
}
