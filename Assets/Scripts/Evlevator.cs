using UnityEngine;
using System.Collections;

public class Evlevator : MonoBehaviour {

	public enum ElevatorLevelEnum
	{
		Level0,
		Level1,
	}

	public ElevatorLevelEnum level = ElevatorLevelEnum.Level0;

	internal void StartElevator()
	{
		if(level == ElevatorLevelEnum.Level0)
		{
			ElevatorSwitch(false);
			NPCManager.Instance.PlayAnimation(gameObject);
		}

		else
		{

		}

	}
	
	public void ElevatorSwitch(bool turnOn)
	{
		GetComponentInChildren<EdgeCollider2D>().gameObject.SetActive(turnOn);
	}

}
