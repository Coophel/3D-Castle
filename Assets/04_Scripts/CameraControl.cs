using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControl : MonoBehaviour
{
    // Component variables
	private CameraControllActions _cameraActions;
	private InputAction _movement;
	private Transform _cameraTransform;
	[SerializeField]
	private Transform _baseTargetTransform;
	[SerializeField]
	private Transform _targetTransform;

	// horizontal motion variables
	[SerializeField]
	private float _maxSpeed = 5f;
	private float _speed;
	[SerializeField]
	private float _acceleration = 10f;
	[SerializeField]
	private float _damping = 15f;

	// vertical motion (Zomming) variables
	[SerializeField]
	private float _stepSize = 2f;
	[SerializeField]
	private float _zoomDampening = 7.5f;
	[SerializeField]
	private float _minHeight = 5f;
	[SerializeField]
	private float _maxHeight = 30f;
	[SerializeField]
	private float _zoomSpeed = 2f;

	// rotation variables
	[SerializeField]
	private float _maxRotationSpeed = 1f;

	// Screen edge motion
	// [SerializeField]
	// [Range(0f, 0.1f)]
	// private float _edgeTolerance = 0.05f;
	// [SerializeField]
	// private bool _useScreenEdge = true;

	private float _zoomHeight;

	private Vector3 _horizontalVelocity;
	private Vector3 _lastPosition;

	Vector3 _startDrag;

#region Unity Functions
	private void Awake()
	{
		_cameraActions = new CameraControllActions();
		_cameraTransform = this.GetComponentInChildren<Camera>().transform;
	}

	private void OnEnable()
	{
		_zoomHeight = _cameraTransform.localPosition.y;
		_cameraTransform.LookAt(_baseTargetTransform);

		_lastPosition = this.transform.position;

		_movement = _cameraActions.Camera.MovementAction;
		_cameraActions.Camera.RotateCamera.performed += RotateCamera;
		_cameraActions.Camera.ZoomCamera.performed += ZoomCamera;
		_cameraActions.Camera.Enable();
	}


	private void OnDisable()
	{
		_cameraActions.Camera.RotateCamera.performed -= RotateCamera;
		_cameraActions.Camera.ZoomCamera.performed -= ZoomCamera;
		// _cameraActions.Camera.
		_cameraActions.Camera.Disable();
	}

	private void Update()
	{
		// Inputs
		GetKeyboardMovement();

		// movement of camera Object;
		UpdateVelocity();
		UpdateBasePosition();
		UpdateCameraPosition();
	}
#endregion

#region Public Functions
	// public void GetTargetPosition(GameObject targetObject)
	// {
	// 	Debug.Log("Get Target!");
	// 	_targetTransform = targetObject.transform;
	// }
#endregion

#region Private Functions
	private void ZoomCamera(InputAction.CallbackContext inputValue)
	{
		float value = -inputValue.ReadValue<Vector2>().y / 100f;

		if (Mathf.Abs(value) > 0.1f)
		{
			_zoomHeight = _cameraTransform.localPosition.y + value * _stepSize;

			if (_zoomHeight < _minHeight)
				_zoomHeight = _minHeight;
			else if (_zoomHeight > _maxHeight)
				_zoomHeight = _maxHeight;
		}
	}
	private void RotateCamera(InputAction.CallbackContext inputValue)
	{
		if (!Mouse.current.middleButton.isPressed)
			return;

		float value = inputValue.ReadValue<Vector2>().x;
		transform.rotation = Quaternion.Euler(0f, value * _maxRotationSpeed + transform.rotation.eulerAngles.y, 0f);
	}
	private void UpdateVelocity()
	{
		_horizontalVelocity = (this.transform.position - _lastPosition) / Time.deltaTime;
		_horizontalVelocity.y = 0f;
		_lastPosition = this.transform.position;
	}
	private void UpdateBasePosition()
	{
		if (_baseTargetTransform.position.sqrMagnitude > 0.1f)
		{
			//create a ramp up or acceleration
			_speed = Mathf.Lerp(_speed, _maxSpeed, Time.deltaTime * _acceleration);
			transform.position += _baseTargetTransform.position * _speed * Time.deltaTime;
		}
		else
		{
			//create smooth slow down
			_horizontalVelocity = Vector3.Lerp(_horizontalVelocity, Vector3.zero, Time.deltaTime * _damping);
			transform.position += _horizontalVelocity * Time.deltaTime;
		}

		//reset for next frame
		_baseTargetTransform.position = Vector3.zero;
	}
	private void GetKeyboardMovement()
	{
		Vector3 inputValue = _movement.ReadValue<Vector2>().x * GetCameraRight()
					+ _movement.ReadValue<Vector2>().y * GetCameraForward();

		inputValue = inputValue.normalized;

		if (inputValue.sqrMagnitude > 0.1f)
			_baseTargetTransform.position += inputValue;
	}
	private void UpdateCameraPosition()
	{
		//set zoom target
		Vector3 zoomTarget = new Vector3(_cameraTransform.localPosition.x, _zoomHeight, _cameraTransform.localPosition.z);
		//add vector for forward/backward zoom
		zoomTarget -= _zoomSpeed * (_zoomHeight - _cameraTransform.localPosition.y) * Vector3.forward;

		_cameraTransform.localPosition = Vector3.Lerp(_cameraTransform.localPosition, zoomTarget, Time.deltaTime * _zoomDampening);
		if (_targetTransform == null)
			_cameraTransform.LookAt(_baseTargetTransform);
		else
			_cameraTransform.LookAt(_targetTransform);
	}
	private Vector3 GetCameraForward()
	{
		Vector3 forward = _cameraTransform.forward;
		forward.y = 0f;
		return forward;
	}

	//gets the horizontal right vector of the camera
	private Vector3 GetCameraRight()
	{
		Vector3 right = _cameraTransform.right;
		right.y = 0f;
		return right;
	}
#endregion
}
