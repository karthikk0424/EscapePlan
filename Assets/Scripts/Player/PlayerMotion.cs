using UnityEngine;
using System.Collections;


public sealed class PlayerMotion : MonoBehaviour 
{
	public Animator PlayerAnimator;
	public WeaponHub WeaponCache;
	private Rigidbody2D PlayerRigidbody;

	private bool isMOVING = false, isJUMPING = false;
	private int currentMotionState = 0;
	private float deltaTime = 0;
	private Vector2 playerVelocity = Vector2.zero;

	private void Awake()
	{
		this.transform.name = "MainPlayer";
		this.transform.tag = "Player";
	}

	private void OnEnable()
	{
		if(WeaponCache == null)
		{
			WeaponCache = GameObject.Find("WeaponHub").GetComponent<WeaponHub>();
		}
		if(PlayerAnimator == null)
		{
			PlayerAnimator = this.GetComponentInChildren<Animator>();
		}
		if(PlayerRigidbody == null)
		{
			PlayerRigidbody = this.GetComponent<Rigidbody2D>();
		}
	}

	private IEnumerator Start()
	{
		while(true)
		{
			deltaTime = (Time.deltaTime * 45f);
			PlayerRigidbody.velocity = playerVelocity;
			yield return null;
		}
	}

	private IEnumerator Jump()
	{
		float lastTime = Time.timeSinceLevelLoad;
		float timer = 0, vel = 0;
		while ((timer < 1) && (isJUMPING))
		{
			vel = Mathf.Lerp(70,0, timer);
			timer += ((Time.timeSinceLevelLoad - lastTime) * 2);
			lastTime = Time.timeSinceLevelLoad;
			playerVelocity = new Vector2(playerVelocity.x, (vel * deltaTime));
			yield return null;
		}
		playerVelocity = new Vector2(playerVelocity.x, 0);
		isJUMPING = false;
	}

	private void setMotion(int state)
	{
		currentMotionState = state;
		PlayerAnimator.SetInteger("MotionState", state);
	}

	private void OnCollisionEnter2D(Collision2D hit) 
	{
		if(hit.collider.tag == "Ground")
		{
			isJUMPING = false;
			if(currentMotionState == 3)
			{
				setMotion(2);
			}
			else if(currentMotionState == -3)
			{
				setMotion(-2);
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D hit)
	{
		switch(hit.collider2D.tag)
		{
			case "Chips":
				if(GameManager.Instance != null)
				{
					GameManager.Instance.GotAChip();
				}
				hit.gameObject.SetActive(false);
				break;

			case "HackKit":
				hit.gameObject.SetActive(false);
				GameManager.Instance.GotHackKit();
				break;

			case "Door":
				GameManager.Instance.OpenDoor();
				break;

			case "DoorEntry":
				GameManager.Instance.EnterLevel();
				break;
			
		}
	}

	internal void InitiateRightTurn()
	{
		if(isMOVING)
		{
			return;
		}
		setMotion(2);
		isMOVING = true;
	}
	
	internal void MoveTowardsRight()
	{
		playerVelocity = new Vector2(25 * deltaTime, playerVelocity.y);
		if((currentMotionState < 0))
		{
			setMotion(2);
		}
	}
	
	internal void StopRightMovement()
	{
		setMotion(1);
		isMOVING = false;
		playerVelocity = Vector2.zero;
	}
	
	internal void InitiateLeftTurn()
	{
		if(isMOVING)
		{ 
			return;
		}
		setMotion(-2);
		isMOVING = true;
	}
	
	internal void MoveTowardsLeft()
	{
		playerVelocity = new Vector2(-25 * deltaTime, playerVelocity.y);
		if(currentMotionState > 0)
		{
			setMotion(-2);
		}
	}
	
	internal void StopLeftMovement()
	{
		setMotion(-1);
		isMOVING = false;
		playerVelocity = Vector2.zero;
	}

	internal void MakeThePlayerToJump()
	{
		if(isJUMPING)
		{
			return;
		}
		isJUMPING = true;
		StartCoroutine(this.Jump());
		if(currentMotionState < 0)
		{
			setMotion(-3);
		}
		else if(currentMotionState > 0)
		{
			setMotion(3);
		}
		else
		{ 
			setMotion(0);
		}
	}

	internal void FireAProjectile()
	{
		WeaponCache.FireForPlayer(this.transform.position, Quaternion.Euler(0,0,-90));
	}
}
