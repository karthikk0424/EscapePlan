
/// <summary>
/// The final boss present in the last level.
/// </summary>
/// <remarks>
/// This class could have been merged with the enemy units. But due to the agile 
/// nature of the project, had to make a seperate class. Might be something to consider 
/// in the future to merge. 
/// </remarks>

using UnityEngine;
using System.Collections;

public class FinalBoss : MonoBehaviour 
{
	#region Variables

	public enum BossUnit
	{
		Guard
	}
	public BossUnit ThisBoss;
	
	public float ForceOnProjectile = 20;
	public WeaponHub WeaponCache; 
	public float position = 0f;
	public float radius = 10f;


	// For Guard
	private bool canFIRE;
	private Vector3 startPosition;

	#endregion

	#region Monobehaviour methods

	/// <summary>
	/// When this game object is being enabled. Caches all the necessary objects. 
	/// </summary>
	private void OnEnable()
	{
		if(WeaponCache == null)
		{
			WeaponCache = GameObject.Find (ConstantVariablesContainer.WeaponHub).GetComponent<WeaponHub>();
		}
		if(WeaponCache == null)
		{
			Debug.Log("<color=red>ATTENTION </color> : No weapon cache assigned");
		}
	}
	
	/// <summary>
	/// When this game object is initialized. It is called after OnEnable method.
	/// </summary>
	private void Start () 
	{
		switch(ThisBoss)
		{
			case BossUnit.Guard:
			canFIRE = true;
			startPosition = this.transform.position;
			StartCoroutine (this.fireProjectile());
			break;
		}
	}

	/// <summary>
	/// Called when the game object is being deleted. 
	/// </summary>
	/// <description>
	/// Derefencing reference types for the Garbage Collector to pick-up.
	/// </description>
	private void OnDestroy()
	{
		WeaponCache = null;
	}
	
	/// <summary>
	/// Called during every frame - it is a monobehaviour method.
	/// </summary>
	void Update()
	{
		switch (ThisBoss)
		{
		case BossUnit.Guard:
			position += Time.deltaTime * 1.5f; // 1.0f is the rotation speed in radians per sec.
			Vector3 pos = new Vector3 (Mathf.Sin(position), Mathf.Cos(position), 0);
			transform.position = startPosition - pos * (2 * radius);
			break;
		}
	}

	#endregion
	
	#region Collision Methods

	/// <summary>
	/// When it collides with the player. 
	/// </summary>
	/// <param name="hit">The info containing infomation about the hit with the player.</param>
	private void OnCollisionEnter2D(Collision2D hit) 
	{
		if(hit.gameObject.CompareTag (ConstantVariablesContainer.PlayerProjectile))
		{
			switch (ThisBoss)
			{
				case BossUnit.Guard:
					hit.gameObject.GetComponent<Projectile>().despawnThisProjectile ();
					StartCoroutine( reSpawnGuard ());
					break;
			}
		}
	}

	#endregion

	#region Methods oriented toward behaviour of the boss

	/// <summary>
	/// Time taken to respawn after the enemy is killed. 
	/// </summary>
	/// <returns>The spawn guard.</returns>
	private IEnumerator reSpawnGuard()
	{
		canFIRE = false;
		this.GetComponent<BoxCollider2D>().enabled = false;
		this.GetComponent<Renderer>().enabled = false;
		yield return new WaitForSeconds(2f);
		// enable fire again. 
		this.GetComponent<Renderer>().enabled = true;
		this.GetComponent<BoxCollider2D>().enabled = true;
		canFIRE = true;

	}

	/// <summary>
	/// When a projectile is to be fired by the boss. 
	/// </summary>
	/// <returns>waiting time to fire a projectile..</returns>
	private IEnumerator fireProjectile()
	{
		if(GameManager.Instance == null)
		{
			yield break;
		}
		var localTarget = transform.InverseTransformPoint (GameManager.Instance.PlayerPosition);
		var targetAngle = Mathf.Atan2 (localTarget.x, localTarget.y) * Mathf.Rad2Deg;
		Quaternion directionOfFire = Quaternion.identity;
		while (this.gameObject.activeSelf)
		{
			float timer = Random.Range(1f, 2f);
			yield return new WaitForSeconds(timer);
			timer = Random.Range(1f,5f);
			if(DataManager.Instance.HackKit)
			{
				canFIRE = false;
				this.GetComponent<BoxCollider2D>().enabled = false;
				this.GetComponent<Renderer>().enabled = false;
				NPCManager.Instance.PlayFireAnimation (this.transform.position);
				yield break;
			}
			if(canFIRE)
			{
				// Fire at the player			
				localTarget = transform.InverseTransformPoint (GameManager.Instance.PlayerPosition);
				targetAngle = Mathf.Atan2 (localTarget.x, localTarget.y) * Mathf.Rad2Deg;
				directionOfFire = Quaternion.Euler (0,0,(90-targetAngle));
				WeaponCache.FireForEnemy (this.transform.position, directionOfFire, ForceOnProjectile);
			}
		}
	}

	#endregion
}
