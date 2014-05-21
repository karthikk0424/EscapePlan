
/// <summary>
/// Weapon hub that possess all the projectiles. 
/// </summary>

using UnityEngine;
using System.Collections;

public sealed class WeaponHub : WeaponBase
{
	#region Variables

	public GameObject Projectile;
	public int TotalProjectilesRequired = 10;
	private ObjectRecycler weaponCache;
	private GameObject currentObject;
	private uint playerFired = 0;

	#endregion

	#region MonoBehaviour - start & end methods

	/// <summary>
	/// When this game object is enabled
	/// </summary>
	private void Start()
	{
		weaponCache = new ObjectRecycler(Projectile, Mathf.Abs(TotalProjectilesRequired), this.gameObject);
	}

	/// <summary>
	/// Called when this game object is destroyed. Cleans up for the garbage collector.
	/// </summary>
	private void OnDestoy()
	{
		weaponCache = null;
		Projectile = null;
		currentObject = null;
	}

	#endregion

	#region Firing Projectile methods

	/// <summary>
	/// Fires a projectile for player.
	/// </summary>
	/// <param name="_worldPosition">World position to be fired at.</param>
	/// <param name="_rot">Rotation of the projectile.</param>
	/// <param name="_force">Force on the projectile.</param>
	internal void FireForPlayer(Vector3 _worldPosition, Quaternion _rot, float _force)
	{
		if(playerFired < 1)
		{
			currentObject = weaponCache.Spawn(_worldPosition, _rot);
			currentObject.GetComponent<Projectile>().propertiesForThisProjectile(this.gameObject, true, _force);
			playerFired++;
		}
	}

	/// <summary>
	/// Fires a projectile for the enemy.
	/// </summary>
	/// <param name="_worldPosition">World position.</param>
	/// <param name="_rot">Rot.</param>
	/// <param name="_force">_force.</param>
	internal void FireForEnemy(Vector3 _worldPosition, Quaternion _rot, float _force)
	{
		currentObject = weaponCache.Spawn(_worldPosition, _rot);
		currentObject.GetComponent<Projectile>().propertiesForThisProjectile(this.gameObject, false, _force);
	}

	#endregion

	#region Despawning Methods

	/// <summary>
	/// Despawns the game object for the player
	/// </summary>
	/// <param name="_go"> The game object</param>
	internal void DespawnForPlayer(GameObject _go)
	{
		weaponCache.Despawn(_go);
		playerFired--;
	}

	/// <summary>
	/// Despawns the projectile for enemy.
	/// </summary>
	/// <param name="_go"> the game object.</param>
	internal void DespawnForEnemy(GameObject _go)
	{
		weaponCache.Despawn(_go);
	}

	#endregion
}
