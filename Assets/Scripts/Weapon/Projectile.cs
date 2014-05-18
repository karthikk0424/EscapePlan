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
		rigidVelocity = (Vector2.right * 20 * (Time.deltaTime * 45));
		this.transform.rigidbody2D.velocity = rigidVelocity;
		//StartCoroutine(this.moveProjectile());
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

	private void OnTriggerEnter2D(Collider2D hit)
	{

	}

	private void OnCollisionEnter2D(Collision2D hit)
	{
		if(hit.collider.tag == "Ground")
		{
			base.Despawn(this.gameObject);
		}
	}
}
