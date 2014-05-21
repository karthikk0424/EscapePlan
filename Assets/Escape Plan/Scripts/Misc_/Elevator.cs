
/// <summary>
/// This script is responsible for the elevators in the scene. 
/// </summary>
 
using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Collider2D))]
public class Elevator : MonoBehaviour 
{
	#region Variables

	// Local Co-ordinates
	public Vector3 EndPosition;
	public bool moveATSTART = false;
	private Vector3 startPosition, resetStartPosition;
	private bool isMOVING; 

	#endregion

	#region Mono Behaviour - start & end methods

	/// <summary>
	/// Called when the elevator is instantiated. 
	/// </summary>
	/// <description>
	/// Initialized the variables
	/// </description>
	private void Awake()
	{
		startPosition = resetStartPosition = this.transform.localPosition;
		isMOVING = false;
	}

	/// <summary>
	/// Called when this gameobject is being enabled.
	/// </summary>
	/// <description>
	/// It subscribes to a delegate to handle the elevator to reset. 
	/// If the elevator is required to move at start then the co-routine is initiated. 
	/// </description>
	private void OnEnable()
	{
		GameManager.OnReset += handleOnReset;
		if(moveATSTART)
		{
			GameManager.Instance.ToggleUserControls(false);
			StartCoroutine(PositionTweener.MoveObjectInLocal(startPosition, EndPosition, this.gameObject, 2f, () => 
			                                                 {
																	GameManager.Instance.ToggleUserControls(true);
																	isMOVING = false;
																}));
		}
	}

	/// <summary>
	/// Called when this gameobject is being disabled.
	/// </summary>
	/// <description>
	/// It subscribes to a delegate to handle the elevator to reset. 
	/// </description>
	private void OnDisable()
	{
		GameManager.OnReset -= handleOnReset;
		if(moveATSTART)
		{
			this.transform.localPosition = resetStartPosition;
		}
	}

	/// <summary>
	/// Event to handle when to reset the elevator
	/// </summary>
	private void handleOnReset ()
	{
		if(resetStartPosition != startPosition)
		{
			EndPosition = startPosition;
			startPosition = resetStartPosition;
		}
		this.transform.localPosition = startPosition;
	}

	#endregion

	#region Collision Events

	/// <summary>
	/// Called whenever the player is one the collision attached with the elevator.
	/// </summary>
	/// <param name="hit">the infor about the collision.</param>
	/// <description>
	/// It looks for key event 'E' to activate the elevator. 
	/// </description>
	private void OnCollisionStay2D(Collision2D hit)
	{
		if((isMOVING) || (moveATSTART))
		{
			return;
		}
		if(Input.GetKeyDown(KeyCode.E))
		{
			if(hit.collider.tag == ConstantVariablesContainer.MainPlayer)
			{
				isMOVING = true;
				GameManager.Instance.ToggleUserControls(false);
				bool _isUP = ((startPosition.y > EndPosition.y) ? (false) : (true));
				StartCoroutine(PositionTweener.MoveObjectInLocal(startPosition, EndPosition, this.gameObject, 2f, () => 
				                                                 {
																	GameManager.Instance.ToggleUserControls(true);
																	GameManager.Instance.MoveCameraUp(_isUP);
																	isMOVING = false;
																}));
				startPosition += EndPosition;
				EndPosition = startPosition - EndPosition;
				startPosition -= EndPosition;
			}
		}
	}

	#endregion
}
