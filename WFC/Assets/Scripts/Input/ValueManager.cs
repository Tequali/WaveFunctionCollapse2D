using System.Collections.Generic;
using UnityEngine;
using Helpers;
using System.Linq;

namespace WaveFunctionCollapse
{
    public class ValueManager<T>
    {
        int[][] _grid;
        Dictionary<int, IValue<T>> valueIndexDictionary = new Dictionary<int, IValue<T>>();
        int index = 0;

        public ValueManager(IValue<T>[][] gridOfValues)
        {
            CreateGridOfIndices(gridOfValues);
        }

        private void CreateGridOfIndices(IValue<T>[][] gridOfValues)
        {
            _grid = MyCollectionExtension.CreateJaggedArray<int[][]>(gridOfValues.Length, gridOfValues[0].Length);
            for (int col = 0; col < gridOfValues.Length; col++)
            {
                for (int row = 0; row < gridOfValues[0].Length; row++)
                {
                    SetIndexToGridPosition(gridOfValues, col, row);
                }
            }
        }

        #region Check/Set Values
        internal Vector2 GetGridSize()
        {
            if (_grid == null)
            {
                return Vector2.zero;
            }
            return new Vector2(_grid[0].Length, _grid.Length);
        }
        public int GetGridValue(int x, int y)
        {
            if(x >= _grid[0].Length || y >= _grid.Length || x < 0 || y < 0)
            {
                throw new System.IndexOutOfRangeException("Grid beinhaltet nicht x: " + x + " y: " + y + " value");
            }
            return _grid[x][y]; // maybe y/x not x/y
        }
        public IValue<T> GetValueFromIndex(int index)
        {
            if (valueIndexDictionary.ContainsKey(index))
            {
                return valueIndexDictionary[index];
            }
            throw new System.Exception("Index " + index + " in valueDictionary nicht gefunden");
        }
        public int GetGridValuesIncludingOffset(int x, int y)
        {
            int xMax = _grid.Length;
            int yMax = _grid[0].Length;
            if (x < 0 && y < 0)
            {
                return GetGridValue(xMax + x, yMax + y);
            }
            if (x < 0 && y >= yMax)
            {
                return GetGridValue(xMax + x, y - yMax);
            }
            if (x >= xMax && y < 0)
            {
                return GetGridValue(x - xMax, yMax + y);
            }
            if (x >= xMax && y >= yMax)
            {
                return GetGridValue(x - xMax, y - yMax);
            }

            if (x < 0)
            {
                return GetGridValue(xMax + x, y);
            }
            if (x >= xMax)
            {
                return GetGridValue(x - xMax, y);
            }
            if (y < 0)
            {
                return GetGridValue(x, yMax + y);
            }

            if (y >= yMax)
            {
                return GetGridValue(x, y - yMax);
            }
            return GetGridValue(x, y);
        }
        public int[][] GetPatternValuesFromGridAt(int x, int y, int patternSize)
        {
            int[][] arrayToReturn = MyCollectionExtension.CreateJaggedArray<int[][]>(patternSize, patternSize);
            for (int col = 0; col < patternSize; col++)
            {
                for (int row = 0; row < patternSize; row++)
                {
                    arrayToReturn[col][row] = GetGridValuesIncludingOffset(x + col, y + row);
                }
            }
            return arrayToReturn;
        }
        private void SetIndexToGridPosition(IValue<T>[][] gridOfValues, int col, int row)
        {
            if (valueIndexDictionary.ContainsValue(gridOfValues[col][row]))
            {
                var key = valueIndexDictionary.FirstOrDefault(x => x.Value.Equals(gridOfValues[col][row]));
                _grid[col][row] = key.Key;
            }
            else
            {
                _grid[col][row] = index;
                valueIndexDictionary.Add(_grid[col][row], gridOfValues[col][row]);
                index++;
            }
        }
        #endregion
    }
}