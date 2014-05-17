using UnityEngine;
using System.Collections;

public class MedallionManager : MonoBehaviour 
{
	private void OnChipCollected(GameObject collectedObject)
	{
		collectedObject.SetActive(false);
		//Update UI
		//Add to User Stat
	}
}
