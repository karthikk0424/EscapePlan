using UnityEngine;
using System.Collections;

public class EnemyUnit : MonoBehaviour 
{
	public WeaponHub WeaponCache;
	public bool isTIMED = false;
	public float Timer = 3.0f;
	public float ForceOnProjectile = 20;
	public bool fireRIGHTSIDE = false;

	private Quaternion directionOfFire;

	private void Start()
	{
		directionOfFire = Quaternion.Euler(0,0,((fireRIGHTSIDE) ? (-90) : (90)));
		TriggerThisEnemy();
	}
	// Timer


	// Fire


	// Death

	internal void TriggerThisEnemy()
	{
		StartCoroutine(this.fireTimed());
	}

	private void OnCollisionEnter2D(Collision2D hit)
	{
		if(hit.collider.tag == "PlayerProjectile")
		{
			hit.gameObject.GetComponent<Projectile>().despawnThisProjectile();
			this.gameObject.SetActive(false);
		}
	}


	private IEnumerator fireTimed()
	{
		do
		{
			yield return new WaitForSeconds(Timer);
			WeaponCache.FireForEnemy(this.transform.position, directionOfFire, ForceOnProjectile);
		}while(true);
	}

}
