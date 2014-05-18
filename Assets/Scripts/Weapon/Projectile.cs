using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody2D), typeof (BoxCollider2D))]
public sealed class Projectile : WeaponBase
{

	private Vector2 rigidVelocity = Vector2.zero;
	private GameObject myRecyler;

	internal void propertiesForThisProjectile(GameObject parent)
	{
		rigidVelocity = (Vector2.right * 20 * (Time.deltaTime * 45));
		this.transform.rigidbody2D.velocity = rigidVelocity;
		myRecyler = parent;
	}

	private void OnTriggerEnter2D(Collider2D hit)
	{

	}

	private void OnCollisionEnter2D(Collision2D hit)
	{
		if(hit.collider.tag == "Ground")
		{
			myRecyler.GetComponent<WeaponHub>().DespawnForPlayer(this.gameObject);
		}
	}
}
