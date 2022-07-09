global using Sandbox;
global using Sandbox.UI;
global using Sandbox.UI.Construct;
global using SandboxEditor;

global using System;
global using System.Collections.Generic;
global using System.Linq;

using Quest.Player;
using Quest.Systems.WorldGrid;
using Quest.UI;

namespace Quest;

public partial class Game : Sandbox.Game
{
	public static new Game Current => Sandbox.Game.Current as Game;
	public Grid MapGrid { get; set; }
	public QuestHud Hud { get; set; }

	public Game()
	{
		if ( Host.IsClient )
		{
			Hud = new QuestHud();
			MapGrid = new Grid();
		}


	}

	public override void ClientJoined( Client client )
	{
		string joinMessage = $"{client.Name} has joined Quest.";
		Log.Info( joinMessage );
		ChatBox.AddInformation( To.Everyone, joinMessage );

		var questPlayer = new QuestPlayer( client );
		client.Pawn = questPlayer;

		questPlayer.Spawn();
	}

	public override void PostLevelLoaded()
	{
		base.PostLevelLoaded();

		MapGrid = new Grid();
	}
}
