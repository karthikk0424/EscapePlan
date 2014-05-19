using UnityEngine;
using System.Collections;

public class Evlevator : MonoBehaviour {

	public LevelEnum level = LevelEnum.Level0;

	internal void StartElevator()
	{
		if(level == LevelEnum.Level0)
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
		if(level == LevelEnum.Level0)
		{
			level = LevelEnum.Level1;
		}
		GameManager.Instance.CurrentPlayerLevel = level;
	}

}
