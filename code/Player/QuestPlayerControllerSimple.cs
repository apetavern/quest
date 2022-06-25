namespace Quest.Player;

public partial class QuestPlayerControllerSimple : BasePlayerController
{
	[Net] public float WalkSpeed { get; set; } = 150f;
	[Net] public float RunSpeed { get; set; } = 250f;

	public static float GroundDistance { get; set; } = 0.25f;

	[Net, Predicted] public Vector3 TargetPosition { get; set; }
	[Net] public bool ShouldMove { get; set; }

	public QuestPlayerControllerSimple()
	{

	}

	public override void FrameSimulate()
	{
		base.FrameSimulate();
	}

	public override void Simulate()
	{
		base.Simulate();

		GetInput();

		WishVelocity = new Vector3( Input.Forward, Input.Left, 0 );
		var inSpeed = WishVelocity.Length.Clamp( 0, 1 );
		WishVelocity *= Input.Rotation.Angles().WithPitch( 0 ).ToRotation();
		WishVelocity = WishVelocity.WithZ( 0 );
		WishVelocity = WishVelocity.Normal * inSpeed;
		WishVelocity *= 1;

		// Ensure our Player is touching the ground.
		TouchGrass();

		// Allow our Player to move.
		Move();
	}

	public void GetInput()
	{
		if ( Input.Pressed( InputButton.PrimaryAttack ) )
		{
			var trace = Trace.Ray( Input.Cursor.Origin, Input.Cursor.Origin + Input.Cursor.Direction * 100000f )
			.Radius( 5f )
			.Run();

			if ( trace.Entity is WorldEntity )
			{
				var endPosition = trace.EndPosition;

				DebugOverlay.Line( endPosition, endPosition + Vector3.Up * 25, 5f );
				TargetPosition = endPosition;
				ShouldMove = true;
			}
		}
	}

	private void TouchGrass()
	{
		Vector3 point = new Vector3( Position ).WithZ( Position.z - GroundDistance );
		TraceResult trace = TraceBBox( Position, point );
		GroundEntity = trace.Entity;

		if ( GroundEntity is null )
		{
			Position = Position.WithZ( Position.z - GroundDistance );
		}
	}

	private void Move()
	{
		if ( Position.AlmostEqual( TargetPosition, 10f ) )
		{
			ShouldMove = false;
		}

		if ( ShouldMove )
		{
			var vel = (TargetPosition - Position).WithZ( 0 ).Normal * WalkSpeed;
			Rotation = Rotation.Lerp( Rotation, Rotation.LookAt( vel ), 64f );

			MoveHelper move = new MoveHelper( Position, vel );
			move.TryMove( Time.Delta );

			Position = move.Position;
			Velocity = move.Velocity;
		}
		else
		{
			Velocity = 0;
		}

	}

	[ConCmd.Server( name: "quest_test" )]
	public static void MoveIt()
	{
		var player = Entity.All.OfType<QuestPlayer>().First();
		player.Position = new Vector3( 0, 0, 50f );
	}
}
