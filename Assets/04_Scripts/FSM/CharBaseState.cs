public abstract class CharBaseState
{
	protected UnitStateMachine _ctx;
	protected CharStateFactory _factory;
	public CharBaseState(UnitStateMachine currentContext, CharStateFactory charStateFactory)
	{
		_ctx = currentContext;
		_factory = charStateFactory;
	}

	void UpdateStates() {}
	protected void SwitchState(CharBaseState newState)
	{
		ExitState();

		newState.EnterState();

		_ctx.CurrentState = newState;
	}
	protected void SetSuperState() {}
	protected void SetSubState() {}

	public abstract void EnterState();
	public abstract void UpdateState();
	public abstract void ExitState();
	public abstract void CheckSwitchState();
	public abstract void InitalizeSubState();
}