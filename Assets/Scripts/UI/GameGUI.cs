using UnityEngine;
using System.Collections;

public class GameGUI : MonoBehaviour 
{

	public GameObject LifeContainer;
	public GameObject ChipUI;

	#region Game Data Manager Calls
	internal void UpdatePlayerLife()
	{
		SetPlayerLife(DataManager.Instance.LifeCount);
	}

	internal void UpdateChipCount()
	{
		SetUIChip(DataManager.Instance.ChipLootSac);
	}
	#endregion

	private void SetPlayerLife(int lifeCount)
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
	
	private void SetUIChip(int chipCount)
	{
		ChipUI.GetComponent<TextMesh>().text = chipCount.ToString("000");
	}

}
