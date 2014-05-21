
/// <summary>
/// The UI present during the game play.
/// </summary>

using UnityEngine;
using System.Collections;

public class GameGUI : MonoBehaviour 
{
	#region Public Variables

	public GameObject LifeContainer, PauseMenu;
	public GameObject ChipUI;
	public TextMesh InfoText;

	#endregion

	#region Monobehaviour methods - start & end methods

	/// <summary>
	/// When this game object enabled, it is called. UI Text is primarily updated here.
	/// </summary>
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

	/// <summary>
	/// When this game object is enabled. Called after OnEnable method.
	/// </summary>
	private void Start()
	{
		PauseMenu.gameObject.SetActive(false);
	}

	#endregion

	#region UI methods for displaying

	/// <summary>
	/// UI for player life is updated
	/// </summary>
	internal void UpdatePlayerLife()
	{
		SetPlayerLife (DataManager.Instance.LifeCount);
	}

	/// <summary>
	/// UI responsible for Chip count is updated
	/// </summary>
	internal void UpdateChipCount()
	{
		SetUIChip (DataManager.Instance.ChipLootSac);
	}

	/// <summary>
	/// Info text at the bottow right corner is updated.
	/// </summary>
	/// <param name="_text">The text to be shown</param>
	internal void UpdateInfoText(string _text)
	{
		InfoText.text = _text;
		StartCoroutine (this.resetText());
	}

	/// <summary>
	/// Toggles the pause menu.
	/// </summary>
	/// <param name="toENABLE">If set to <c>true</c> display the pause menu.</param>
	internal void TogglePauseMenu(bool _toENABLE)
	{
		switch(_toENABLE)
		{
			case true:
				PauseMenu.gameObject.SetActive (true);
				InfoText.text = "Pause Menu";
				break;

			case false:
				PauseMenu.gameObject.SetActive (false);
				InfoText.text = "Resumed";
				break;
		}
		StartCoroutine (this.resetText ());
	}

	/// <summary>
	/// Info on the player life is updated
	/// </summary>
	/// <param name="lifeCount">Life count.</param>
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

	/// <summary>
	/// Set the count for chips collected
	/// </summary>
	/// <param name="chipCount"> Total chips collected.</param>
	private void SetUIChip(int _chipCount)
	{
		ChipUI.GetComponent<TextMesh>().text = _chipCount.ToString("000");
	}
	
	/// <summary>
	/// Resets the text to empty string
	/// </summary>
	/// <returns> wait time before to display empty string</returns>
	private IEnumerator resetText()
	{
		yield return new WaitForSeconds(3f);
		InfoText.text = System.String.Empty;
	}

	#endregion
	
}
