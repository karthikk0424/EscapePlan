using UnityEngine;
using System.Collections;

public class FinalBoss : MonoBehaviour 
{
	public enum BossUnit
	{
		Guard, 
		MainBoss
	}
	public BossUnit ThisBoss;

	private static int bossHitCount = 10;
	public float ForceOnProjectile = 20;
	public WeaponHub WeaponCache; 

	// For Guard
	private bool canFIRE;
	private Quaternion rotation;
	private Vector3 radius = new Vector3(4, 0, 0);
	private float currentRotation = 0f;

	// Use this for initialization
	void Start () 
	{
		switch(ThisBoss)
		{
			case BossUnit.Guard:
			canFIRE = true;
			StartCoroutine (this.fireProjectile());
			break;
		}
	}

	private void OnEnable()
	{
		if(WeaponCache == null)
		{
			WeaponCache = GameObject.Find(StaticVariablesContainer.WeaponHub).GetComponent<WeaponHub>();
		}
		if(WeaponCache == null)
		{
			Debug.Log("<color=red>ATTENTION </color> : No weapon cache assigned");
		}
	}

	// Update is called once per frame
	void Update ()
	{
		switch(ThisBoss)
		{
			case BossUnit.Guard:
				currentRotation += (Time.deltaTime * 50);
				rotation = Quaternion.Euler(0, 0, currentRotation);
				transform.position = (rotation * radius);
				break;
		}
	}

	private void OnCollisionEnter2D(Collision2D hit) 
	{
		if(hit.gameObject.CompareTag(StaticVariablesContainer.PlayerProjectile))
		{
			switch (ThisBoss)
			{
				case BossUnit.MainBoss:
					if(bossHitCount > 0)
					{
						bossHitCount--;
					}
					else
					{
						StartCoroutine(this.loadFinalScene());
					}
					break;
				case BossUnit.Guard:
					StartCoroutine( reSpawnGuard());
					break;
			}
		}
	}

	private IEnumerator reSpawnGuard()
	{
		canFIRE = false;
		this.collider.enabled = false;
		this.renderer.enabled = false;
		yield return new WaitForSeconds(2f);
		// enable fire again. 
		this.renderer.enabled = true;
		this.collider.enabled = true;
		canFIRE = true;

	}

	private IEnumerator fireProjectile()
	{
		if(GameManager.Instance == null)
		{
			yield break;
		}
		var localTarget = transform.InverseTransformPoint(GameManager.Instance.PlayerPosition);
		var targetAngle = Mathf.Atan2(localTarget.x, localTarget.y) * Mathf.Rad2Deg;
		Quaternion directionOfFire = Quaternion.identity;
		while(this.gameObject.activeSelf)
		{
			float timer = Random.Range(1f, 4f);
			yield return new WaitForSeconds(timer);
			timer = Random.Range(1f,3f);
			if(canFIRE)
			{
				// Fire at the player			
				localTarget = transform.InverseTransformPoint(GameManager.Instance.PlayerPosition);
				targetAngle = Mathf.Atan2(localTarget.x, localTarget.y) * Mathf.Rad2Deg;
				directionOfFire = Quaternion.Euler(0,0,-targetAngle);
				WeaponCache.FireForEnemy(this.transform.position, directionOfFire, ForceOnProjectile);
			}
		}
	}

	private IEnumerator loadFinalScene()
	{

		// load the final doubel decision scene. 
		yield return null;
	}
}
