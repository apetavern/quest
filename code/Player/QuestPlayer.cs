using Quest.Systems.Interactions;
using Quest.Systems.Inventory;
using Quest.Systems.Items.Mining;
using Quest.Systems.Skills;
using Quest.Systems.States;

namespace Quest.Player;

public partial class QuestPlayer : AnimatedEntity, IInteractable
{
	[Net, Predicted]
	public PawnController Controller { get; set; }

	[Net, Predicted]
	public PawnAnimator Animator { get; set; }

	[Net, Predicted]
	public BaseCarriable ActiveChild { get; set; }

	[Net, Predicted]
	public BaseCarriable LastActiveChild { get; set; }

	public CameraMode Camera
	{
		get => Components.Get<CameraMode>();
		set => Components.Add( value );
	}

	[BindComponent] public PlayerInventoryComponent Inventory { get; }
	[BindComponent] public PlayerSkillComponent Skills { get; }
	[BindComponent] public StateMachineComponent StateMachine { get; }

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

		SetModel( "models/player/citizen_quest.vmdl" );
		ClothingContainer.DressEntity( this );

		Components.Create<PlayerInventoryComponent>();
		Components.Create<PlayerSkillComponent>();
		Components.Create<StateMachineComponent>();

		Skills.PopulateSkills();
		StateMachine.Init();

		CreateHull();

		Camera = new QuestPlayerCamera();
		Controller = new QuestPlayerControllerSimple();
		Animator = new QuestPlayerAnimator();
	}

	public override void Simulate( Client cl )
	{
		base.Simulate( cl );

		SimulateActiveChild( cl, ActiveChild );
		Controller?.Simulate( cl, this, Animator );
		StateMachine.StateMachine?.Simulate();

		if ( Input.Pressed( InputButton.Jump ) )
		{

			if ( Host.IsServer )
			{
				var pickaxe = new Pickaxe();
				Inventory.AddItem( pickaxe );
				ActiveChild = pickaxe.GetCarriable();
				ActiveChild.OnCarryStart( this );
			}
		}
	}

	public virtual void CreateHull()
	{
		CollisionGroup = CollisionGroup.Player;
		AddCollisionLayer( CollisionLayer.Player );
		SetupPhysicsFromAABB( PhysicsMotionType.Dynamic, new Vector3( -16, -16, 0 ), new Vector3( 16, 16, 72 ) );

		MoveType = MoveType.MOVETYPE_WALK;
		EnableHitboxes = true;
	}

	public virtual void SimulateActiveChild( Client client, BaseCarriable child )
	{
		if ( LastActiveChild != child )
		{
			OnActiveChildChanged( LastActiveChild, child );
			LastActiveChild = child;
		}

		if ( !LastActiveChild.IsValid() )
			return;

		LastActiveChild.Simulate( client );
	}

	public virtual void OnActiveChildChanged( BaseCarriable previous, BaseCarriable next )
	{
		previous?.ActiveEnd( this, previous.Owner != this );
		next?.ActiveStart( this );
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
