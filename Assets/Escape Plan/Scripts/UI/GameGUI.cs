using UnityEngine;
using System.Collections;

public class GameGUI : MonoBehaviour 
{
	public GameObject LifeContainer, PauseMenu;
	public GameObject ChipUI;
	public TextMesh InfoText;

	private void Start()
	{
		PauseMenu.gameObject.SetActive(false);
	}

	private void OnEnable()
	{
		int curLevel = DataManager.Instance.CurrentLevelNumber;

		switch(curLevel)
		{
			case 7:
				UpdateInfoText("Boss!!!");
				break;

			case 8:
				UpdateInfoText("Be Positive");
				break;

			case 9:
				UpdateInfoText("");
				break;

			case 10:
				UpdateInfoText("Death is INEVITABLE!");
				break;

			default:
				UpdateInfoText("Level "+ curLevel.ToString());
				break;
		}


	}

	#region Game Data Manager Calls
	internal void UpdatePlayerLife()
	{
		SetPlayerLife(DataManager.Instance.LifeCount);
	}

	internal void UpdateChipCount()
	{
		SetUIChip(DataManager.Instance.ChipLootSac);
	}

	internal void UpdateInfoText(string _text)
	{
		InfoText.text = _text;
		StartCoroutine(this.resetText());
	}

	internal void TogglePauseMenu(bool toENABLE)
	{
		switch(toENABLE)
		{
			case true:
				PauseMenu.gameObject.SetActive(true);
				InfoText.text = "Pause Menu";
				break;

			case false:
				PauseMenu.gameObject.SetActive(false);
				InfoText.text = "Resumed";
				break;
		}
		StartCoroutine(this.resetText());
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

	private IEnumerator resetText()
	{
		yield return new WaitForSeconds(3f);
		InfoText.text = System.String.Empty;
	}
}
