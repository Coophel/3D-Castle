using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharTestMove : CharMoveState
{
	public CharTestMove(UnitStateMachine currentContext, CharStateFactory charStateFactory) : base(currentContext, charStateFactory)
	{
	}
	public override void EnterState()
	{
		base.EnterState();
	}
}
