using UnityEngine;
using System.Collections;

public class OnCollectItem : OnItemPickedBase 
{
	public PickUpItemType ItemType;
	private void OnTriggerEnter2D(Collider2D other)
	{
		base.OnCollectedItem(gameObject, ItemType);
	}
}
