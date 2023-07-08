using System;
using System.Collections.Generic;
using UnityEngine;
using Helpers;

namespace WaveFunctionCollapse
{
    public static class PatternFinder
    {
        private static void CalculateRelativeFrequency(Dictionary<int, PatternData> patternIndexDictionary, int totalFrequency)
        {
            foreach (var item in patternIndexDictionary.Values)
            {
                item.CalculateRelativeFrequency(totalFrequency);
            }
        }
        
        #region Add Patterns and Neighbours to Dictionaries
        private static void AddNewPattern(Dictionary<string, PatternData> patternHashcodeDictionary, Dictionary<int, PatternData> patternIndexDictionary, string hashValue, Pattern pattern)
        {
            PatternData data = new PatternData(pattern);
            patternHashcodeDictionary.Add(hashValue, data);
            patternIndexDictionary.Add(pattern.Index, data);
        }
        public static void AddNeighboursToDictionary(Dictionary<int,PatternNeighbours> dictionary, int patternIndex, PatternNeighbours patternNeighbours)
        {
            if(dictionary.ContainsKey(patternIndex) == false)
            {
                dictionary.Add(patternIndex, patternNeighbours);
            }
            dictionary[patternIndex].AddNeighbour(patternNeighbours);
        }
        #endregion

        internal static Dictionary<int, PatternNeighbours> FindPossibleNeighboursForAllPatterns(IFindNeighbourStrategy strategy, PatternDataResults patternFinderResult)
        {
            return strategy.FindNeighbours(patternFinderResult);
        }
        public static PatternNeighbours CheckNeighboursInEachDirection(int x, int y, PatternDataResults patternDataResults)
        {
            PatternNeighbours patternNeighbours = new PatternNeighbours();
            foreach(Direction dir in Enum.GetValues(typeof(Direction)))
            {
                int possiblePatternIndex = patternDataResults.GetNeighbourInDirection(x, y, dir);
                if (possiblePatternIndex >= 0)
                {
                    patternNeighbours.AddPatternToDictionary(dir, possiblePatternIndex);
                }
            }
            return patternNeighbours;
        }
        internal static PatternDataResults GetPatternDataFromGrid<T>(ValueManager<T> valueManager, int patternSize, bool equalWeights)
        {
            Dictionary<string, PatternData> patternHashcodeDictionary = new Dictionary<string, PatternData>();
            Dictionary<int, PatternData> patternIndexDictionary = new Dictionary<int, PatternData>();
            Vector2 sizeOfGrid = valueManager.GetGridSize();
            int patternGridSizeX = 0, patternGridSizeY = 0;
            int colMin = -1, rowMin = -1, colMax = -1, rowMax = -1;
            if (patternSize < 3)
            {
                patternGridSizeX = (int)sizeOfGrid.x + 3 - patternSize;
                patternGridSizeY = (int)sizeOfGrid.y + 3 - patternSize;
                colMax = patternGridSizeX - 1;
                rowMax = patternGridSizeY - 1;
            }
            else
            {
                patternGridSizeX = (int)sizeOfGrid.x + patternSize - 1;
                patternGridSizeY = (int)sizeOfGrid.y + patternSize - 1;
                colMin = 1 - patternSize;
                rowMin = 1 - patternSize;
                colMax = (int)sizeOfGrid.x;
                rowMax = (int)sizeOfGrid.y;
            }
            int[][] patternIndicesGrid = MyCollectionExtension.CreateJaggedArray<int[][]>(patternGridSizeY,patternGridSizeX);
            int totalFrequency = 0, patternIndex = 0;
            for (int col = colMin; col < colMax; col++)
            {
                for (int row = rowMin; row < rowMax; row++)
                {
                    int[][] gridValues = valueManager.GetPatternValuesFromGridAt(col, row, patternSize);
                    string hashValue = HashCodeCalculator.CalculateHashCode(gridValues);

                    if (patternHashcodeDictionary.ContainsKey(hashValue) == false)
                    {
                        Pattern pattern = new Pattern(gridValues, hashValue, patternIndex);
                        patternIndex++;
                        AddNewPattern(patternHashcodeDictionary, patternIndexDictionary, hashValue, pattern);
                    }
                    else
                    {
                        if (equalWeights == false)
                        {
                            patternIndexDictionary[patternHashcodeDictionary[hashValue].Pattern.Index].AddToFrequency();
                        }
                    }
                    totalFrequency++;
                    if (patternSize < 3)
                    {
                                          //vllt wieder austauschen zu row dann coll
                        patternIndicesGrid[col + 1][row + 1] = patternHashcodeDictionary[hashValue].Pattern.Index;
                    }
                    else
                    {
                        patternIndicesGrid[col + patternSize - 1][row + patternSize - 1] = patternHashcodeDictionary[hashValue].Pattern.Index;
                    }
                }
            }
            CalculateRelativeFrequency(patternIndexDictionary, totalFrequency);
            return new PatternDataResults(patternIndicesGrid, patternIndexDictionary);
        }
    }
}