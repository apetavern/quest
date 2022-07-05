using Quest.Systems.Interactions;

namespace Quest.Entities;

[HammerEntity]
[Library( "quest_ore_deposit" )]
[Title( "Copper Ore Deposit" )]
[Category( "World Entities" )]
[EditorModel( COPPER_OREVEIN_MODEL_PATH )]
public partial class OreDeposit : ModelEntity, IInteractable
{
	[Net] public bool Depleted { get; set; } = false;
	[Net] public TimeSince TimeSinceDepleted { get; set; }

	private const string COPPER_OREVEIN_MODEL_PATH = "models/resources/copper_orevein/copper_orevein.vmdl";

	public string GetInteracteeName()
	{
		return "Ore Deposit";
	}

	public IEnumerable<Interaction> GetInteractions()
	{
		return new List<Interaction>()
		{
			new MineInteraction( this ),
			new ExamineInteraction( this )
		};
	}

	public override void Spawn()
	{
		base.Spawn();

		CreateHull();
		SetModel( COPPER_OREVEIN_MODEL_PATH );
		Rotation = new Angles( 0, Rand.Float( 0, 360 ), 0 ).ToRotation();
	}

	[Event.Tick]
	public void Tick()
	{
		if ( Depleted && TimeSinceDepleted > 8f )
		{
			Depleted = false;
		}

		if ( Depleted && Model.Name == COPPER_OREVEIN_MODEL_PATH )
		{
			// SetModel to depleted, or destroy?
		}
		else
		{
			SetModel( COPPER_OREVEIN_MODEL_PATH );
		}
	}

	public override void StartTouch( Entity other )
	{
		base.StartTouch( other );
		Log.Info( $"{other.Client.Name} has touched {this}" );
	}

	public virtual void CreateHull()
	{
		CollisionGroup = CollisionGroup.Prop;
		AddCollisionLayer( CollisionLayer.Solid );
		SetupPhysicsFromAABB( PhysicsMotionType.Static, new Vector3( -32, -32, 0 ), new Vector3( 32, 32, 64 ) );

		EnableHitboxes = true;
	}

	public string GetExamineText()
	{
		return "A rocky outcrop.";
	}
}
