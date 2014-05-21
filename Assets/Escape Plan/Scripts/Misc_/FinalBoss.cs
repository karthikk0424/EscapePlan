using UnityEngine;
using System.Collections;

public class FinalBoss : MonoBehaviour 
{
	public enum BossUnit
	{
		Guard
	}
	public BossUnit ThisBoss;
	
	public float ForceOnProjectile = 20;
	public WeaponHub WeaponCache; 

	// For Guard
	private bool canFIRE;
	public float position = 0f;
	public float radius = 10f;
	private Vector3 startPosition;

	// Use this for initialization
	void Start () 
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
				position += Time.deltaTime * 1.5f; // 1.0f is the rotation speed in radians per sec.
				Vector3 pos = new Vector3(Mathf.Sin(position), Mathf.Cos(position), 0);
				transform.position = startPosition - pos * (2 * radius);
				break;
		}
	}

	private void OnCollisionEnter2D(Collision2D hit) 
	{
		if(hit.gameObject.CompareTag(StaticVariablesContainer.PlayerProjectile))
		{
			switch (ThisBoss)
			{
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
			float timer = Random.Range(1f, 3f);
			yield return new WaitForSeconds(timer);
			timer = Random.Range(1f,3f);
			if(DataManager.Instance.HackKit)
			{
				canFIRE = false;
				this.collider.enabled = false;
				this.renderer.enabled = false;
				StartCoroutine( this.loadFinalScene());
				yield break;
			}
			if(canFIRE)
			{
				// Fire at the player			
				localTarget = transform.InverseTransformPoint(GameManager.Instance.PlayerPosition);
				targetAngle = Mathf.Atan2(localTarget.x, localTarget.y) * Mathf.Rad2Deg;
				directionOfFire = Quaternion.Euler(0,0,(90-targetAngle));
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
