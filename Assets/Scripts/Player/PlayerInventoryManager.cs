using UnityEngine;
using System.Collections;

public sealed class PlayerInventoryManager 
{
	#region Singleton created on access
	private static PlayerInventoryManager instance = null;
	private PlayerInventoryManager() {}

	public static PlayerInventoryManager Instance
	{
		get
		{
			if(instance == null)
			{
				instance = new PlayerInventoryManager();
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
