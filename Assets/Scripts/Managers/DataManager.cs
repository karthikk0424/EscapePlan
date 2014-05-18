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

	#region Chip Setter/Getters
	public int ChipLootSac
	{
		set
		{
			chips = chips + value;
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
}
