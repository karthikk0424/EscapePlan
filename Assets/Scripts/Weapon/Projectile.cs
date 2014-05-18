using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody2D), typeof (BoxCollider2D))]
public sealed class Projectile : WeaponBase
{
	/*  Required Properties
		1. Enemy/Player Projectile
	 */
	private bool isTRAVELLING = false;
	private Vector2 rigidVelocity = Vector2.zero;
	private float deltaTime = 0f;

	internal void propertiesForThisProjectile()
	{
		isTRAVELLING = true;
		Debug.Log(this.transform.forward);
		rigidVelocity = (this.transform.forward * 200);
		StartCoroutine(this.moveProjectile());
	}

	private IEnumerator moveProjectile()
	{
		while(isTRAVELLING)
		{
			rigidVelocity = (this.transform.forward * 200);// * Time.deltaTime * 45);
			this.rigidbody2D.velocity = (rigidVelocity);
		//	Debug.Log(rigidVelocity);
			yield return null;
		}
	}
}
