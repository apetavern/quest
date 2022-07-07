using Quest.Systems.Interactions;

namespace Quest.UI.Interactions;

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

						if ( interaction.ResolveOnServer )
						{
							Interaction.TryServerResolve( interaction.Owner.NetworkIdent, interaction.ID );
						}

						InteractionEntryContainer.DeleteChildren();
						InteractionMenuContainer.RemoveClass( "show" );
					} );
				}
			}

		}
	}

	public override void Tick()
	{
		CheckPanelHover();

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

				var xPos = Mouse.Position.x;
				var yPos = Mouse.Position.y;

				if ( xPos + InteractionEntryContainer.Box.Rect.width > Screen.Width )
				{
					xPos -= InteractionEntryContainer.Box.Rect.width;
				}

				if ( yPos + InteractionEntryContainer.Box.Rect.height > Screen.Height )
				{
					yPos -= InteractionMenuContainer.Box.Rect.height * 2;
				}

				InteractionMenuContainer.Style.Left = xPos * ScaleFromScreen;
				InteractionMenuContainer.Style.Top = yPos * ScaleFromScreen;
			}
		}
	}

	private void CheckPanelHover()
	{
		var c = InteractionMenuContainer.Box.Rect;

		var xPos = c.Position.x;
		var yPos = c.Position.y;
		var minBoundary = new Vector2( xPos, yPos );
		var maxBoundary = new Vector2( xPos + c.Size.x, yPos + c.Size.y );

		bool mouseInBoundary = (
			Mouse.Position.x > minBoundary.x
			&& Mouse.Position.y > minBoundary.y
			&& Mouse.Position.x < maxBoundary.x
			&& Mouse.Position.y < maxBoundary.y);
		InteractionMenuContainer.SetClass( "pointer-events", mouseInBoundary );
	}
}
