namespace Quest.Systems.Interactions;

public partial class MineInteraction : Interaction
{
	public override string ID => "mine_interaction";
	public override string Name => "Mine";

	public MineInteraction( Entity entity )
	{
		Owner = entity;
	}

	public override bool CanResolve => true;

	public override bool ResolveOnServer => false;

	protected override void OnClientResolve()
	{
		ChatBox.Say( "Mine Interaction" );
	}
}
