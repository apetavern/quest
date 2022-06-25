using Quest.Systems.Interactions;

namespace Quest.Entities;

[HammerEntity]
[Library( "quest_ore_deposit" )]
[Title( "Ore Deposit" )]
[Category( "World Entities" )]
public class OreDeposit : ModelEntity, IInteractable
{
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
		SetModel( "models/rocka.vmdl" );
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
