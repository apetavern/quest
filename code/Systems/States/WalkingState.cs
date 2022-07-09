using Quest.Player;

namespace Quest.Systems.States;

public enum WalkType
{
	ToPosition,
	ToEntity
}

public partial class WalkingState : State
{
	public override string ID => "state_walking";
	public override string Name => "Walking";

	[Net] public Entity Target { get; set; }
	[Net] public Vector3 TargetPosition { get; set; }
	[Net] public WalkType Type { get; set; }

	public WalkingState() { }

	public WalkingState( WalkType walkType, Vector3 target, Entity owner )
	{
		Type = walkType;
		TargetPosition = target;
		Owner = owner;
	}

	public WalkingState( WalkType walkType, Entity target, Entity owner )
	{
		Type = walkType;
		Target = target;
		Owner = owner;
	}

	public override void Start()
	{
		var player = Owner as QuestPlayer;
		var controller = player.Controller as QuestPlayerController;

		if ( Type == WalkType.ToEntity )
			controller.MoveTo( Target );
		else
			controller.MoveTo( TargetPosition );
	}

	public override void Simulate()
	{
		// DebugOverlay.ScreenText( Target.ToString(), 1 );

		var player = Owner as QuestPlayer;
		var controller = player.Controller as QuestPlayerController;

		if ( !controller.ShouldMove )
		{
			Done = true;
		}
	}

	public override void Stop()
	{

	}
}
