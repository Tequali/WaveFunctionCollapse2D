using UnityEngine.Tilemaps;
using Helpers;

namespace WaveFunctionCollapse
{
    public class InputReader : IInputReader<TileBase>
    {
        private Tilemap _inputTilemap;

        public InputReader(Tilemap input)
        {
            _inputTilemap = input;
        }
        
        public IValue<TileBase>[][] ReadInputToGrid()
        {
            var grid = ReadInputTileMap();

            TileBaseValue[][] gridOfValues = null;
            if (grid != null)
            {
                gridOfValues = MyCollectionExtension.CreateJaggedArray<TileBaseValue[][]>(grid.Length, grid[0].Length);
                for (int col = 0; col < grid.Length; col++)
                {
                    for (int row = 0; row < grid[0].Length; row++)
                    {
                        gridOfValues[col][row] = new TileBaseValue(grid[col][row]);
                    }
                }
            }
            return gridOfValues;
        }

        private TileBase[][] ReadInputTileMap()
        {
            InputImageParameters imageParameters = new InputImageParameters(_inputTilemap);
            return CreateTileBaseGrid(imageParameters);
        }

        private TileBase[][] CreateTileBaseGrid(InputImageParameters imageParameters)
        {
            TileBase[][] gridOfInputTiles = null;
            gridOfInputTiles = MyCollectionExtension.CreateJaggedArray<TileBase[][]>(imageParameters.Height, imageParameters.Width);
            for (int col = 0; col < imageParameters.Width; col++)
            {
                for (int row = 0; row < imageParameters.Height; row++)
                {
                    gridOfInputTiles[col][row] = imageParameters.QueueOfTiles.Dequeue().Tile;
                }
            }
            return gridOfInputTiles;
        }
    }
}
