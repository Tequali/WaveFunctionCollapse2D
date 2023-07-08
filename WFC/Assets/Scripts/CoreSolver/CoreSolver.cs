using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WaveFunctionCollapse
{
    public class CoreSolver
    {
        PatternManager patternManager;
        OutputGrid outputGrid;
        CoreHelper coreHelper;
        PropogationHelper propogationHelper;

        public CoreSolver(OutputGrid outputGrid, PatternManager patternManager)
        {
            this.outputGrid = outputGrid;
            this.patternManager = patternManager;
            this.coreHelper = new CoreHelper(this.patternManager);
            this.propogationHelper = new PropogationHelper(this.outputGrid, this.coreHelper);
        }

        public void Propogate()
        {
            while (propogationHelper.PairsToPropogate.Count > 0)
            {
                var propogationPair = propogationHelper.PairsToPropogate.Dequeue();
                if (propogationHelper.CheckIfPairShouldBeProcessed(propogationPair))
                {
                    ProcessCell(propogationPair);
                }
                if (propogationHelper.CheckForConflicts() || outputGrid.CheckIfGridIsSolved())
                {
                    return;
                }
            }
            // sind in einer Sackgasse angelangt, müssen neu anfangen
            if (propogationHelper.CheckForConflicts() && propogationHelper.PairsToPropogate.Count == 0 && propogationHelper.LowEntropySet.Count == 0)
            {
                propogationHelper.SetConflictFlag();
            }
        }
        private void PropogateNeighbours(VectorPair propogationPair)
        {
            var possibleValuesAtNeighbour = outputGrid.GetPossibleValueForPosition(propogationPair.CellToPropogatePosition);
            int startCount = possibleValuesAtNeighbour.Count;
            RemoveImpossibleNeighbours(propogationPair, possibleValuesAtNeighbour);
            int newPossiblePatternCount = possibleValuesAtNeighbour.Count;
            propogationHelper.AnalysePropogationResults(propogationPair, startCount, newPossiblePatternCount);
        }
        private void ProcessCell(VectorPair propogationPair)
        {
            if (outputGrid.CheckIfCellIsCollapsed(propogationPair.CellToPropogatePosition))
            {
                propogationHelper.EnqueueUncollapseNeighbours(propogationPair);
            }
            else
            {
                PropogateNeighbours(propogationPair);
            }
        }

        private void RemoveImpossibleNeighbours(VectorPair propogationPair, HashSet<int> possibleValuesAtNeighbour)
        {
            HashSet<int> possibleIndices = new HashSet<int>();
            foreach (var patternIndexAtBase in outputGrid.GetPossibleValueForPosition(propogationPair.BaseCellPosition))
            {
                var possibleNeighboursForBase = patternManager.GetPossibleNeighboursForPatternInDirection(patternIndexAtBase, propogationPair.DirectionFromBase);
                possibleIndices.UnionWith(possibleNeighboursForBase);
            }
            possibleValuesAtNeighbour.IntersectWith(possibleIndices);
        }
        public Vector2Int GetLowestEntropyCell()
        {
            if (propogationHelper.LowEntropySet.Count <= 0)
            {
                return outputGrid.GetRandomCell();
            }
            else
            {
                var lowestEntropyElement = propogationHelper.LowEntropySet.First();
                Vector2Int returnVector = lowestEntropyElement.Position;
                propogationHelper.LowEntropySet.Remove(lowestEntropyElement);
                return returnVector;
            }
        }
        public void CollapseCell(Vector2Int cellCoordinates)
        {
            var possibleValues = outputGrid.GetPossibleValueForPosition(cellCoordinates).ToList();
            if (possibleValues.Count == 0 || possibleValues.Count == 1)
            {
                return;
            }
            else
            {
                int index = coreHelper.SelectSolutionPatternFromFrequency(possibleValues);
                outputGrid.SetPatternOnPosition(cellCoordinates.x, cellCoordinates.y, possibleValues[index]);
            }
            if (coreHelper.CheckCellSolutionForCollision(cellCoordinates, outputGrid) == false)
            {
                propogationHelper.AddNewPairsToPropogateQueue(cellCoordinates, cellCoordinates);
            }
            else
            {
                //  gab eine Collision, setze zurück
                propogationHelper.SetConflictFlag();
            }
        }

        #region Check if Collapsed or Conflicted
        public bool CheckIfSolved()
        {
            return outputGrid.CheckIfGridIsSolved();
        }
        public bool CheckForConflicts()
        {
            return propogationHelper.CheckForConflicts();
        }
        #endregion
    }
}