using Quest.Systems.Interactions;

namespace Quest.UI;

[UseTemplate]
public class InteractionHint : Panel
{
	public Label PrimaryAttackHintLabel { get; set; }

	public InteractionHint()
	{

	}

	public override void Tick()
	{
		var trace = Trace.Ray( Input.Cursor.Origin, Input.Cursor.Origin + Input.Cursor.Direction * 100000f )
			.Radius( 5f )
			.Ignore( Local.Pawn )
			.Run();

		PrimaryAttackHintLabel.SetClass( "show", (trace.Entity is IInteractable) || (trace.Entity is WorldEntity) );

		if ( trace.Hit )
		{
			if ( trace.Entity is IInteractable interactable )
			{
				PrimaryAttackHintLabel.Text = interactable.GetInteractions().First().Name + " " + interactable.GetInteracteeName();
			}
			else if ( trace.Entity is WorldEntity )
			{
				PrimaryAttackHintLabel.Text = "Walk here";
			}

		}
	}
}
