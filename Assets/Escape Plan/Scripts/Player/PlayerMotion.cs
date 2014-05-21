/// <remarks>
/// Developed for Big Viking Games, London, Canada.
/// </remarks>
/// <summary>
/// Class that controls the Player. It has modules for controlling the player
/// which have to be called from another class which would detect the input based upon the platforms.
/// It also has modules for Animating the sprites. Inherits MonoBehaviour to utilize Unity's resources. 
/// </summary>
/// /// <description>
/// Rigidbody2D parameters : Mass = 50; Linear Drag = 0; Angular Drag = 0; Gravity Scale = 50; Fixed Angle = true;
/// isKinematic = false; Interpolate = non; Sleeping Mode = Start Asleep; Collision Detection = Discrete
/// </description>

using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody2D), typeof (BoxCollider2D))]
public sealed class PlayerMotion : MonoBehaviour 
{
	#region Variables
	public Animator PlayerAnimator;
	public WeaponHub WeaponCache;
	public float ForceOnProjectile = 20.0f;
	private Rigidbody2D PlayerRigidbody;
	private BoxCollider2D myCollider;

	private bool isMOVING = false, isJUMPING = false;
	private int currentMotionState = 0;
	private float deltaTime = 0;
	private Vector2 playerVelocity = Vector2.zero;
	#endregion
	
	#region MonoBehaviour Methods - Start & End
	private void Awake()
	{
		this.transform.name = "MainPlayer";
		this.transform.tag = StaticVariablesContainer.MainPlayer;
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
		if(myCollider == null)
		{
			myCollider = this.GetComponent<BoxCollider2D>();
		}
		GameManager.OnReset += HandleOnReset;
	}

