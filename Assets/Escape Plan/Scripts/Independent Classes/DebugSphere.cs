/// <summary>
/// Primarily focussed to give Designers the freedom to place elements that are specific to each
/// and every level, which could cached by any script - whenever needed. 
/// Currently with a limited functionality, it could be expanded based upon designers needs. 
/// </summary>

using UnityEngine;
using System.Collections;

public class DebugSphere : MonoBehaviour
{ 
	/// <summary>
	/// Draw a gizmo which is spherical on the editor window. This gizmo is primarily used to spawn the player
	/// while a level is loaded. 
	/// </summary>
	private void OnDrawGizmosSelected() 
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere (transform.position, 1);
	}
}
