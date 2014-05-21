
/// <summary>
/// Responsible for the enemies present in the screen. 
/// </summary>

using UnityEngine;
using System.Collections;

public class EnemyUnit : MonoBehaviour 
{
	#region public variables 

	public bool isTIMED = false, startONAWAKE = true;
	public float Timer = 3.0f, ForceOnProjectile = 20f;

	public enum DirectionToFire
	{
		LeftSide, 
		RightSide, 
		Upwards, 
		Downwards,
		TowardsThePlayer
	}
	public DirectionToFire FireDirection;

	#endregion

	#region Private Variables

	private WeaponHub WeaponCache;
	private Quaternion directionOfFire;
	private bool isACTIVE;

	#endregion

	#region Monobehaviour - start and end events

	/// <summary>
	/// Called when this gameobject is being instantiated. 
	/// </summary>
	/// <description>
	/// Sets the values for angle of firing the projectile
	/// </description>
	private void Awake()
	{
		switch(FireDirection)
		{
			case DirectionToFire.LeftSide:
				directionOfFire = Quaternion.Euler (0,0,90);
				break;

			case DirectionToFire.RightSide:
				directionOfFire = Quaternion.Euler (0,0,-90);
				break;

			case DirectionToFire.Upwards:
				directionOfFire = Quaternion.Euler (0,0,0);
				break;

			case DirectionToFire.Downwards:
				directionOfFire = Quaternion.Euler (180,0,0);
				break;

			case DirectionToFire.TowardsThePlayer:
				directionOfFire = Quaternion.Euler (0,0,90);	
				break;

			default:
				directionOfFire = Quaternion.Euler (0,0,0);	
				break;
		}
	}

	/// <summary>
	/// When this game object is activated. Caches a reference variable.
	/// </summary>
	private void OnEnable()
	{
		if(WeaponCache == null)
		{
			WeaponCache = GameObject.Find (ConstantVariablesContainer.WeaponHub).GetComponent<WeaponHub>();
		}
	}

	/// <summary>
	/// Called after OnEnable when this GameObject is instantiated.
	/// </summary>
	private void Start()
	{
		if(WeaponCache == null)
		{
			return;
		}
		if(startONAWAKE)
		{
			triggerThisEnemy ();
		}
	}

	/// <summary>
	/// When this unit is destroyed. Derefences the reference types for garbage collection.
	/// </summary>
	private void OnDestroy()
	{
		WeaponCache = null;
	}

	/// <summary>
	/// When this game object is disabled.
	/// </summary>
	private void OnDisable()
	{
		isACTIVE = false;
	}

	#endregion

	#region Collision & Trigger events

	/// <summary>
	/// When you want this enemy to trigger a projectile.
	/// </summary>
	private void triggerThisEnemy()
	{
		isACTIVE = true;
		if(FireDirection == DirectionToFire.TowardsThePlayer)
		{
			StartCoroutine (this.fireTimedAtTheEnemy());
		}
		else
		{
			StartCoroutine (this.fireTimed());
		}
	}

	/// <summary>
	/// When it collides with the player. 
	/// </summary>
	/// <param name="hit">the info about the hit.</param>
	private void OnCollisionEnter2D(Collision2D hit)
	{
		if(hit.collider.tag == ConstantVariablesContainer.PlayerProjectile)
		{
			NPCManager.Instance.PlayFireAnimation (this.transform.position);
			hit.gameObject.GetComponent<Projectile>().despawnThisProjectile();
			this.gameObject.SetActive (false);
		}
	}

	#endregion

	#region Projectile Firing Routines

	/// <summary>
	/// Fires timely till this gameobject is active.
	/// </summary>
	/// <returns>yield based on the timer variable.</returns>
	private IEnumerator fireTimed()
	{
		do
		{
			yield return new WaitForSeconds (Timer);
			WeaponCache.FireForEnemy (this.transform.position, directionOfFire, ForceOnProjectile);
		}while (isACTIVE);
	}

	/// <summary>
	/// Fires directly at the enemy.
	/// </summary>
	/// <returns>yield for a timers till the next fire</returns>
	private IEnumerator fireTimedAtTheEnemy()
	{
		Transform player = GameObject.FindWithTag (ConstantVariablesContainer.MainPlayer).GetComponent<Transform>();
		if(player == null)
		{
			yield break;
		}
		var localTarget = transform.InverseTransformPoint(player.position);
		var targetAngle = Mathf.Atan2 (localTarget.x, localTarget.y) * Mathf.Rad2Deg;

		do
		{
			yield return new WaitForSeconds (Timer);
			localTarget = transform.InverseTransformPoint (player.position);
			targetAngle = Mathf.Atan2 (localTarget.x, localTarget.y) * Mathf.Rad2Deg;
			directionOfFire = Quaternion.Euler (0,0,-targetAngle);
			WeaponCache.FireForEnemy (this.transform.position, directionOfFire, ForceOnProjectile);
		}while(isACTIVE);
	}

	#endregion
}
