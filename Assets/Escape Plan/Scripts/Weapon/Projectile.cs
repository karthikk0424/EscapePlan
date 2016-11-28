
/// <summary>
/// Projectiles present in the scene uses this class. It used by both the enemy and the player.
/// </summary>

using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody2D), typeof (BoxCollider2D))]
public sealed class Projectile : WeaponBase
{
	#region Variables

	private Vector2 projectileDirection = Vector2.zero;
	private float forceOnTheProjectile = 20;
	private GameObject myRecyler;
	private bool forPLAYER = false;

	#endregion

	#region Projectile Properties

	/// <summary>
	/// Sets the Properties for this projectile.
	/// </summary>
	/// <param name="parent">Parent of this projectile.</param>
	/// <param name="_forPLAYER">If set to <c>true</c> is fired by the player.</param>
	/// <param name="_forceOnProjectile">Force on projectile.</param>
	internal void propertiesForThisProjectile(GameObject _parent, bool _forPLAYER, float _forceOnProjectile)
	{
		myRecyler = _parent;
		forPLAYER = _forPLAYER;
		forceOnTheProjectile = ((_forceOnProjectile > 0) ? (_forceOnProjectile) : (20));
		this.transform.tag = ((_forPLAYER) ? (ConstantVariablesContainer.PlayerProjectile) : (ConstantVariablesContainer.EnemyProjectile));
		projectileDirection = (this.transform.localRotation * new Vector2(0,1));
		AddVelocityToRigidBody ();
	}

	/// <summary>
	/// Adds the force to rigid body.
	/// </summary>
	private void AddForceToRigidBody()
	{
		this.transform.GetComponent<Rigidbody2D>().AddForce(projectileDirection * forceOnTheProjectile * (Time.deltaTime * 45));
	}

	/// <summary>
	/// Adds the velocity to rigid body.
	/// </summary>
	private void AddVelocityToRigidBody()
	{
		//Mass = 0.01; Linear Drag = 0; Angular Drag = 1; Gravity Scale = 0; Fixed Angle = true; is Kinematic = false; 
		this.transform.GetComponent<Rigidbody2D>().velocity = (projectileDirection * (forceOnTheProjectile * (Time.deltaTime * 45)));
	}

	#endregion

	#region Recycling Methods

	/// <summary>
	/// Despawns the this projectile by calling this object recycler.
	/// </summary>
	internal void despawnThisProjectile()
	{
		switch (forPLAYER)
		{
			case true:
				myRecyler.GetComponent<WeaponHub>().DespawnForPlayer (this.gameObject);
				break;
				
			case false:
				myRecyler.GetComponent<WeaponHub>().DespawnForEnemy (this.gameObject);
				break;
		}
	}

	#endregion

	#region Collision Events

	/// <summary>
	/// When collision occurs, this event is called. 
	/// </summary>
	/// <param name="hit"> the info containing about the hit</param>
	private void OnCollisionEnter2D(Collision2D hit)
	{
		if(this.gameObject.activeSelf == false)
		{
			return;
		}

		switch (hit.collider.tag)
		{
            case ConstantVariablesContainer.HackGuard:
                myRecyler.GetComponent<WeaponHub>().DespawnForEnemy(this.gameObject);
                break;
			case ConstantVariablesContainer.Ground:
				despawnThisProjectile();
				break;

			case ConstantVariablesContainer.EnemyProjectile:
				break;


			case ConstantVariablesContainer.PlayerProjectile:
				break;
		}
	}

	#endregion
}
