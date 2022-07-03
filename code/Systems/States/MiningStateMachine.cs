using Quest.Entities;
using Quest.Player;

namespace Quest.Systems.States;

public partial class MiningStateMachine : PlayerStateMachine
{
	public OreDeposit OreDeposit;

	public MiningStateMachine() { }

	public MiningStateMachine( OreDeposit targetOre )
	{
		OreDeposit = targetOre;
	}

	public override void Simulate()
	{
		base.Simulate();

		if ( ActiveState.Done )
		{
			StateIndex++;
			if ( StateIndex >= States.Count )
			{
				var player = Owner as QuestPlayer;
				player.StateMachine.ChangeStateMachine( new IdleStateMachine() );
			}
			else
			{
				ChangeState( States[StateIndex] );
			}
		}
	}

	public override void Start()
	{
		States.Add( new WalkingState( OreDeposit.Position, Owner ) );
		States.Add( new MiningState( OreDeposit, Owner ) );

		ChangeState( States.First() );
	}

	public override void Stop()
	{
		Log.Info( Host.Name + " Stop" );
	}
}
