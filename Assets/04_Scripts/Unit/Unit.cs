using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Unit : MonoBehaviour
{
    [SerializeField]
	private UnitStates unitObject;	// get unit type;

	[SerializeField]
	public Unit target;

	public HexGrid hexGrid;
	[SerializeField]
	public Hex UnitHex;

	[SerializeField]
	private float movementDuration = 1, rotationDuration = 0.3f;

	private GlowHighlight _glowHighlight;

	private Queue<Vector3> pathPositions = new Queue<Vector3>();
	
	// test moveing
	public AstarNode targetPosition = null;
	public List<AstarNode> Astarpaths = new List<AstarNode>();
	public Queue<AstarNode> paths = new Queue<AstarNode>();

	// This Unit State
	public UnitState state;

	public event Action<Unit> MovementFinished;


#region Unity Functions
	private void Awake()
	{
		_glowHighlight = this.GetComponent<GlowHighlight>();
		state = new UnitState(unitObject);
	}

	private void Update()
	{
		UnitHex = hexGrid.GetTileAt(hexGrid.GetClosestHex(transform.position));
	}
#endregion

#region Public Functions
	public void Deselect()
	{
		_glowHighlight.ToggleGlow(false);
	}
	public void Select()
	{
		_glowHighlight.ToggleGlow();
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
		Quaternion startRotation = transform.rotation;
		endPosition.y = transform.position.y;
		Vector3 direction = endPosition - transform.position;
		Quaternion endRotation = Quaternion.LookRotation(direction, Vector3.up);

		if (Mathf.Approximately(Mathf.Abs(Quaternion.Dot(startRotation, endRotation)), 1.0f) == false)
		{
			float timeElapsed = 0;
			while (timeElapsed < rotationDuration)
			{
				timeElapsed += Time.deltaTime;
				float lerpStep = timeElapsed / rotationDuration;
				transform.rotation = Quaternion.Lerp(startRotation, endRotation, lerpStep);
				yield return null;
			}
			transform.rotation = endRotation;
		}
		StartCoroutine(MovementCoroutine(endPosition));
	}

	private IEnumerator MovementCoroutine(Vector3 endPosition)
	{
		Vector3 startPosition = transform.position;
		endPosition.y = startPosition.y;
		float timeElapsed = 0;

		// move time set!
		while (timeElapsed < movementDuration / state.MoveSpeed)
		{
			timeElapsed += Time.deltaTime;
			float lerpStep = timeElapsed / (movementDuration / state.MoveSpeed);
			transform.position = Vector3.Lerp(startPosition, endPosition, lerpStep);
			yield return null;
		}
		transform.position = endPosition;

		if (pathPositions.Count > 0)
		{
			Debug.Log("Select the next position");
			StartCoroutine(RotationCoroutine(pathPositions.Dequeue(), rotationDuration));
		}
		else
		{
			Debug.Log("Movement finished");
			MovementFinished?.Invoke(this);
		}
	}
#endregion
}

