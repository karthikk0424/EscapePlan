using UnityEngine;
using System.Collections;

public sealed class WeaponHub : WeaponBase
{
	public GameObject Projectile;
	
	public int TotalProjectilesRequired = 10;
	private ObjectRecycler weaponCache;
	private GameObject currentObject;

	private uint playerFired = 0;

	private void Start()
	{
		weaponCache = new ObjectRecycler(Projectile, Mathf.Abs(TotalProjectilesRequired), this.gameObject);
	}

	internal void FireForPlayer(Vector3 worldPosition, Quaternion rot, float _force)
	{
		if(playerFired < 1)
		{
			currentObject = weaponCache.Spawn(worldPosition, rot);
			currentObject.GetComponent<Projectile>().propertiesForThisProjectile(this.gameObject, true, _force);
			playerFired++;
		}
	}

	internal void DespawnForPlayer(GameObject _go)
	{
		weaponCache.Despawn(_go);
		playerFired--;
	}

	internal void DespawnForEnemy(GameObject _go)
	{
		weaponCache.Despawn(_go);
	}

	internal void FireForEnemy(Vector3 worldPosition, Quaternion rot, float _force)
	{
		currentObject = weaponCache.Spawn(worldPosition, rot);
		currentObject.GetComponent<Projectile>().propertiesForThisProjectile(this.gameObject, false, _force);
	}
}
