using UnityEngine;
using System.Collections;

public sealed class WeaponHub : WeaponBase
{
	public GameObject Projectile;

	private ObjectRecycler weaponCache;
	private GameObject currentObject;

	private uint playerFired = 0;

	private void Start()
	{
		weaponCache = new ObjectRecycler(Projectile, 5, this.gameObject);
	}

	internal void FireForPlayer(Vector3 worldPosition, Quaternion rot, float _force)
	{
		if(playerFired < 3)
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

	internal void FireForEnemy(Vector3 worldPosition, Quaternion rot, float _force)
	{
		currentObject = weaponCache.Spawn(worldPosition, rot);
		currentObject.GetComponent<Projectile>().propertiesForThisProjectile(this.gameObject, false, _force);
	}
}
