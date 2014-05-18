
/// <summary>
/// Tween from one vector another
/// Game Manager notifies level manager anthing related to level happens
/// </summary>

using UnityEngine;
using System.Collections;

public class PositionTweener : MonoBehaviour 
{	
	public Vector2 From;
	public Vector2 To;
	public float Speed = 0.25f;

	public void Awake()
	{
		LevelManager.Instance.RegisterToPlayAnimation(this.gameObject);
	}

	public void PlayAnimation()
	{
		StartCoroutine(StartAnimation());
	}

	IEnumerator  StartAnimation()
	{

		float lastTime = Time.timeSinceLevelLoad;
		float timer = 0, vel = 0;
		while ((timer < 1))
		{
			Debug.Log(timer);
			transform.localPosition = Vector2.Lerp(From, To, timer);
			timer += ((Time.timeSinceLevelLoad - lastTime) * Speed);
			lastTime = Time.timeSinceLevelLoad;
			yield return null;
		}
	}

	public void ResetPosition()
	{

	}
}
