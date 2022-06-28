using Quest.Player;
using Quest.Systems.Items.Mining;

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

	public override bool ResolveOnServer => true;

	protected override void OnServerResolve()
	{
		var player = Caller.Pawn as QuestPlayer;
		var controller = player.Controller as QuestPlayerControllerSimple;

		controller.MoveTo( Owner );
		player.Inventory.AddItem( new Ore() );
	}

	protected override void OnClientResolve()
	{
		ChatBox.Say( "Mine Interaction" );
	}
}
