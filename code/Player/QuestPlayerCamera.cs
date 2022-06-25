namespace Quest.Player;

public class QuestPlayerCamera : CameraMode
{
	private Angles orbitAngles;

	private float orbitDistance { get; set; } = 400f;
	private float targetOrbitDistance { get; set; } = 400f;
	private float wheelSpeed => 10f;
	private float sensitivity => 1.6f;

	public override void Update()
	{
		if ( Local.Pawn is not AnimatedEntity pawn )
			return;

		Position = pawn.Position;

		Position += Vector3.Up * (pawn.CollisionBounds.Center.z * pawn.Scale);
		Rotation = Rotation.From( orbitAngles );

		Position += Rotation.Backward * orbitDistance;
		FieldOfView = 70f;
	}

	public override void BuildInput( InputBuilder input )
	{
		if ( Local.Pawn is not AnimatedEntity pawn )
			return;

		var wheel = input.MouseWheel;
		if ( wheel != 0 )
		{
			targetOrbitDistance -= wheel * wheelSpeed;
			targetOrbitDistance = targetOrbitDistance.Clamp( 125, 500 );
		}

		orbitDistance = orbitDistance.LerpTo( targetOrbitDistance, Time.Delta * wheelSpeed );

		if ( Input.UsingController )
		{
			orbitAngles.yaw += input.AnalogLook.yaw;
			orbitAngles.pitch += input.AnalogLook.pitch;
			orbitAngles = orbitAngles.Normal;
			input.ViewAngles = orbitAngles.WithPitch( 0f );
		}
		else if ( input.Down( InputButton.Zoom ) )
		{
			orbitAngles.yaw += input.AnalogLook.yaw * sensitivity;
			orbitAngles.pitch += input.AnalogLook.pitch * sensitivity;
			orbitAngles = orbitAngles.Normal;
		}
		else
		{
			var direction = Screen.GetDirection( Mouse.Position, FieldOfView, Rotation, Screen.Size );
			var hitPos = PlaneIntersectionWithZ( Position, direction, pawn.EyePosition.z );
			input.ViewAngles = (hitPos - pawn.EyePosition).EulerAngles;
		}

		orbitAngles.pitch = orbitAngles.pitch.Clamp( 20, 80 );
		input.InputDirection = Rotation.From( orbitAngles.WithPitch( 0f ) ) * input.AnalogMove;
	}

	public static Vector3 PlaneIntersectionWithZ( Vector3 pos, Vector3 dir, float z )
	{
		float a = (z - pos.z) / dir.z;
		return new( dir.x * a + pos.x, dir.y * a + pos.y, z );
	}
}
