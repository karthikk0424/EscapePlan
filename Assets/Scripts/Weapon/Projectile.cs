using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody2D), typeof (BoxCollider2D))]
public sealed class Projectile : WeaponBase
{
	private Vector2 projectileDirection = Vector2.zero;
	private float forceOnTheProjectile = 20;
	private GameObject myRecyler;

	internal void propertiesForThisProjectile(GameObject parent)
	{
		myRecyler = parent;
		projectileDirection = (this.transform.localRotation * new Vector2(0,1));
		AddVelocityToRigidBody();
	}

	private void AddForceToRigidBody()
	{
		this.transform.rigidbody2D.AddForce(projectileDirection * forceOnTheProjectile * (Time.deltaTime * 45));
	}

	private void AddVelocityToRigidBody()
	{
		//Mass = 0.01; Linear Drag = 0; Angular Drag = 1; Gravity Scale = 0; Fixed Angle = true; is Kinematic = false; 
		this.transform.rigidbody2D.velocity = (projectileDirection * (forceOnTheProjectile * (Time.deltaTime * 45)));
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
