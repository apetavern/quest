namespace Quest.Systems.States.Machines;

public class IdleStateMachine : PlayerStateMachine
{
	public override void Simulate()
	{
		base.Simulate();
	}

	public override void Start()
	{
		States.Add(new IdleState());

		ActiveState = States.First();
	}

	public override void Stop()
	{
		// nothing to do.
	}
}
