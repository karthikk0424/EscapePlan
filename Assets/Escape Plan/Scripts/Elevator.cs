using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Collider2D))]
public class Elevator : MonoBehaviour 
{
	// Local Co-ordinates
	public Vector3 EndPosition;
	public bool moveATSTART = false;
	private Vector3 StartPosition, resetStartPosition;
	private bool isMOVING; 

	private void Awake()
	{
		StartPosition = resetStartPosition = this.transform.localPosition;
		isMOVING = false;
	}

	private void OnEnable()
	{
		GameManager.OnReset += HandleOnReset;
		if(moveATSTART)
		{
	
			GameManager.Instance.ToggleUserControls(false);
			StartCoroutine(PositionTweener.MoveObjectInLocal(StartPosition, EndPosition, this.gameObject, 2f, () => 
			                                                 {
																	GameManager.Instance.ToggleUserControls(true);
																	isMOVING = false;
																}));
		}
	}

	private void OnDisable()
	{
		GameManager.OnReset -= HandleOnReset;
		if(moveATSTART)
		{
			this.transform.localPosition = resetStartPosition;
		}
	}

	private void HandleOnReset ()
	{
		if(resetStartPosition != StartPosition)
		{
			EndPosition = StartPosition;
			StartPosition = resetStartPosition;
		}
		this.transform.localPosition = StartPosition;
	}

	private void OnCollisionStay2D(Collision2D hit)
	{
		if((isMOVING) || (moveATSTART))
		{
			return;
		}
		if(Input.GetKeyDown(KeyCode.E))
		{
			if(hit.collider.tag == "Player")
			{
				isMOVING = true;
				GameManager.Instance.ToggleUserControls(false);
				bool _isUP = ((StartPosition.y > EndPosition.y) ? (false) : (true));
				StartCoroutine(PositionTweener.MoveObjectInLocal(StartPosition, EndPosition, this.gameObject, 2f, () => 
				                                                 {
																	GameManager.Instance.ToggleUserControls(true);
																	GameManager.Instance.MoveCameraUp(_isUP);
																	isMOVING = false;
																}));
				StartPosition += EndPosition;
				EndPosition = StartPosition - EndPosition;
				StartPosition -= EndPosition;
			}
		}
	}
}
