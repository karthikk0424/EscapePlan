using UnityEngine;
using System.Collections;

public class WeaponBase : MonoBehaviour
{
	private ObjectRecycler weaponRecyler;
	private GameObject currentGo;


	protected void PoolThisObject(GameObject go, int count, GameObject parent)
	{
		weaponRecyler = new ObjectRecycler(go, count, parent);
	}

	protected void FireAProjectile(Vector3 worldPosition, Quaternion rot)
	{
		currentGo = weaponRecyler.Spawn(worldPosition,rot);
		currentGo.GetComponent<Projectile>().propertiesForThisProjectile();
	}
}
