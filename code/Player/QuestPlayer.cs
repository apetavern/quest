using Quest.Systems.Interactions;
using Quest.Systems.Inventory;
using Quest.Systems.Items.Mining;
using Quest.Systems.Skills;
using Quest.Systems.States;
using Quest.Systems.States.Machines;

namespace Quest.Player;

public partial class QuestPlayer : AnimatedEntity, IInteractable
{
	[Net, Predicted]
	public PawnController Controller { get; set; }

	[Net, Predicted]
	public PawnAnimator Animator { get; set; }

	[Net]
	public BaseCarriable ActiveChild { get; set; }

	[Net]
	public BaseCarriable LastActiveChild { get; set; }

	public CameraMode Camera
	{
		get => Components.Get<CameraMode>();
		set => Components.Add( value );
	}

	[BindComponent] public PlayerInventoryComponent Inventory { get; }
	[BindComponent] public PlayerSkillComponent Skills { get; }
	[Net] public StateMachine StateMachine { get; set; }

	public ClothingContainer ClothingContainer { get; set; } = new();

	[Net] TimeUntil TimeUntilNextMineAnim { get; set; } = 1f;

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
		Scale = 1.33f;
		ClothingContainer.DressEntity( this );

		Components.Create<PlayerInventoryComponent>();
		Components.Create<PlayerSkillComponent>();

		Skills.PopulateSkills();
		StateMachine = new IdleStateMachine();
		StateMachine.Start();

		CreateHull();

		Camera = new QuestPlayerCamera();
		Controller = new QuestPlayerController();
		Animator = new QuestPlayerAnimator();
	}

	public override void Simulate( Client cl )
	{
		base.Simulate( cl );

		SimulateActiveChild( cl, ActiveChild );
		Controller?.Simulate( cl, this, Animator );
		StateMachine?.Simulate();

		if ( StateMachine.ActiveState is MiningState && TimeUntilNextMineAnim <= 0f )
		{
			SetAnimParameter( "b_attack", true );
			TimeUntilNextMineAnim = 1f;
		}

		if ( Input.Pressed( InputButton.Jump ) )
		{
			if ( IsServer )
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

	public void ChangeStateMachine( StateMachine @new )
	{
		StateMachine?.Stop();
		StateMachine = @new;
		StateMachine.Owner = this;
		StateMachine?.Start();
	}
}
