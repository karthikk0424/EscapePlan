using UnityEngine;
using System.Collections;

public class PlayerMotion : MonoBehaviour 
{
	public Animator Player;
	public Rigidbody2D PlayerRigidbody;

	private bool isMOVING = false, isJUMPING = false;
	private int currentMotionState = 0;
	private float deltaTime = 0;
	private Vector2 playerVelocity = Vector2.zero;

	private void Start()
	{
		deltaTime = (Time.deltaTime * 45f);
	}

	private void Update()
	{
		deltaTime = (Time.deltaTime * 45f);
		if(Input.GetKeyDown(KeyCode.LeftArrow))
		{
			if(isMOVING){ return;}
			setMotion(-2);
			isMOVING = true;
		}

		if(Input.GetKeyUp(KeyCode.LeftArrow))
		{
			setMotion(-1);
			isMOVING = false;
			playerVelocity = Vector2.zero;
		}	

		if(Input.GetKey(KeyCode.LeftArrow))
		{
			playerVelocity = new Vector2(-25 * deltaTime, playerVelocity.y);
			if(currentMotionState > 0)
			{
				setMotion(-2);
			}
		}
		
		if(Input.GetKeyDown(KeyCode.RightArrow))
		{
			if(isMOVING){ return;}
			setMotion(2);
			isMOVING = true;
		}

		if(Input.GetKeyUp(KeyCode.RightArrow))
		{
			setMotion(1);
			isMOVING = false;
			playerVelocity = Vector2.zero;
		}

		if(Input.GetKey(KeyCode.RightArrow))
		{
			playerVelocity = new Vector2(25 * deltaTime, playerVelocity.y);
			if((currentMotionState < 0))
			{
				setMotion(2);
			}
		}

		if(Input.GetKeyDown(KeyCode.UpArrow))
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
		PlayerRigidbody.velocity = playerVelocity;
	}

	private IEnumerator Jump()
	{
		float lastTime = Time.timeSinceLevelLoad;
		float timer = 0, vel = 0;
		while (timer < 1)
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
		Player.SetInteger("MotionState", state);
	}

	private void OnCollisionEnter2D(Collision2D hit) 
	{
		if(hit.collider.tag == "Ground")
		{
			isJUMPING = false;
			Debug.Log("<color=red> State =  </color> " + currentMotionState);
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
}
