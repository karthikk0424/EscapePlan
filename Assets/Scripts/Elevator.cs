using UnityEngine;
using System.Collections;

public enum ElevatorState
{
	ElevatorUp,
	ElevatorDown
}

public class Elevator : MonoBehaviour {

	public ElevatorState currentState = ElevatorState.ElevatorDown;

	internal void StartElevator()
	{
		//THis level has nothing to do with the scene level;
		if(currentState == ElevatorState.ElevatorDown)
		{
			NPCManager.Instance.PlayAnimation(gameObject);
		}
		else
		{
			NPCManager.Instance.PlayAnimationReverse(gameObject);
		}
		ElevatorSwitch(false);
		UpdateElevatorLevel();
	}

	
	public void ElevatorSwitch(bool turnOn)
	{
		transform.FindChild("Collider").gameObject.SetActive(turnOn);
	}

	private void UpdateElevatorLevel()
	{
	}

}
