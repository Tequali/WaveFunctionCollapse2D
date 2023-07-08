using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WaveFunctionCollapse
{
    public class PropogationHelper
    {
        OutputGrid outputGrid;
        CoreHelper coreHelper;
        bool cellWithNoSolutionPresent;
        SortedSet<LowEntropyCell> lowEntropySet = new SortedSet<LowEntropyCell>();
        Queue<VectorPair> pairsToPropogate = new Queue<VectorPair>();

        public SortedSet<LowEntropyCell> LowEntropySet { get => lowEntropySet; }
        public Queue<VectorPair> PairsToPropogate { get => pairsToPropogate; }
        public PropogationHelper(OutputGrid outputGrid, CoreHelper coreHelper)
        {
            this.outputGrid = outputGrid;
            this.coreHelper = coreHelper;
        }
        public void AnalysePropogationResults(VectorPair propogationPair, int startCount, int newPossiblePatternCount)
        {
            if (newPossiblePatternCount > 1 && startCount > newPossiblePatternCount)
            {
                AddNewPairsToPropogateQueue(propogationPair.CellToPropogatePosition, propogationPair.BaseCellPosition);
                AddToLowEntropySet(propogationPair.CellToPropogatePosition);
            }
            if (newPossiblePatternCount == 0)
            {
                cellWithNoSolutionPresent = true;
            }
            if (newPossiblePatternCount == 1)
            {
                cellWithNoSolutionPresent = coreHelper.CheckCellSolutionForCollision(propogationPair.CellToPropogatePosition, outputGrid);
            }
        }

        #region Add to sets or Queues
        internal void EnqueueUncollapseNeighbours(VectorPair propogationPair)
        {
            var uncollapsedNeighbours = coreHelper.CheckIfNeighboursAreCollapsed(propogationPair, outputGrid);
            foreach(var uncollapsed in uncollapsedNeighbours)
            {
                pairsToPropogate.Enqueue(uncollapsed);
            }
        }
        private void AddToLowEntropySet(Vector2Int cellToPropogatePosition)
        {
            var elementOfLowEntropySet = lowEntropySet.Where(x => x.Position == cellToPropogatePosition).FirstOrDefault();
            if (elementOfLowEntropySet == null && outputGrid.CheckIfCellIsCollapsed(cellToPropogatePosition) == false)
            {
                float entropy = coreHelper.CalculateEntropy(cellToPropogatePosition, outputGrid);
                lowEntropySet.Add(new LowEntropyCell(cellToPropogatePosition, entropy));
            }
            else
            {
                lowEntropySet.Remove(elementOfLowEntropySet);
                elementOfLowEntropySet.Entropy = coreHelper.CalculateEntropy(cellToPropogatePosition, outputGrid);
                lowEntropySet.Add(elementOfLowEntropySet);
            }
        }
        public void AddNewPairsToPropogateQueue(Vector2Int cellToPropogatePosition, Vector2Int baseCellPosition)
        {
            var list = coreHelper.Create4DirectionNeighbours(cellToPropogatePosition, baseCellPosition);
            foreach (var item in list)
            {
                pairsToPropogate.Enqueue(item);
            }
        }
#endregion

        #region Check if conflicted or to be processed
        public bool CheckIfPairShouldBeProcessed(VectorPair propogationPair)
        {
            return outputGrid.CheckIfValidPosition(propogationPair.CellToPropogatePosition) && propogationPair.AreWeCheckingPreviousCellAgain() == false;
        }
        public bool CheckForConflicts()
        {
            return cellWithNoSolutionPresent;
        }
        #endregion
        public void SetConflictFlag()
        {
            cellWithNoSolutionPresent = true;
        }
    }
}