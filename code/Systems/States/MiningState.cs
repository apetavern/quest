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

	private TimeSince TimeSinceLastHit { get; set; }

	private bool doJiggle;
	private Rotation initialRot;

	public MiningState() { }

	public MiningState( OreDeposit target, Entity owner )
	{
		Target = target;
		Owner = owner;

		// Store initial rotation of the Ore Deposit.
		initialRot = target.Rotation;
	}

	public override void Simulate()
	{
		DebugOverlay.ScreenText( Target.Name, 1 );
		var player = Owner as QuestPlayer;

		OnHitEffects();

		if ( MiningTime <= 0f && ShouldMine )
		{
			if ( Host.IsServer )
			{
				player.Inventory.AddItem( new Ore() );
				player.Skills.AddExperience( Skills.SkillType.skill_mining, 10 );

				Target.Depleted = true;
				Target.TimeSinceDepleted = 0f;
				doJiggle = false;
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
			MiningTime = 3.5f;
		}
	}

	public override void Stop()
	{

	}

	private void OnHitEffects()
	{
		if ( Host.IsServer )
		{
			bool shouldJiggle = (0.3f <= TimeSinceLastHit && TimeSinceLastHit < 0.4f);

			if ( ShouldMine && shouldJiggle )
			{
				Target.Rotation = Target.Rotation.RotateAroundAxis( Vector3.Up, 5f );
			}

			if ( ShouldMine && TimeSinceLastHit > 1f )
			{
				// Wait an extra second due to animation start delay.
				TimeSinceLastHit = !doJiggle ? -1f : 0f;
				doJiggle = true;
			}

			if ( ShouldMine )
			{
				Target.Rotation = Rotation.Lerp( Target.Rotation, initialRot, 0.5f );
			}
		}
	}
}
