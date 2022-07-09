using Quest.Entities;
using Quest.Player;

namespace Quest.Systems.States.Machines;

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
				player.ChangeStateMachine( new IdleStateMachine() );
			}
			else
			{
				ChangeState( States[StateIndex] );
			}
		}
	}

	public override void Start()
	{
		States.Add( new WalkingState( WalkType.ToEntity, OreDeposit, Owner ) );
		States.Add( new MiningState( OreDeposit, Owner ) );

		if ( States.Count > 0 )
			ChangeState( States.First() );
	}

	public override void Stop()
	{
		Log.Info( Host.Name + " Stop" );
	}
}
