using UnityEngine;

namespace WaveFunctionCollapse
{
    public class VectorPair
    {
        public Vector2Int BaseCellPosition { get; set; }
        public Vector2Int CellToPropogatePosition { get; set; }
        public Vector2Int PreviousCellPosition { get; set; }
        public Direction DirectionFromBase { get; set; }
        public VectorPair(Vector2Int baseCellPosition, Vector2Int cellToPropagatePosition, Direction directionFromBase, Vector2Int previousCellPosition)
        {
            BaseCellPosition = baseCellPosition;
            CellToPropogatePosition = cellToPropagatePosition;
            DirectionFromBase = directionFromBase;
            PreviousCellPosition = previousCellPosition;
        }

        public bool AreWeCheckingPreviousCellAgain()
        {
            return PreviousCellPosition == CellToPropogatePosition;
        }
    }
}