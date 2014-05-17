using UnityEngine;
using System.Collections;

public enum PickUpItemType
{
	None = 0,
	Chips = 1,
	HackKit = 2
}

public class OnItemPickedBase : MonoBehaviour 
{
	protected void OnCollectedItem(GameObject collectedObject, PickUpItemType type)
	{
		switch (type)
		{
			case PickUpItemType.Chips:
			PlayerInventoryManager.Instance.ChipLootSac = StaticVariablesContainer.CHIP_VALUE;
			Debug.Log(PlayerInventoryManager.Instance.ChipLootSac);
			break;

			case PickUpItemType.HackKit:
			PlayerInventoryManager.Instance.HackKit = true;
			break;
		}
		collectedObject.SetActive(false);
	}
}
