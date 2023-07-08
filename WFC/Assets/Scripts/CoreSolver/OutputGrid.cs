using Helpers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WaveFunctionCollapse
{
    public class OutputGrid
    {
        Dictionary<int, HashSet<int>> indexPossiblePatternDictionary = new Dictionary<int, HashSet<int>>();
        public int width { get; }
        public int height { get; }
        private int maxNumberOfPatterns = 0;

        public OutputGrid(int width, int height, int numberOfPatterns)
        {
            this.width = width;
            this.height = height;
            this.maxNumberOfPatterns = numberOfPatterns;
            ResetAllPossibilities();
        }

        public void ResetAllPossibilities()
        {
            HashSet<int> allPossiblePatternList = new HashSet<int>();
            allPossiblePatternList.UnionWith(Enumerable.Range(0, this.maxNumberOfPatterns).ToList());

            indexPossiblePatternDictionary.Clear();
            for (int i = 0; i < height * width; i++)
            {
                indexPossiblePatternDictionary.Add(i, new HashSet<int>(allPossiblePatternList));
            }
        }

        #region Check Values
        public bool CheckCellExists(Vector2Int position)
        {
            int index = GetIndexFromCoordinates(position);
            return indexPossiblePatternDictionary.ContainsKey(index);
        }
        public bool CheckIfCellIsCollapsed(Vector2Int position)
        {
            return GetPossibleValueForPosition(position).Count <= 1;
        }
        public bool CheckIfGridIsSolved()
        {
            return !indexPossiblePatternDictionary.Any(x => x.Value.Count > 1);
        }
        internal bool CheckIfValidPosition(Vector2Int position)
        {
            return MyCollectionExtension.ValidateCoordinates(position.x, position.y, width, height);
        }
        #endregion

        #region Get/Set Values
        public Vector2Int GetRandomCell()
        {
            int randmIndex = UnityEngine.Random.Range(0, indexPossiblePatternDictionary.Count);
            return GetCoordsFromIndex(randmIndex);
        }
        private int GetIndexFromCoordinates(Vector2Int position)
        {
            return position.x + width * position.y;
        }
        private Vector2Int GetCoordsFromIndex(int randmIndex)
        {
            Vector2Int coords = Vector2Int.zero;
            coords.x = randmIndex / this.width;
            coords.y = randmIndex % this.height;
            return coords;
        }
        public HashSet<int> GetPossibleValueForPosition(Vector2Int position)
        {
            int index = GetIndexFromCoordinates(position);
            if (indexPossiblePatternDictionary.ContainsKey(index))
            {
                return indexPossiblePatternDictionary[index];
            }
            return new HashSet<int>();
        }
        public int[][] GetSolvedOutputGrid()
        {
            int[][] returnGrid = MyCollectionExtension.CreateJaggedArray<int[][]>(this.height, this.width);
            if (CheckIfGridIsSolved() == false)
            {
                return MyCollectionExtension.CreateJaggedArray<int[][]>(0, 0);
            }
            for (int row = 0; row < this.height; row++)
            {
                for (int col = 0; col < this.width; col++)
                {
                    int index = GetIndexFromCoordinates(new Vector2Int(col, row));
                    returnGrid[row][col] = indexPossiblePatternDictionary[index].First();
                }
            }
            return returnGrid;
        }
        public void SetPatternOnPosition(int x, int y, int patternIndex)
        {
            int index = GetIndexFromCoordinates(new Vector2Int(x, y));
            indexPossiblePatternDictionary[index] = new HashSet<int>() { patternIndex };
        }
        #endregion
    }
}