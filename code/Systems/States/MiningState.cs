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


	TimeSince TimeSinceLastHit { get; set; }

	bool DoJiggle;

	Vector3 startPosition;

	Rotation startRotation;

	public MiningState() { }

	public MiningState( OreDeposit target, Entity owner )
	{
		Target = target;
		Owner = owner;
		startPosition = target.Position;
		startRotation = target.Rotation;
	}

	public override void Simulate()
	{
		DebugOverlay.ScreenText( Target.Name, 1 );
		var player = Owner as QuestPlayer;


		bool shouldJiggle = MathF.Round( TimeSinceLastHit * 10f ) / 10f == 0.3f;

		if ( Host.IsServer && ShouldMine && shouldJiggle )
		{
			float RFloat = Rand.Float( -2f, 2f );
			Target.Position += new Vector3( RFloat, RFloat, 0f );
			Target.Rotation += new Angles( RFloat * 0.1f, 0, RFloat * 0.1f ).ToRotation();
		}

		if ( Host.IsServer && ShouldMine && TimeSinceLastHit > 1f )
		{
			TimeSinceLastHit = !DoJiggle ? -1f : 0f; //Has to wait a second extra because of animation start delay
			DoJiggle = true;
		}

		if ( Host.IsServer && ShouldMine )
		{
			Target.Position = Vector3.Lerp( Target.Position, startPosition, 0.5f );
			Target.Rotation = Rotation.Lerp( Target.Rotation, startRotation, 0.5f );
		}

		if ( MiningTime <= 0f && ShouldMine )
		{
			if ( Host.IsServer )
			{
				player.Inventory.AddItem( new Ore() );
				player.Skills.AddExperience( Skills.SkillType.skill_mining, 10 );
				DoJiggle = false;
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
}
