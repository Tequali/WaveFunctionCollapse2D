using System;
using System.Collections.Generic;

namespace WaveFunctionCollapse
{
    public class NeighbourStrategy2AndMore : IFindNeighbourStrategy
    {
        public Dictionary<int, PatternNeighbours> FindNeighbours(PatternDataResults patternFinderResult)
        {
            Dictionary<int, PatternNeighbours> result = new Dictionary<int, PatternNeighbours>();
            foreach (var patternDataToCheck in patternFinderResult.PatternIndexDictionary)
            {
                foreach (var possibleNeighboursForPattern in patternFinderResult.PatternIndexDictionary)
                {
                    FindNeighboursInAllDirections(result, patternDataToCheck, possibleNeighboursForPattern);
                }
            }
            return result;
        }

        private void FindNeighboursInAllDirections(Dictionary<int, PatternNeighbours> result, KeyValuePair<int, PatternData> patternDataToCheck, KeyValuePair<int, PatternData> possibleNeighboursForPattern)
        {
            foreach (Direction dir in Enum.GetValues(typeof(Direction)))
            {
                if (patternDataToCheck.Value.CompareToGrid(dir, possibleNeighboursForPattern.Value))
                {
                    if (result.ContainsKey(patternDataToCheck.Key) == false)
                    {
                        result.Add(patternDataToCheck.Key, new PatternNeighbours());
                    }
                    result[patternDataToCheck.Key].AddPatternToDictionary(dir, possibleNeighboursForPattern.Key);
                }
            }
        }
    }
}