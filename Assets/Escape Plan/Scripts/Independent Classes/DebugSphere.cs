using UnityEngine;
using System.Collections;

public class DebugSphere : MonoBehaviour {

	private void OnDrawGizmosSelected() 
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(transform.position, 1);
	}
}
