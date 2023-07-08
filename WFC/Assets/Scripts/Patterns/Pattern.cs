using Helpers;
using System.Collections.Generic;

namespace WaveFunctionCollapse
{
    public class Pattern
    {
        private int _index;
        private int[][] _grid;

        public string HashIndex { get; set; }
        public int Index { get => _index; }

        public Pattern(int[][] grid, string hashCode, int index)
        {
            _grid = grid;
            HashIndex = hashCode;
            _index = index;
        }
        
        private void CreatePartOfGrid(int xmin, int xmax, int ymin, int ymax, int[][] gridPartToCompare)
        {
            List<int> tempList = new List<int>();
            for (int col = xmin; col < xmax; col++)
            {
                for (int row = ymin; row < ymax; row++)
                {
                    tempList.Add(_grid[col][row]);
                }
            }

            for (int i = 0; i < tempList.Count; i++)
            {
                int x = i % gridPartToCompare.Length;
                int y = i / gridPartToCompare.Length;
                gridPartToCompare[x][y] = tempList[i];
            }
        }
        
        #region Check Values
        internal bool ComparePatternToAnotherPattern(Direction dir, Pattern pattern)
        {
            int[][] myGrid = GetGridValuesInDirection(dir);
            int[][] otherGrid = pattern.GetGridValuesInDirection(dir.GetOppositeDirectionTo());

            for (int col = 0; col < myGrid.Length; col++)
            {
                for (int row = 0; row < myGrid[0].Length; row++)
                {
                    if (myGrid[col][row] != otherGrid[col][row])
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public bool CheckValueAtPosition(int x, int y, int value)
        {
            return value.Equals(GetGridValue(x, y));
        }
        #endregion

        #region Get/Set Values
        public int GetGridValue(int x,int y)
        {
            return _grid[y][x];
        }
        private int[][] GetGridValuesInDirection(Direction dir)
        {
            int[][] gridPartToCompare;
            switch (dir)
            {
                case Direction.Up:
                    gridPartToCompare = MyCollectionExtension.CreateJaggedArray<int[][]>(_grid.Length - 1, _grid.Length);
                    CreatePartOfGrid(0, _grid.Length, 1, _grid.Length, gridPartToCompare);
                    break;
                case Direction.Down:
                    gridPartToCompare = MyCollectionExtension.CreateJaggedArray<int[][]>(_grid.Length - 1, _grid.Length);
                    CreatePartOfGrid(0, _grid.Length, 0, _grid.Length - 1, gridPartToCompare);
                    break;
                case Direction.Left:
                    gridPartToCompare = MyCollectionExtension.CreateJaggedArray<int[][]>(_grid.Length, _grid.Length-1);
                    CreatePartOfGrid(0, _grid.Length - 1, 0, _grid.Length, gridPartToCompare);
                    break;
                case Direction.Right:
                    gridPartToCompare = MyCollectionExtension.CreateJaggedArray<int[][]>(_grid.Length, _grid.Length-1);
                    CreatePartOfGrid(1, _grid.Length, 0, _grid.Length, gridPartToCompare);
                    break;
                default:
                    return _grid;
            }
            return gridPartToCompare;
        }
        public void SetGridValue(int x, int y, int value)
        {
            _grid[y][x] = value;
        }
        #endregion
    }
}