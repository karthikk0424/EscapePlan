using UnityEngine;
using System.Collections;

public static class TestTween
{
	internal static IEnumerator MoveObjectFrom(Vector3 _startPosition, Vector3 _endPosition, GameObject _go, float _timer, System.Action _onComplete)
	{
		float lastTime = Time.timeSinceLevelLoad;
		float timer = 0;
		_timer = (1/_timer);
		while ((timer < 1))
		{
			_go.transform.localPosition = Vector2.Lerp(_startPosition, _endPosition, timer);
			timer += ((Time.timeSinceLevelLoad - lastTime) * _timer);
			lastTime = Time.timeSinceLevelLoad;
			yield return null;
		}
		if(_onComplete != null)
		{
			_onComplete();
		}
	}




	internal static void DetectMethod()
	{
		Debug.Log("Method");
	}

}
