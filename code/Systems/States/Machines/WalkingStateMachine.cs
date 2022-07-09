using Quest.Player;

namespace Quest.Systems.States.Machines;

public class WalkingStateMachine : PlayerStateMachine
{
	public Vector3 Target;

	public WalkingStateMachine() { }

	public WalkingStateMachine( Vector3 target )
	{
		Target = target;
	}

	public override void Simulate()
	{
		base.Simulate();

		if ( ActiveState.Done )
		{
			var player = Owner as QuestPlayer;
			player.ChangeStateMachine( new IdleStateMachine() );
		}
	}

	public override void Start()
	{
		States.Add( new WalkingState( WalkType.ToPosition, Target, Owner ) );
		ChangeState( States.First() );
	}

	public override void Stop()
	{

	}
}
