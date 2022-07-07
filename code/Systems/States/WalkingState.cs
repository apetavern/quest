using Quest.Player;

namespace Quest.Systems.States;

public partial class WalkingState : State
{
	public override string ID => "state_walking";
	public override string Name => "Walking";

	[Net] public Vector3 Target { get; set; }

	public WalkingState() { }

	public WalkingState( Vector3 target, Entity owner )
	{
		Target = target;
		Owner = owner;
	}

	public override void Start()
	{
		var player = Owner as QuestPlayer;
		var controller = player.Controller as QuestPlayerController;

		controller.MoveTo( Target );
	}

	public override void Simulate()
	{
		DebugOverlay.ScreenText( Target.ToString(), 1 );

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