	private void HandleOnReset ()
	{
		StartCoroutine( this.SetPlayerProperties (true));
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

	private void OnDisable()
	{
		GameManager.OnReset -= HandleOnReset;
	}

	internal IEnumerator SetPlayerProperties(bool toACTIVATE)
	{
		setAnimation(0);
		playerVelocity = Vector2.zero;
		this.rigidbody2D.isKinematic = (!toACTIVATE);
		this.GetComponent<BoxCollider2D>().enabled = toACTIVATE;
		PlayerAnimator.SetBool("isALIVE", toACTIVATE);
		yield return new WaitForSeconds(0.2f);
		this.GetComponentInChildren<SpriteRenderer>().enabled = toACTIVATE;
	}
	#endregion

	#region Animation Regions

	/// <summary>
	/// The plays the appropriate animation for the main player
	/// </summary>
	/// <param name="state"> 
	/// <listheader> Different Animation States</listheader>
	/// <list type="0 = Neutral Idle Animation">
	/// <list type="-1/1 = Left/Right Idle Animation">
	/// <list type="-2/2 = Left/Right Running Animation">
	/// <list type="-3/3 = Left/Right Jump Animation">
	/// </param>
	/// <description>
	/// The Artist/Designer could just make animations and they could
	/// clearly view the animation by just playing through the levels. 
	/// </description>
	private void setAnimation(int state)
	{
		currentMotionState = state;
		PlayerAnimator.SetInteger("MotionState", state);
	}
	#endregion
	
	#region Methods associated with Collider2D

	/// <summary>
	/// On Collision with static or dynamic colliders, this event is raised. 
	/// </summary>
	/// <param name="hit"> Contains the info about the collision with the collider</param>
	/// <description>
	/// The colliders would be <list type="Ground"> <list type="enter the enemy tag">
	/// </description>
	private void OnCollisionEnter2D(Collision2D hit) 
	{
		switch (hit.collider.tag)
		{
			case StaticVariablesContainer.Ground:
				isJUMPING = false;
				if(currentMotionState == 3)
				{
					setAnimation((isMOVING) ? (2) : (1));
				}
				else if(currentMotionState == -3)
				{
					setAnimation((isMOVING) ? (-2) : (-1));
				}
				break;

			case StaticVariablesContainer.EnemyProjectile:
				hit.transform.GetComponent<Projectile>().despawnThisProjectile();
				GameManager.Instance.DeathForPlayer();
				break;

			case StaticVariablesContainer.Enemy:
				GameManager.Instance.DeathForPlayer();
				break;
		}
	}

	/// <summary>
	/// On Collision with static or dynamic triggers, this event is raised. 
	/// </summary>
	/// <param name="hit">Contains info about the trigger/collider</param>
	private void OnTriggerEnter2D(Collider2D hit)
	{
		switch(hit.collider2D.tag)
		{
			case StaticVariablesContainer.Chips:
				GameManager.Instance.GotAChip();
				hit.gameObject.SetActive(false);
				break;

			case StaticVariablesContainer.HackKit:
				hit.gameObject.SetActive(false);
				GameManager.Instance.GotHackKit();
				break;

			case StaticVariablesContainer.Door:
				GameManager.Instance.OpenDoor();
				break;

			case StaticVariablesContainer.EntryDoor:
				GameManager.Instance.EnterLevel();
				break;

			case StaticVariablesContainer.Ammo:
				GameManager.Instance.GotAmmo();
				hit.gameObject.SetActive(false);
				break;
		}
	}
	#endregion

	#region Input Oriented Methods

	/// <summary>
	/// Initiates the Right-Side turn on the player
	/// </summary>
	/// <description>
	/// Checks whether the player is running, else it sets the animation to move to Right-Side. 
	/// </description>
	internal void InitiateRightTurn()
	{
		if(isMOVING)
		{
			return;
		}
		setAnimation(2);
		isMOVING = true;
		myCollider.center = new Vector2(-0.37f, 0f);
	}

	/// <summary>
	/// The animation for running it the Right-Side direction, i.e. towards positive X axis. 
	/// </summary>
	/// <description>
	/// The playervelocity variables is altered by leaving the Y values just as the same, just in case if the player is jumping. 
	/// The appropriate animation for running is played. 
	/// </description>
	internal void MoveTowardsRight()
	{
		playerVelocity = new Vector2(25 * deltaTime, playerVelocity.y);
		if((currentMotionState < 0))
		{
			setAnimation(2);
		}
	}

	/// <summary>
	/// Stops to move in the Right-Side direction. 
	/// </summary>
	/// <description>
	/// The animation is set to Right-Side Idle animation & player velocity is dropped to zero.
	/// </description>
	internal void StopRightMovement()
	{
		setAnimation(1);
		isMOVING = false;
		playerVelocity = Vector2.zero;
	}

	/// <summary>
	/// Initiates the Left-Side turn on the player
	/// </summary>
	/// <description>
	/// Checks whether the player is running, else it sets the animation to move to Left-Side. 
	/// </description>
	internal void InitiateLeftTurn()
	{
		if(isMOVING)
		{ 
			return;
		}
		setAnimation(-2);
		isMOVING = true;
		myCollider.center = new Vector2(0.24f, 0f);
	}

	/// <summary>
	/// The animation for running it the Left-Side direction, i.e. towards negative X axis. 
	/// </summary>
	/// <description>
	/// The playervelocity variables is altered by leaving the Y values just as the same, just in case if the player is jumping. 
	/// The appropriate animation for running is played. 
	/// </description>
	internal void MoveTowardsLeft()
	{
		playerVelocity = new Vector2(-25 * deltaTime, playerVelocity.y);
		if(currentMotionState > 0)
		{
			setAnimation(-2);
		}
	}

	/// <summary>
	/// Stops to move in the Left-Side direction. 
	/// </summary>
	/// <description>
	/// The animation is set to Left-Side idle animation & player velocity is dropped to zero.
	/// </description>
	internal void StopLeftMovement()
	{
		setAnimation(-1);
		isMOVING = false;
		playerVelocity = Vector2.zero;
	}

	/// <summary>
	/// When the player is required to JUMP, this module is called. 
	/// </summary>
	/// <description>
	/// If the player is already Jumping, this module does not execute beyond the first instruction. 
	/// The Co-Routine for the player to jump is being called. As the player is jumping - the required 
	/// animation are also played based on the current animation state. 
	/// </description>
	internal void MakeThePlayerToJump()
	{
		if(isJUMPING)
		{
			return;
		}
		isJUMPING = true;
		StartCoroutine(this.jumpRoutine());
		if(currentMotionState < 0)
		{
			setAnimation(-3);
		}
		else if(currentMotionState > 0)
		{
			setAnimation(3);
		}
		else
		{ 
			setAnimation(0);
		}
	}

	/// <summary>
	/// The Co-Routine responsible for the player to JUMP. 
	/// </summary>
	/// <description>
	/// A while-loop is being executed till a timer runs out of a boolean is changed which forces to stop jumping. 
	/// The boolean "isJUMPING" is altered based on the timer or if the player touches another collider. 
	/// The timer for half-second. Time.timeSinceLevelLoad is used as it is consistent, irrespective of Time.timeScale.
	/// </description>
	/// <returns>IEnumerable.</returns>
	private IEnumerator jumpRoutine()
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


	/// <summary>
	/// When a Scene Transition occurs
	/// </summary>
	/// <description>
	///	
	/// </description>
	internal void TelePortPlayer(Vector3 position)
	{
		this.transform.localPosition = position;
		StartCoroutine (SetPlayerProperties (true));
	}

	/// <summary>
	/// When a Projectile has to be fired by the Player. 
	/// </summary>
	/// <description>
	/// The WeaponCache is a reference on WeaponHub which pools the projectiles. When the player fires the projectile, 
	/// the properties for that projectile are also set. 
	/// </description>
	internal void FireAProjectile()
	{
		if(currentMotionState < 0)
		{
			WeaponCache.FireForPlayer(this.transform.position, Quaternion.Euler(0,0,90), ForceOnProjectile);
		}
		else if(currentMotionState > 0)
		{
			WeaponCache.FireForPlayer(this.transform.position, Quaternion.Euler(0,0,-90), ForceOnProjectile);
		}
		else 
		{
			WeaponCache.FireForPlayer(this.transform.position, Quaternion.identity, ForceOnProjectile);
		}
	}
	#endregion
}
