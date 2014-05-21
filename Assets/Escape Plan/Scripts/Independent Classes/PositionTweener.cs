
/// <summary>
/// Tweens a gameobject from one position to another. 
/// As the methods are static, it could be called from any other class. 
/// </summary>

using UnityEngine;
using System.Collections;

public class PositionTweener
{	
	#region Variables
	// The type of easing function to be deployed.
	public enum EasingType
	{
		EaseIn,
		EaseOut, 
		EaseInOut
	};
	public static EasingType ThisType = EasingType.EaseOut;

	#endregion

	#region Static methods for external calls to Tween
	/// <summary>
	/// Move the gameobject in local co-ordinates.
	/// </summary>
	/// <returns>Yield in a while loop, till the timer runs out.</returns>
	/// <param name="_startPosition">The start position.</param>
	/// <param name="_endPosition">The end position</param>
	/// <param name="_go">The gameobject to move.</param>
	/// <param name="_timer">Time to take to tween.</param>
	/// <param name="_onComplete">When the job is done, it reports back.</param>
	internal static IEnumerator MoveObjectInLocal( Vector3 _startPosition, Vector3 _endPosition, GameObject _go, float _timer, System.Action _onComplete)
	{
		float lastTime = Time.timeSinceLevelLoad;
		float timer = 0;
		_timer = (1/_timer);
		while ((timer < 1))
		{
			_go.transform.localPosition = Vector2.Lerp (_startPosition, _endPosition, EaseTheTimer (timer));
			timer += ((Time.timeSinceLevelLoad - lastTime) * _timer);
			lastTime = Time.timeSinceLevelLoad;
			yield return null;
		}
		if(_onComplete != null)
		{
			_onComplete ();
		}
	}
	#endregion


	#region Easing Functions 

	/// <summary>
	/// A timer is implemented to ease during linear interpolation. 
	/// </summary>
	/// <returns>The eased variable for the param</returns>
	/// <param name="param"> timer variable to ease.</param>
	/// <see cref="http://theinstructionlimit.com/wp-content/uploads/2009/07/Easing.cs"/>
	private static float EaseTheTimer(float param)
	{
		switch (ThisType)
		{
			case EasingType.EaseIn:
				return	(Mathf.Sin (param * (Mathf.PI/2)  - (Mathf.PI/2)) + 1);

			case EasingType.EaseOut:
				return	(Mathf.Sin (param * (Mathf.PI/2)));

			case EasingType.EaseInOut:
				return	((Mathf.Sin (param * (Mathf.PI/2) - (Mathf.PI/2)) + 1) / 2);	
	
			default:
				return param;
		}
	}
	#endregion
}
