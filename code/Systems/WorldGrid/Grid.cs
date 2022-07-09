namespace Quest.Systems.WorldGrid;

public partial class Grid : Entity
{
	public Vector3[,] MapGrid;

	public const float StepX = 64f;
	public const float StepY = 64f;
	public const float MaxHeight = 1024f;

	public Grid()
	{
		// Get the bounds of the world.
		BBox worldBounds = Map.Physics.Body.GetBounds();
		float xWorld = worldBounds.Size.x;
		float yWorld = worldBounds.Size.y;

		// Get the size of the world as it will map to the grid.
		int gridSizeX = (int)(xWorld / StepX);
		int gridSizeY = (int)(yWorld / StepY);

		// Initialize 2D Array with the size of the grid (in tiles).
		MapGrid = new Vector3[gridSizeX, gridSizeY];

		for ( int i = 0; i < MapGrid.GetLength( 0 ); i++ )
		{
			for ( int j = 0; j < MapGrid.GetLength( 1 ); j++ )
			{
				float xPos = (i * StepX) - (xWorld / 2) + (StepX / 2);
				float yPos = (j * StepY) - (yWorld / 2) + (StepY / 2);

				var skyPos = new Vector3( xPos, yPos, MaxHeight );
				var tr = Trace.Ray( skyPos, skyPos + Vector3.Down * MaxHeight )
					.WorldOnly()
					.Run();

				float zPos = tr.EndPosition.z;

				Vector3 pos = new Vector3( xPos, yPos, zPos );
				MapGrid[i, j] = pos;
			}
		}
	}

	public Vector3 GetTilePosition( Vector3 position )
	{
		var i = (position.x + (Map.Physics.Body.GetBounds().Size.x / 2)) / StepX;
		var j = (position.y + (Map.Physics.Body.GetBounds().Size.y / 2)) / StepY;
		var newPos = MapGrid[i.FloorToInt(), j.FloorToInt()];
		return newPos;
	}
}
