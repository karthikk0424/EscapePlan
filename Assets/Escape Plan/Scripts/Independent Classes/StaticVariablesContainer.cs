using UnityEngine;
using System.Collections;

public sealed class StaticVariablesContainer 
{
	public const int CHIP_VALUE = 1;
	public const int BONUS_LIFE_TARGET = 25;
	public const int MAX_LIVES = 3;
	public const int TRAP_DOOR_LEVEL = 9; 
	public static Vector3 TRANSITION_SPAWNPOINT = new Vector3 (-6.2F, -13.5F);
	public static Vector3 DEFAULT_CAMERA_POSITION = new Vector3 (0, 0 , -10);
	public const string Level0 = "Level0";
	public const string Level1 = "Level1";
	public const string Level2 = "Level2";
	public const string TRAP_LEVEL = "TrapDoor";

	public const float RESPAWN_DELAY = 3.0f; 
	public const string WeaponHub = "WeaponHub";

	#region Tags
	public const string MainPlayer = "Player";
	public const string MainCamera = "MainCamera";
	public const string Manager = "Manager";
	public const string Ground = "Ground";
	public const string Chips = "Chips";
	public const string HackKit = "HackKit";
	public const string Door = "Door";
	public const string EntryDoor = "EntryDoor";
	public const string EnemyProjectile = "EnemyProjectile";
	public const string PlayerProjectile = "PlayerProjectile";
	public const string Enemy = "Enemy";
	public const string Ammo = "Ammo";
	#endregion

}
