using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace WaveFunctionCollapse
{
    public class InputImageParameters
    {
        Vector2Int? bottomLeftTileCoords = null;
        Vector2Int? topRightTileCoords = null;
        BoundsInt inputTileMapBounds;
        TileBase[] inputTilemapArray;
        Queue<TileContainer> queueOfTiles = new Queue<TileContainer>();
        private int width = 0, height = 0;
        private Tilemap inputTilemap;


        #region Accessors
        public Queue<TileContainer> QueueOfTiles { get => queueOfTiles; set => queueOfTiles = value; }
        public int Width { get => width; }
        public int Height { get => height; }
        #endregion
        public InputImageParameters(Tilemap inputTilemap)
        {
            this.inputTilemap = inputTilemap;
            this.inputTileMapBounds = this.inputTilemap.cellBounds;
            this.inputTilemapArray = this.inputTilemap.GetTilesBlock(this.inputTileMapBounds);
            ExtractNonEmptyTiles();
            VerifyInputTiles();
        }

        //  Stellt sicher, dass es keine Ausreißer oder Lücken gab
        private void VerifyInputTiles()
        {
            if (topRightTileCoords == null || bottomLeftTileCoords == null)
            {
                throw new System.Exception("WFC: Input Tilemap is leer");
            }
            int minX = bottomLeftTileCoords.Value.x;
            int maxX = topRightTileCoords.Value.x;
            int minY = bottomLeftTileCoords.Value.y;
            int maxY = topRightTileCoords.Value.y;

            width = Math.Abs(maxX - minX) + 1;
            height = Math.Abs(maxY - minY) + 1;
            int tileCount = width * height;
            if(queueOfTiles.Any(tile => tile.X>maxX || tile.X <minX || tile.Y > maxY || tile.Y < minY))
            {
                throw new System.Exception("WFC: Tilemap soll gefüllte Tilemap sein");
            }
            if (queueOfTiles.Count != tileCount)
            {
                throw new System.Exception("WFC: Tilemap beinhaltet leere Felder");
            }
        }
        //  Stellt sicher, dass alle Stellen Inhalt haben mit denen man arbeiten kann
        private void ExtractNonEmptyTiles()
        {
            for (int col = 0; col < inputTileMapBounds.size.x; col++)
            {
                for (int row = 0; row < inputTileMapBounds.size.y; row++)
                {
                    int index = col + (row * inputTileMapBounds.size.x);

                    TileBase tile = inputTilemapArray[index];
                    if(bottomLeftTileCoords==null && tile != null)
                    {
                        bottomLeftTileCoords = new Vector2Int(col, row);
                    }
                    if(tile != null)
                    {
                        queueOfTiles.Enqueue(new TileContainer(tile, col, row));
                        topRightTileCoords = new Vector2Int(col, row);
                    }
                }
            }
        }
    }
}
