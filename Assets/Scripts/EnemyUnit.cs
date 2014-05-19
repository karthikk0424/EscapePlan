﻿using UnityEngine;
using System.Collections;

public class EnemyUnit : MonoBehaviour 
{
	public WeaponHub WeaponCache;
	public bool isTIMED = false, fireRIGHTSIDE = false, fireATTHEPLAYER = false;
	public float Timer = 3.0f, ForceOnProjectile = 20f;

	private Quaternion directionOfFire;
	private bool isACTIVE;

	private void Start()
	{
		TriggerThisEnemy();
	}
	// Timer


	// Fire


	// Death

	internal void TriggerThisEnemy()
	{
		isACTIVE = true;
		directionOfFire = Quaternion.Euler(0,0,((fireRIGHTSIDE) ? (-90) : (90)));
		if(fireATTHEPLAYER)
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
		//http://answers.unity3d.com/questions/15822/how-to-get-the-positive-or-negative-angle-between.html
		Transform player = GameObject.FindWithTag("Player").GetComponent<Transform>();
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
		if(hit.collider.tag == "PlayerProjectile")
		{
			hit.gameObject.GetComponent<Projectile>().despawnThisProjectile();
			this.gameObject.SetActive(false);
		}
	}

	private void OnDisable()
	{
		isACTIVE = false;
	}
}
