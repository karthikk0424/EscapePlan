
/// <summary>
/// Tween from one vector another
/// Game Manager notifies level manager anthing related to level happens
/// </summary>

using UnityEngine;
using System.Collections;

public class PositionTweener //: MonoBehaviour 
{	
	/*
	public Vector2 TweenFrom;
	public Vector2 TweenTo;
	public float TimeToMove = 4;
	public bool ResetOnLoad = false;
	public GameObject SourceObject;
	public TriggerActionType ActionType;
*/
	/*
	public void PlayAnimation()
	{
		this.StartCoroutine(StartAnimation(TweenFrom, TweenTo));
	}

	public void PlayAnimationReverse()
	{
		this.StartCoroutine(StartAnimation(TweenTo, TweenFrom));
	}

	public void StopAnimation()
	{
		StopCoroutine("StartAnimation");
	}

	private IEnumerator StartAnimation(Vector2 tweenFrom, Vector2 tweenTo)
	{
		float lastTime = Time.timeSinceLevelLoad;
		float timer = 0;
		TimeToMove = (1/TimeToMove);
		while ((timer < 1))
		{
			transform.localPosition = Vector2.Lerp(tweenFrom, tweenTo, EaseTheTimer(timer));
			timer += ((Time.timeSinceLevelLoad - lastTime) * TimeToMove);
			lastTime = Time.timeSinceLevelLoad;
			yield return null;
		}
			NPCManager.Instance.OnCompleteAction(gameObject, ActionType);
	}


*/



	public enum EasingType
	{
		EaseIn,
		EaseOut, 
		EaseInOut
	};
	public static EasingType ThisType = EasingType.EaseOut;


	internal static IEnumerator MoveObjectInLocal(Vector3 _startPosition, Vector3 _endPosition, GameObject _go, float _timer, System.Action _onComplete)
	{
		float lastTime = Time.timeSinceLevelLoad;
		float timer = 0;
		_timer = (1/_timer);
		while ((timer < 1))
		{
			_go.transform.localPosition = Vector2.Lerp(_startPosition, _endPosition, EaseTheTimer(timer));
			timer += ((Time.timeSinceLevelLoad - lastTime) * _timer);
			lastTime = Time.timeSinceLevelLoad;
			yield return null;
		}
		if(_onComplete != null)
		{
			_onComplete();
		}
	}



	//http://theinstructionlimit.com/wp-content/uploads/2009/07/Easing.cs
	private static float EaseTheTimer(float param)
	{
		switch(ThisType)
		{
			case EasingType.EaseIn:
				return	Mathf.Sin(param * (Mathf.PI/2)  - (Mathf.PI/2)) + 1;

			case EasingType.EaseOut:
				return	Mathf.Sin(param * (Mathf.PI/2));

			case EasingType.EaseInOut:
				return	(Mathf.Sin(param * (Mathf.PI/2) - (Mathf.PI/2)) + 1) / 2;	
	
			default:
				return param;
		}
	}
}
