using UnityEngine;
using System.Collections;

public sealed class DataManager 
{
	#region Singleton created on access
	private static DataManager instance = null;
	private DataManager() {}

	public static DataManager Instance
	{
		get
		{
			if(instance == null)
			{
				instance = new DataManager();
			}
			return instance;
		}
	}
	#endregion

	private int chips;
	private bool isHackitpicked;
	private int numberOfLife = StaticVariablesContainer.MAX_LIVES;
	private int bonusCounter = 0;
	#region Chip Setter/Getters
	public int ChipLootSac
	{
		set
		{
			chips = value;
		}
		get
		{
			return chips;
		}
	}
	#endregion

	#region Chip HackKit
	public bool HackKit
	{
		set
		{
			isHackitpicked = value;
		}
		get
		{
			return isHackitpicked;
		}
	}
	#endregion

	#region Player Life
	public int LifeCount
	{
		set
		{
			numberOfLife = value;
		}

		get
		{
			return numberOfLife;
		}
	}
	#endregion

	#region Player Life
	public int BonusTrackerChipCount
	{
		set
		{
			bonusCounter = value;
		}
		
		get
		{
			return bonusCounter;
		}
	}
	#endregion
}
