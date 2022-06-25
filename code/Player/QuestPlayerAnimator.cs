namespace Quest.Player;

public class QuestPlayerAnimator : PawnAnimator
{
	TimeSince TimeSinceFootShuffle = 60;

	public override void Simulate()
	{
		var idealRotation = Rotation.LookAt( Input.Rotation.Forward.WithZ( 0 ), Vector3.Up );

		DoRotation( idealRotation );
		DoWalk();

		//
		// Let the animation graph know some shit
		//
		bool sitting = HasTag( "sitting" );
		bool noclip = HasTag( "noclip" ) && !sitting;

		SetAnimParameter( "b_grounded", GroundEntity != null || noclip || sitting );
		SetAnimParameter( "b_noclip", noclip );
		SetAnimParameter( "b_sit", sitting );

		if ( Host.IsClient && Client.IsValid() )
		{
			SetAnimParameter( "voice", Client.TimeSinceLastVoice < 0.5f ? Client.VoiceLevel : 0.0f );
		}
	}

	public virtual void DoRotation( Rotation idealRotation )
	{
		float turnSpeed = 0.01f;

		//
		// If we're moving, rotate to our ideal rotation
		//
		Rotation = Rotation.Slerp( Rotation, idealRotation, WishVelocity.Length * Time.Delta * turnSpeed );

		SetAnimParameter( "b_shuffle", TimeSinceFootShuffle < 0.1 );
	}

	void DoWalk()
	{
		// Move Speed
		{
			var dir = Velocity;
			var forward = Rotation.Forward.Dot( dir );
			var sideward = Rotation.Right.Dot( dir );

			var angle = MathF.Atan2( sideward, forward ).RadianToDegree().NormalizeDegrees();

			SetAnimParameter( "move_direction", angle );
			SetAnimParameter( "move_speed", Velocity.Length );
			SetAnimParameter( "move_groundspeed", Velocity.WithZ( 0 ).Length );
			SetAnimParameter( "move_y", sideward );
			SetAnimParameter( "move_x", forward );
			SetAnimParameter( "move_z", Velocity.z );
		}

		// Wish Speed
		{
			var dir = WishVelocity;
			var forward = Rotation.Forward.Dot( dir );
			var sideward = Rotation.Right.Dot( dir );

			var angle = MathF.Atan2( sideward, forward ).RadianToDegree().NormalizeDegrees();

			SetAnimParameter( "wish_direction", angle );
			SetAnimParameter( "wish_speed", WishVelocity.Length );
			SetAnimParameter( "wish_groundspeed", WishVelocity.WithZ( 0 ).Length );
			SetAnimParameter( "wish_y", sideward );
			SetAnimParameter( "wish_x", forward );
			SetAnimParameter( "wish_z", WishVelocity.z );
		}
	}

	public override void OnEvent( string name )
	{
		// DebugOverlay.Text( Pos + Vector3.Up * 100, name, 5.0f );

		if ( name == "jump" )
		{
			Trigger( "b_jump" );
		}

		base.OnEvent( name );
	}
}
