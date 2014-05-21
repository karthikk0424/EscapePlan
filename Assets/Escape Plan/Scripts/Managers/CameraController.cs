
/// <summary>
/// This class is responsible for Camera Movement. 
/// </summary>
/// <description>
/// The current functionality <list type="a"> when game is reset or a new level is loaded. 
/// <list type="b"> When the game requires the camera to move.
/// </description>

using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
	private Vector3 defaultMoviableUnits = new Vector3(0, 20, 0);

	#region Monobehaviour methods - Start & End

	/// <summary>
	/// Called only once when this camera is instanstatied for the first time. 
	/// </summary>
	/// <description>
	/// It sets all the default properties for the camera. 
	/// </description>
	private void Awake()
	{
		// Default Camera Properties
		this.camera.isOrthoGraphic = true;
		this.camera.orthographicSize = 10;
		this.camera.nearClipPlane = 0.3f;
		this.camera.farClipPlane = 20f;
		this.camera.renderingPath = RenderingPath.Forward;
	}

	/// <summary>
	/// Called whenever the Camera is enabled.
	/// </summary>
	/// <description>
	/// It subscribes to a delegate from Game Manager to handle when the game is being reset. 
	/// </description>
	private void OnEnable()
	{
		GameManager.OnReset += HandleOnReset;
	}

	/// <summary>
	/// It resets the camera to the default position.
	/// </summary>
	private void HandleOnReset()
	{
		this.transform.localPosition = ConstantVariablesContainer.DEFAULT_CAMERA_POSITION;
	}

	/// <summary>
	/// Called whenever the camera is being disabled
	/// </summary>
	/// <description>
	/// Unsubscribes to a delegate from Game Manager to handle when the game is being reset.
	/// </description>
	private void OnDisable()
	{
		GameManager.OnReset -= HandleOnReset;
	}

	#endregion

	#region Methods called externally

	/// <summary>
	/// Move the camera either on the positive Y axis or on it's negative.
	/// </summary>
	/// <param name="_moveUP">If set to <c>true</c> move on the positive Y axis.</param>
	internal void MoveCamera(bool _moveUP)
	{
		switch(_moveUP)
		{
			case true:
				this.transform.localPosition += defaultMoviableUnits;
				break;


			case false:
				this.transform.localPosition -= defaultMoviableUnits;
				break;
		}
	}

	/// <summary>
	/// Transforms the camera to a fixed position.
	/// </summary>
	/// <param name="_pos">Position to be transformed.</param>
	internal void SetCameraToThisPosition(Vector3 _pos)
	{
		this.transform.localPosition = _pos;
	}	

	#endregion
}
