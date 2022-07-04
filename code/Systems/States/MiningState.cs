using Quest.Entities;
using Quest.Player;
using Quest.Systems.Items.Mining;

namespace Quest.Systems.States;

public partial class MiningState : State
{
	public override string ID => "state_mining";
	public override string Name => "Mining";

	[Net] public OreDeposit Target { get; set; }
	[Net] public TimeUntil MiningTime { get; set; }
	[Net] public bool ShouldMine { get; set; } = false;

	public MiningState() { }

	public MiningState( OreDeposit target, Entity owner )
	{
		Target = target;
		Owner = owner;
	}

	public override void Simulate()
	{
		DebugOverlay.ScreenText( Target.Name, 1 );
		var player = Owner as QuestPlayer;

		if ( MiningTime <= 0f && ShouldMine )
		{
			if ( Host.IsServer )
			{
				player.Inventory.AddItem( new Ore() );
				player.Skills.AddExperience( Skills.SkillType.skill_mining, 10 );
			}

			Done = true;
		}
	}

	public override void Start()
	{
		var player = Owner as QuestPlayer;

		if ( player.ActiveChild is not PickaxeCarriable )
		{
			Log.Info( Host.Name + " You don't have a pickaxe equipped." );
			Done = true;
			return;
		}
		else
		{
			ShouldMine = true;
			MiningTime = 3f;
		}
	}

	public override void Stop()
	{

	}
}
