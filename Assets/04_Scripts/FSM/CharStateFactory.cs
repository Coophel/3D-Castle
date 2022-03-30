public class CharStateFactory
{
    UnitStateMachine _context;

	public CharStateFactory(UnitStateMachine currentContext)
	{
		_context = currentContext;
	}

	public CharBaseState Idle()
	{
		return new CharIdleState(_context, this);
	}
	public CharBaseState Move()
	{
		return new CharMoveState(_context, this);
	}
	public CharBaseState Attack()
	{
		return new CharAttackState(_context, this);
	}
}
