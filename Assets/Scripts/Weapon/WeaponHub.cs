using UnityEngine;
using System.Collections;

public class WeaponHub : WeaponBase
{
	public GameObject Projectile;
	
	private ObjectRecycler weaponCache;
	private GameObject currentObject;

	private uint playerFired = 0;

	private void Start()
	{
		weaponCache = new ObjectRecycler(Projectile, 5, this.gameObject);
	}

	internal void FireForPlayer(Vector3 worldPosition, Quaternion rot)
	{
		if(playerFired < 3)
		{
			currentObject = weaponCache.Spawn(worldPosition, rot);
			currentObject.GetComponent<Projectile>().propertiesForThisProjectile(this.gameObject);
			playerFired++;
		}
	}

	internal void DespawnForPlayer(GameObject _go)
	{
		weaponCache.Despawn(_go);
		playerFired--;
	}



}
