using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class UnitMerge : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
	[SerializeField]
	LayerMask _layer;

	private GameObject _currentRaycastObject;
	[SerializeField]
	private Vector3 _currentPosition;
    
	private Camera _camera;
	private RaycastHit _hit;

	public void OnPointerDown(PointerEventData eventData)
	{
		Debug.Log ("OnMouseDrag");
		transform.position = new Vector3(_camera.ScreenToWorldPoint(Mouse.current.position.ReadValue()).x, transform.position.y + 1f, _camera.ScreenToWorldPoint(Mouse.current.position.ReadValue()).z);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		Debug.Log("OnMouseDown");

		_currentPosition = transform.position;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		Debug.Log ("OnMouseUp");
		if (Physics.Raycast(_camera.ScreenToWorldPoint(Mouse.current.position.ReadValue()), _camera.transform.forward, out _hit, 10f, _layer))
		{
			Debug.Log($"What we hit! {_hit.collider}");
		}
		else
		{
			OnNullTile();
		}
	}

#region Unity Functions
	private void Awake()
	{
		_camera = Camera.main;
	}
#endregion

#region Public Functions

#endregion

#region Private Functions
	// private void OnMouseDown()
	// {
	// 	Debug.Log("OnMouseDown");
	// 	_currentPosition = transform.position;
	// }

	// private void OnMouseDrag()
	// {
	// 	Debug.Log ("OnMouseDrag");
	// 	transform.position = new Vector3(_camera.ScreenToWorldPoint(Mouse.current.position.ReadValue()).x, transform.position.y + 1f, _camera.ScreenToWorldPoint(Mouse.current.position.ReadValue()).z);
	// }

	// private void OnMouseUp()
	// {
	// 	Debug.Log ("OnMouseUp");
	// 	if (Physics.Raycast(_camera.ScreenToWorldPoint(Mouse.current.position.ReadValue()), _camera.transform.forward, out _hit, 10f, _layer))
	// 	{
	// 		Debug.Log($"What we hit! {_hit.collider}");
	// 	}
	// 	else
	// 	{
	// 		OnNullTile();
	// 	}
	// }

	private void OnNullTile()
	{
		transform.position = _currentPosition;
	}
#endregion
}
