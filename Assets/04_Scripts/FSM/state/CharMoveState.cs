using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharMoveState : CharBaseState
{
	public CharMoveState(UnitStateMachine currentContext, CharStateFactory charStateFactory)
	: base (currentContext, charStateFactory) {}
	public override void CheckSwitchState()
	{
		if (_ctx.MoveFinish == true)
		{
			SwitchState(_factory.Idle());
		}
	}

	public override void EnterState()
	{
		Debug.Log ("Enter MoveState");
		Debug.Log(_ctx.TestTarget.gameObject.name);
		_ctx._astarPaths = Astar.FindPathAstar(_ctx.UnitAstarNode, _ctx.TestTarget.GetComponent<AstarNode>());
		_ctx.GetNextPath(_ctx._astarPaths);
		MoveBySpeed(_ctx.UnitState.MoveSpeed);
	}

	public override void ExitState()
	{
		Debug.Log ("Exit MoveState");
	}

	public override void InitalizeSubState()
	{
		throw new System.NotImplementedException();
	}

	public override void UpdateState()
	{
		CheckSwitchState();
	}

	void MoveBySpeed(float speed)
	{
		_ctx._pathPositions = new Queue<Vector3>(_ctx._paths);
		Vector3 firstTarget = _ctx._pathPositions.Dequeue();
		_ctx.StartCoroutine(RotationCoroutine(firstTarget, 0.3f));
	}
	IEnumerator RotationCoroutine(Vector3 endPosition, float rotationDuration)
	{
		Quaternion startRotation = _ctx.transform.rotation;
		endPosition.y = _ctx.transform.position.y;
		Vector3 direction = endPosition - _ctx.transform.position;
		Quaternion endRotation = Quaternion.LookRotation(direction, Vector3.up);

		if (Mathf.Approximately(Mathf.Abs(Quaternion.Dot(startRotation, endRotation)), 1.0f) == false)
		{
			float timeElapsed = 0;
			while (timeElapsed < rotationDuration)
			{
				timeElapsed += Time.deltaTime;
				float lerpStep = timeElapsed / rotationDuration;
				_ctx.transform.rotation = Quaternion.Lerp(startRotation, endRotation, lerpStep);
				yield return null;
			}
			_ctx.transform.rotation = endRotation;
		}
		_ctx.StartCoroutine(MovementCoroutine(endPosition));
	}
	IEnumerator MovementCoroutine(Vector3 endPosition)
	{
		Vector3 startPosition = _ctx.transform.position;
		endPosition.y = startPosition.y;
		float timeElapsed = 0;

		// move time set!
		while (timeElapsed < _ctx.UnitState.MoveSpeed)
		{
			timeElapsed += Time.deltaTime;
			float lerpStep = timeElapsed / (_ctx.UnitState.MoveSpeed);
			_ctx.transform.position = Vector3.Lerp(startPosition, endPosition, lerpStep);
			yield return null;
		}
		_ctx.transform.position = endPosition;

		if (_ctx._pathPositions.Count > 0)
		{
			Debug.Log("Select the next position");
			_ctx.StartCoroutine(RotationCoroutine(_ctx._pathPositions.Dequeue(), 0.3f));
		}
		else
		{
			Debug.Log("Movement finished");
			_ctx.MoveFinish = true;
		}
	}
}
