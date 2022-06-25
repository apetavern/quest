namespace Quest.Systems.Interactions;

public abstract partial class Interaction
{
	/// <summary>
	/// The owner of the interaction.
	/// </summary>
	public Entity Owner { get; set; }

	/// <summary>
	/// The machine-readable identifier of the interaction.
	/// </summary>
	public virtual string ID => "";

	/// <summary>
	/// The human-readable name of the interaction.
	/// </summary>
	public virtual string Name => "";

	/// <summary>
	/// Whether the interaction should be resolved serverside.
	/// </summary>
	public virtual bool ResolveOnServer => true;

	/// <summary>
	/// Whether the interaction can be resolved.
	/// </summary>
	public virtual bool CanResolve => true;

	/// <summary>
	/// Public method for resolving interaction clientside.
	/// </summary>
	public void ClientResolve()
	{
		OnClientResolve();
	}

	/// <summary>
	/// Public method for resolving interaction serverside.
	/// </summary>
	public void ServerResolve()
	{
		OnServerResolve();
	}

	/// <summary>
	/// Method called when interaction is resolved clientside.
	/// </summary>
	protected virtual void OnClientResolve() { }

	/// <summary>
	/// Method called when interaction is resolved serverside.
	/// </summary>
	protected virtual void OnServerResolve() { }

	[ConCmd.Server( "quest_interact" )]
	public static void TryServerResolve( int netIdent, string id )
	{
		var entity = Entity.FindByIndex( netIdent );
		if ( entity.IsValid() && entity is IInteractable interactable )
		{
			var options = interactable.GetInteractions();
			var interaction = options.FirstOrDefault( x => x.ID == id );

			if ( interaction is null )
				return;

			interaction.ServerResolve();
		}
	}
}
