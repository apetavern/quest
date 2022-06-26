using Quest.Systems.Interactions;

namespace Quest.UI;

[UseTemplate]
public class InteractionMenu : Panel
{
	public Panel InteractionMenuContainer { get; set; }
	public Label InteracteeLabel { get; set; }
	public Panel InteractionEntryContainer { get; set; }

	public InteractionMenu()
	{
		InteractionMenuContainer.AddEventListener( "onclick", () =>
		{
			InteractionMenuContainer.RemoveClass( "show" );
		} );
	}

	private void PopulateInteractionMenu( TraceResult[] traces )
	{
		InteractionEntryContainer.DeleteChildren();

		foreach ( var trace in traces )
		{
			if ( trace.Entity is not IInteractable interactable )
				continue;

			foreach ( var interaction in interactable.GetInteractions() )
			{
				if ( interaction.CanResolve )
				{
					string intText = $"{interaction.Name} {interactable.GetInteracteeName()}";

					Label label = InteractionEntryContainer.Add.Label( intText, "interaction-entry" );
					label.AddEventListener( "onclick", () =>
					{
						interaction.ClientResolve();
						InteractionEntryContainer.DeleteChildren();
						InteractionMenuContainer.RemoveClass( "show" );
					} );
				}
			}

		}
	}

	public override void Tick()
	{
		if ( Input.Pressed( InputButton.SecondaryAttack ) )
		{
			var traces = Trace.Ray( Input.Cursor.Origin, Input.Cursor.Origin + Input.Cursor.Direction * 100000f )
			.Radius( 5f )
			.Ignore( Local.Pawn )
			.RunAll();

			PopulateInteractionMenu( traces );
			if ( InteractionEntryContainer.ChildrenCount > 0 )
			{
				InteractionMenuContainer.AddClass( "show" );

				InteractionMenuContainer.Style.Left = ScaleFromScreen * Mouse.Position.x;
				InteractionMenuContainer.Style.Top = ScaleFromScreen * Mouse.Position.y;
			}
		}
	}
}
