using Quest.Systems.Interactions;

namespace Quest.UI;

[UseTemplate]
public class InteractionMenu : Panel
{
	public Label PrimaryAttackHintLabel { get; set; }
	public Panel InteractionMenuContainer { get; set; }
	public Label InteracteeLabel { get; set; }
	public Panel InteractionEntryContainer { get; set; }


	// List of Entities -> Multiple interactions for each entity
	// A full list of interactions must exist that is updated

	// public List<Entity> InteractableEntities { get; set; } = new();

	public InteractionMenu()
	{
		InteractionMenuContainer.AddEventListener( "onclick", () =>
		{
			InteractionMenuContainer.RemoveClass( "show" );
		} );
	}



	public override void Tick()
	{
		var trace = Trace.Ray( Input.Cursor.Origin, Input.Cursor.Origin + Input.Cursor.Direction * 100000f )
			.Radius( 5f )
			.Run();

		PrimaryAttackHintLabel.SetClass( "show", (trace.Entity is IInteractable) || (trace.Entity is WorldEntity) );

		if ( trace.Hit )
		{
			if ( trace.Entity is IInteractable interactable )
			{
				PrimaryAttackHintLabel.Text = interactable.GetInteractions().First().Name + " " + interactable.GetInteracteeName();
				InteracteeLabel.Text = "Choose Option"; //interactable.GetInteracteeName();
			}
			else if ( trace.Entity is WorldEntity )
			{
				PrimaryAttackHintLabel.Text = "Walk here";
			}

			if ( Input.Pressed( InputButton.SecondaryAttack ) )
			{
				InteractionMenuContainer.AddClass( "show" );
				InteractionMenuContainer.Style.Left = Mouse.Position.x;
				InteractionMenuContainer.Style.Top = Mouse.Position.y;
			}
		}

		/*foreach ( var trace in traces )
		{
			// If the trace hits, the entity is valid, and the list doesn't already contain the entity.
			if ( trace.Hit && trace.Entity is Entity entity && !InteractableEntities.Contains( trace.Entity ) )
			{
				InteractableEntities.Add( entity );
			}
		}*/
	}

	// Should this take in an IInteractable instead?
	/*	public void SetEntity( IInteractable interactableEntity )
		{
			InteractionsContainer.DeleteChildren();
			InteracteeLabel.Text = interactableEntity.GetInteracteeName();

			var interactions = interactableEntity.GetInteractions().Where( interaction => interaction.CanResolve );
			foreach ( Interaction interaction in interactions )
			{
				InteractionsContainer.Add.Label( interaction.Name, "interaction-entry" );
			}
		}*/
}
