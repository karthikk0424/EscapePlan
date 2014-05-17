using UnityEngine;
using System.Collections;

public class DoorOpenner : MonoBehaviour 
{
	private bool isDoorOpened;

	private void OnTriggerEnter2D(Collider2D other)
	{
		//SceneManager.Instance.OpenDoor();
		//SceneManager has private methods to check for door State DoorState.Locked Door State.UnLocked
		//SceneManager will process scene trasitions
	}
}
