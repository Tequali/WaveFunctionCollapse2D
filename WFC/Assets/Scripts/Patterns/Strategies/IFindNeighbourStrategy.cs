using System.Collections.Generic;

namespace WaveFunctionCollapse
{
    public interface IFindNeighbourStrategy
    {
        Dictionary<int, PatternNeighbours> FindNeighbours(PatternDataResults patternFinderResult);
    }
}