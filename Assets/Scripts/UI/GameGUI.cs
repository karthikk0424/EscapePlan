using UnityEngine;
using System.Collections;

public class GameGUI : MonoBehaviour 
{

	public GameObject LifeContainer;

	internal void SetPlayerLife(int lifeCount)
	{
		foreach(Transform t in LifeContainer.transform)
		{
			t.gameObject.SetActive(false);
		}
		for(int i = 1; i <= lifeCount; i++ )
		{
			LifeContainer.transform.GetChild(i - 1).gameObject.SetActive(true);
		}
	}
}
