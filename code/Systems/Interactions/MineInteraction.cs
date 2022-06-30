﻿using Quest.Player;
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

		controller.MoveTo( Owner as Entity );
		player.Inventory.AddItem( new Ore() );
		player.Skills.AddExperience( "skill_mining", 10 );
	}
}
