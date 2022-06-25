namespace Quest.Systems.Grid;

public class Grid
{
	public float[,] WorldGrid { get; set; }
	private int stepSize = 16;

	public Grid()
	{
		BBox worldBounds = Map.Physics.Body.GetBounds();
		WorldGrid = new float[(int)worldBounds.Size.x / stepSize, (int)worldBounds.Size.y / stepSize];

		for ( int i = 0; i < WorldGrid.GetLength( 0 ); i++ )
		{
			for ( int j = 0; j < WorldGrid.GetLength( 1 ); j++ )
			{
				WorldGrid[i, j] = 1f;
			}
		}
	}
}
