using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Unit : MonoBehaviour
{
    [SerializeField]
	private float moveSpeed;
	public float MoveSpeed { get => moveSpeed; }

	[SerializeField]
	private float movementDuration , rotationDuration ;

	private GlowHighlight glowHighlight;
	private Queue<Vector3> pathPositions = new Queue<Vector3>();

	public event Action<Unit> MovementFinished;

#region Unity Functions
	private void Awake()
	{
		glowHighlight = GetComponent<GlowHighlight>();
	}
#endregion

#region Public Functions
	public void Deselect()
	{
		glowHighlight.ToggleGlow(false);
	}
	public void Select()
	{
		glowHighlight.ToggleGlow();
	}

	public void MoveThroughPath(List<Vector3> currentPath)
	{
		pathPositions = new Queue<Vector3>(currentPath);
		Vector3 firstTarget = pathPositions.Dequeue();
		StartCoroutine(RotationCoroutine(firstTarget, rotationDuration));
	}

#endregion

#region Private Functions
	private IEnumerator RotationCoroutine(Vector3 endPosition, float rotationDuration)
	{
		
		yield return null;
	}

	private IEnumerator MovementCoroutine(Vector3 endPosition)
	{
		yield return null;
	}
#endregion
}
