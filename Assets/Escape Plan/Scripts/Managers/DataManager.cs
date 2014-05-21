
/// <remarks>
/// Developed for Big Viking Games, London, Canada.
/// </remarks>
/// <summary>
/// Holds all the data pertinent to the game.
/// </summary>
/// <description>
/// It holds data that might be required throughout the game. This classes has the potential
/// to expand on a larger scale - if more data transaction is involved. 
/// </description>

using UnityEngine;
using System.Collections;

public sealed class DataManager 
{
	#region Singleton created to be accessed anywhere.
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

	#region Variables

	private int chips;
	private int numberOfLife = ConstantVariablesContainer.MAX_LIVES;
	private int bonusCounter = 0;
	private int currentLevel = 1;
	private bool isHackitpicked;
	private bool isAmmoAvailable = false;

	#endregion

	#region Getters & Setters
	
	/// <summary>
	/// Gets or sets the count for chips that been collection. 
	/// </summary>
	/// <value>The count on chips collected.</value>
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

	/// <summary>
	/// Gets or sets the boolean which is responsible for moving to the next level.
	/// </summary>
	/// <value><c>true</c> if hack kit has been obtained; otherwise, <c>false</c>.</value>
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
	
	/// <summary>
	/// Gets or sets boolean on whether the player has ammo to fire. 
	/// </summary>
	/// <value><c>true</c> if the weapon is ready; otherwise, <c>false</c>.</value>
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
	
	/// <summary>
	/// Gets or sets the life count. The count on remaining lives
	/// </summary>
	/// <value>The life count.</value>
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
	
	/// <summary>
	/// Gets or sets the bonus chip count. With this count, the life could be extended. 
	/// </summary>
	/// <value>The bonus chips count.</value>
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

	/// <summary>
	/// Gets or sets the current level number.
	/// </summary>
	/// <value>The level that is being played.</value>
	public int CurrentLevelNumber
	{
		set
		{
			currentLevel = value;
		}
		get
		{
			return currentLevel;
		}

	}
	#endregion
}
