using Quest.Systems.Interactions;

namespace Quest.Player;

public partial class QuestPlayer : AnimatedEntity, IInteractable
{
	[Net, Predicted]
	public PawnController Controller { get; set; }

	[Net, Predicted]
	public PawnAnimator Animator { get; set; }

	public CameraMode Camera
	{
		get => Components.Get<CameraMode>();
		set => Components.Add( value );
	}

	public ClothingContainer ClothingContainer { get; set; } = new();

	public QuestPlayer()
	{

	}

	public QuestPlayer( Client client ) : this()
	{
		ClothingContainer.LoadFromClient( client );
	}

	public override void Spawn()
	{
		base.Spawn();
		SetModel( "models/citizen/citizen.vmdl" );

		ClothingContainer.DressEntity( this );

		CreateHull();

		Camera = new QuestPlayerCamera();
		Controller = new QuestPlayerControllerSimple();
		Animator = new QuestPlayerAnimator();
	}

	public override void Simulate( Client cl )
	{
		base.Simulate( cl );

		Controller?.Simulate( cl, this, Animator );
	}

	public override void BuildInput( InputBuilder inputBuilder )
	{
		base.BuildInput( inputBuilder );

		Controller?.BuildInput( inputBuilder );
	}

	public virtual void CreateHull()
	{
		CollisionGroup = CollisionGroup.Player;
		AddCollisionLayer( CollisionLayer.Player );
		SetupPhysicsFromAABB( PhysicsMotionType.Dynamic, new Vector3( -16, -16, 0 ), new Vector3( 16, 16, 72 ) );

		MoveType = MoveType.MOVETYPE_WALK;
		EnableHitboxes = true;
	}

	public override void FrameSimulate( Client cl )
	{
		base.FrameSimulate( cl );

		Controller?.Simulate( cl, this, Animator );
	}

	public IEnumerable<Interaction> GetInteractions()
	{
		return new List<Interaction>()
		{
			new ExamineInteraction( this )
		};
	}

	public string GetInteracteeName()
	{
		return Client.Name;
	}

	public string GetExamineText()
	{
		return $"That {Client.Name} fella seems like an alright guy.";
	}
}
