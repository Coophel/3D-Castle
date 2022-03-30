using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharIdleState : CharBaseState
{
	public CharIdleState(UnitStateMachine currentContext, CharStateFactory charStateFactory)
	: base (currentContext, charStateFactory) {}
	public override void CheckSwitchState()
	{
		// if game start yet  (ready mode)
		if (_ctx.MoveFinish == false && _ctx.TestTarget != null)
			SwitchState(_factory.Move());
	}

	public override void EnterState()
	{
		Debug.Log ("Enter IdleState");

	}

	public override void ExitState()
	{
		Debug.Log ("Exit IdleState");
	}

	public override void InitalizeSubState()
	{
		throw new System.NotImplementedException();
	}

	public override void UpdateState()
	{
		Debug.Log ("Updating IdleState");
		_ctx.UnitAstarNode = _ctx.UnitHex.GetComponent<AstarNode>();
		CheckSwitchState();
	}
}
