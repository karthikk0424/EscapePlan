using UnityEngine;
using System.Collections;

public sealed class DataManager 
{
	#region Singleton created on access
	private static DataManager instance = null;
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
	private int numberOfLife = StaticVariablesContainer.MAX_LIVES;
	private int bonusCounter = 0;
	
	private bool isHackitpicked;
	private bool isAmmoAvailable = false;

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

	#region Weapon ammo
	public bool WeaponReadyStatus
	{
		set
		{
			isAmmoAvailable = value;
		}
		get
		{
			return isAmmoAvailable;
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
