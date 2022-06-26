﻿namespace Quest.Player;

public partial class QuestPlayerControllerSimple : BasePlayerController
{
	[Net, Predicted] public float WalkSpeed { get; set; } = 150f;
	[Net] public float RunSpeed { get; set; } = 250f;

	public static float GroundDistance => 0.25f;

	[Net, Predicted] public Vector3 TargetPosition { get; set; }
	[Net, Predicted] public bool ShouldMove { get; set; }

	private static Vector3 mins = new Vector3( -16, -16, 0 );
	private static Vector3 maxs = new Vector3( 16, 16, 72 );

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

			MoveHelper move = new( Position, Velocity );
			move.TryMove( Time.Delta );
			move.Velocity = vel;
			move.MaxStandableAngle = 50f;
			move.Trace = move.Trace.Ignore( Pawn ).Size( mins, maxs );

			Position = move.Position;
			Velocity = move.Velocity;
		}
		else
		{
			Velocity = 0f;
		}

	}

	[ConCmd.Server( name: "quest_test" )]
	public static void MoveIt()
	{
		var player = Entity.All.OfType<QuestPlayer>().First();
		player.Position = new Vector3( 0, 0, 50f );
	}
}
