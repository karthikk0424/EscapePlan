using UnityEngine;
using System.Collections;

public class EnemyUnit : MonoBehaviour 
{
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

	private WeaponHub WeaponCache;
	private Quaternion directionOfFire;
	private bool isACTIVE;

	private void Awake()
	{
		switch(FireDirection)
		{
			case DirectionToFire.LeftSide:
				directionOfFire = Quaternion.Euler(0,0,90);
				break;

			case DirectionToFire.RightSide:
				directionOfFire = Quaternion.Euler(0,0,-90);
				break;

			case DirectionToFire.Upwards:
				directionOfFire = Quaternion.Euler(0,0,0);
				break;

			case DirectionToFire.Downwards:
				directionOfFire = Quaternion.Euler(180,0,0);
				break;

			case DirectionToFire.TowardsThePlayer:
				directionOfFire = Quaternion.Euler(0,0,90);	
				break;

			default:
				directionOfFire = Quaternion.Euler(0,0,0);	
				break;
		}
	}

	private void OnEnable()
	{
		if(WeaponCache == null)
		{
			WeaponCache = GameObject.Find(StaticVariablesContainer.WeaponHub).GetComponent<WeaponHub>();
		}
	}

	private void Start()
	{
		if(WeaponCache == null)
		{
			return;
		}
		if(startONAWAKE)
		{
			TriggerThisEnemy();
		}
	}

	internal void TriggerThisEnemy()
	{
		isACTIVE = true;
		if(FireDirection == DirectionToFire.TowardsThePlayer)
		{
			StartCoroutine(this.fireTimedAtTheEnemy());
		}
		else
		{
			StartCoroutine(this.fireTimed());
		}
	}

	private IEnumerator fireTimed()
	{
		do
		{
			yield return new WaitForSeconds(Timer);
			WeaponCache.FireForEnemy(this.transform.position, directionOfFire, ForceOnProjectile);
		}while(isACTIVE);
	}

	private IEnumerator fireTimedAtTheEnemy()
	{
		Transform player = GameObject.FindWithTag(StaticVariablesContainer.MainPlayer).GetComponent<Transform>();
		if(player == null)
		{
			yield break;
		}
		var localTarget = transform.InverseTransformPoint(player.position);
		var targetAngle = Mathf.Atan2(localTarget.x, localTarget.y) * Mathf.Rad2Deg;

		do
		{
			yield return new WaitForSeconds(Timer);
			localTarget = transform.InverseTransformPoint(player.position);
			targetAngle = Mathf.Atan2(localTarget.x, localTarget.y) * Mathf.Rad2Deg;
			directionOfFire = Quaternion.Euler(0,0,-targetAngle);
			WeaponCache.FireForEnemy(this.transform.position, directionOfFire, ForceOnProjectile);
		}while(isACTIVE);
	}

	private void OnCollisionEnter2D(Collision2D hit)
	{
		if(hit.collider.tag == StaticVariablesContainer.PlayerProjectile)
		{
			NPCManager.Instance.PlayFireAnimation (this.transform.position);
			hit.gameObject.GetComponent<Projectile>().despawnThisProjectile();
			this.gameObject.SetActive(false);
		}
	}

	private void OnDisable()
	{
		isACTIVE = false;
	}
}
