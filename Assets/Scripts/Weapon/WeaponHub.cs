using UnityEngine;
using System.Collections;

public class WeaponHub : WeaponBase
{
	public GameObject Projectile;
	
	private ObjectRecycler weaponCache;
	private GameObject currentObject;

	private void Start()
	{
//		base.PoolThisObject(Projectile, 5, this.gameObject);
		weaponCache = new ObjectRecycler(Projectile, 5, this.gameObject);
	}

	internal void FireAProjectile(Vector3 worldPosition, Quaternion rot)
	{
	//	base.FireAProjectile(worldPosition, rot);

		currentObject = weaponCache.Spawn(worldPosition, rot);
		currentObject.GetComponent<Projectile>().propertiesForThisProjectile(this.gameObject);
	}

	internal void DespawnMe(GameObject _go)
	{
		weaponCache.Despawn(_go);
	}
	
}
