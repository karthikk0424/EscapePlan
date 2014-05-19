
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
	public float TimeToMove = 4;
	//public float Speed = 0.25f;
	public bool ResetOnLoad = false;

	public enum EasingType
	{
		EaseIn,
		EaseOut, 
		EaseInOut
	};
	public EasingType ThisType = EasingType.EaseOut;

	public void PlayAnimation()
	{
		this.StartCoroutine(StartAnimation());
	}

	private IEnumerator StartAnimation()
	{
		float lastTime = Time.timeSinceLevelLoad;
		float timer = 0;
		TimeToMove = (1/TimeToMove);
		while ((timer < 1))
		{
			transform.localPosition = Vector2.Lerp(TweenFrom, TweenTo, EaseTheTimer(timer));
			timer += ((Time.timeSinceLevelLoad - lastTime) * TimeToMove);
			lastTime = Time.timeSinceLevelLoad;
			yield return null;
		}
	}

	//http://theinstructionlimit.com/wp-content/uploads/2009/07/Easing.cs
	private float EaseTheTimer(float param)
	{
		switch(ThisType)
		{
			case EasingType.EaseIn:
				return	Mathf.Sin(param * (Mathf.PI/2)  - (Mathf.PI/2)) + 1;
				break;

			case EasingType.EaseOut:
				return	Mathf.Sin(param * (Mathf.PI/2));
				break;

			case EasingType.EaseInOut:
				return	(Mathf.Sin(param * (Mathf.PI/2) - (Mathf.PI/2)) + 1) / 2;	
				break;
	
			default:
				return param;
				break;
		}
	}


}
