using Helpers;
using System.Collections.Generic;

namespace WaveFunctionCollapse
{
    public class PatternManager
    {
        Dictionary<int, PatternData> patternDataIndexDictionary;
        Dictionary<int, PatternNeighbours> patternPossibleNeighboursDictionary;
        int _patternSize = -1;
        IFindNeighbourStrategy strategy;
        public PatternManager(int patternSize)
        {
            _patternSize = patternSize;
        }

        public void ProcessGrid<T>(ValueManager<T> valueManager, bool equalWeights, string strategyName = null)
        {
            NeighbourStragetyFactory stragetyFactory = new NeighbourStragetyFactory();
            strategy = stragetyFactory.CreateInstance(strategyName == null ? _patternSize + "" : strategyName);
            CreatePatterns(valueManager, strategy, equalWeights);
        }

        internal int[][] ConvertPatternToValues<T>(int[][] outputValues)
        {
            int patternOutputWidth = outputValues.Length;
            int patternOutputHeight = outputValues[0].Length;
            int valueGridWith = patternOutputWidth + _patternSize - 1;
            int valueGridHeight = patternOutputHeight + _patternSize - 1;
            int[][] valueGrid = MyCollectionExtension.CreateJaggedArray<int[][]>(valueGridWith, valueGridHeight);
            for (int col = 0; col < patternOutputWidth; col++)
            {
                for (int row = 0; row < patternOutputHeight; row++)
                {
                    Pattern pattern = GetPatternDataFromIndex(outputValues[col][row]).Pattern;
                    GetPatternValues(patternOutputWidth, patternOutputHeight, valueGrid, col, row, pattern);
                }
            }
            return valueGrid;
        }

        private void CreatePatterns<T>(ValueManager<T> valueManager, IFindNeighbourStrategy strategy, bool equalWeights)
        {
            var patternFinderResult = PatternFinder.GetPatternDataFromGrid(valueManager, _patternSize, equalWeights);

            patternDataIndexDictionary = patternFinderResult.PatternIndexDictionary;
            GetPatternNeighbours(patternFinderResult, strategy);
        }
        
        #region Get Values
        private void GetPatternValues(int patternOutputWidth, int patternOutputHeight, int[][] valueGrid, int col, int row, Pattern pattern)
        {
            if (row == patternOutputHeight - 1 && col == patternOutputWidth - 1)
            {
                for (int col_1 = 0; col_1 < _patternSize; col_1++)
                {
                    for (int row_1 = 0; row_1 < _patternSize; row_1++)
                    {
                        valueGrid[col + col_1][row + row_1] = pattern.GetGridValue(col_1, row_1);
                    }
                }
            }
            else if (row == patternOutputHeight - 1)
            {
                for (int row_1 = 0; row_1 < _patternSize; row_1++)
                {
                    valueGrid[col][row + row_1] = pattern.GetGridValue(0, row_1);
                }
            }
            else if (col == patternOutputWidth - 1)
            {
                for (int col_1 = 0; col_1 < _patternSize; col_1++)
                {
                    valueGrid[col+col_1][row] = pattern.GetGridValue(col_1,0);
                }
            }
            else
            {
                valueGrid[col][row] = pattern.GetGridValue(0, 0);
            }

        }
        private void GetPatternNeighbours(PatternDataResults patternFinderResult, IFindNeighbourStrategy strategy)
        {
            patternPossibleNeighboursDictionary = PatternFinder.FindPossibleNeighboursForAllPatterns(strategy, patternFinderResult);
        }
        public PatternData GetPatternDataFromIndex(int index)
        {
            return patternDataIndexDictionary[index];
        }
        public HashSet<int> GetPossibleNeighboursForPatternInDirection(int patternIndex, Direction dir)
        {
            return patternPossibleNeighboursDictionary[patternIndex].GetNeighboursInDirection(dir);
        }

        public float GetPatternFrequency(int index)
        {
            return GetPatternDataFromIndex(index).RelativeFrequency;
        }

        public float GetPatternFrequencyLog2(int index)
        {
            return GetPatternDataFromIndex(index).RelativeFrequencyLog2;
        }
        public int GetNumberOfPatterns()
        {
            return patternDataIndexDictionary.Count;
        }
        #endregion
    }
}