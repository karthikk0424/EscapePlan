using UnityEngine;
using System.Collections;

public class WeaponHub : WeaponBase
{
	public GameObject Projectile;
	

	private void Start()
	{
		base.PoolThisObject(Projectile, 5, this.gameObject);
	}

	internal void FireAProjectile(Vector3 worldPosition, Quaternion rot)
	{
		base.FireAProjectile(worldPosition, rot);
	}

	private void Update()
	{
		base.DebugRecylcer();
	}
}
