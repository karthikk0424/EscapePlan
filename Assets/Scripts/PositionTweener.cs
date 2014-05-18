
/// <summary>
/// Tween from one vector another
/// Game Manager notifies level manager anthing related to level happens
/// </summary>

using UnityEngine;
using System.Collections;

public class PositionTweener : MonoBehaviour 
{	
	public Vector2 TweenFrom;
	public Vector2 TweenTo;
	public float Speed = 0.25f;
	public bool ResetOnLoad = false;

	public void PlayAnimation()
	{
		StartCoroutine(StartAnimation());
	}

	IEnumerator  StartAnimation()
	{

		float lastTime = Time.timeSinceLevelLoad;
		float timer = 0;
		while ((timer < 1))
		{
			transform.localPosition = Vector2.Lerp(TweenFrom, TweenTo, timer);
			timer += ((Time.timeSinceLevelLoad - lastTime) * Speed);
			lastTime = Time.timeSinceLevelLoad;
			yield return null;
		}
	}
}
