using UnityEngine;
using System.Collections;

public class Chips : MonoBehaviour
{

	private void OnEnable()
	{
		GameManager.onRotation += HandleonRotation;
	}

	void HandleonRotation (Quaternion _rot)
	{
		this.transform.rotation = _rot;
	}

	private void OnDisable()
	{
		GameManager.onRotation -= HandleonRotation;
	}


}