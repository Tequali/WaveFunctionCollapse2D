using UnityEngine;
using UnityEngine.Tilemaps;

namespace WaveFunctionCollapse
{
    public class TileMapOutput : IOutputCreator<Tilemap>
    {
        private Tilemap outputImage;
        private ValueManager<TileBase> valueManager;
        public Tilemap OutputImage => outputImage;

        public TileMapOutput(ValueManager<TileBase> valueManager, Tilemap outputImage)
        {
            this.outputImage = outputImage;
            this.valueManager = valueManager;
        }

        public void CreateOutput(PatternManager manager, int[][] outputvalues, int width, int height)
        {
            if (outputvalues.Length == 0)
            {
                return;
            }
            this.outputImage.ClearAllTiles();

            int[][] valueGrid;
            valueGrid = manager.ConvertPatternToValues<TileBase>(outputvalues);

            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    TileBase tile = this.valueManager.GetValueFromIndex(valueGrid[col][row]).Value;
                    this.outputImage.SetTile(new Vector3Int(col, row, 0), tile);
                }
            }
        }
    }
}