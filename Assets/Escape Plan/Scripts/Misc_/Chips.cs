
/// <summary>
/// Chips that are present on the level for player to collect.
/// </summary>
/// <description>
/// Rotates the chips is it's primary functionality.
/// </description>

using UnityEngine;
using System.Collections;

public class Chips : MonoBehaviour
{
	#region Event from GameManager

	/// <summary>
	/// Called when this gameobject is being enabled.
	/// </summary>
	/// <description>
	/// It is subscribes to the game manager to know when to rotate the chip. 
	/// </description>
	private void OnEnable()
	{
		GameManager.onRotation += handleOnRotation;
	}

	/// <summary>
	/// This is an event which rotates the coin.
	/// </summary>
	/// <param name="_rot"> The desired rotation</param>
	private void handleOnRotation (Quaternion _rot)
	{
		this.transform.rotation = _rot;
	}

	/// <summary>
	/// Called when this gameobject is disabled
	/// </summary>
	/// <description>
	/// It is un-subscribes to the game manager.
	/// </description>
	private void OnDisable()
	{
		GameManager.onRotation -= handleOnRotation;
	}

	#endregion
}