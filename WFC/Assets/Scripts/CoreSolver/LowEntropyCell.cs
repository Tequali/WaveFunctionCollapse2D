using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WaveFunctionCollapse
{
    public class LowEntropyCell : IComparable<LowEntropyCell>, IEqualityComparer<LowEntropyCell>
    {
        #region Properties
        public Vector2Int Position { get; set; }
        public float Entropy { get; set; }
        public float smallEntropyNoise { get; set; }
        #endregion
        public LowEntropyCell(Vector2Int position, float entropy)
        {
            smallEntropyNoise = UnityEngine.Random.Range(0.001f, 0.005f);
            this.Entropy = entropy + smallEntropyNoise;
            this.Position = position;
        }
        #region Vergleiche
        public int CompareTo(LowEntropyCell other)
        {
            //  ist gr��er
            if (Entropy > other.Entropy) return 1;
            //  ist kleiner
            else if (Entropy < other.Entropy) return -1;
            //  sind qleich
            else return 0;
        }

        public bool Equals(LowEntropyCell cell1, LowEntropyCell cell2)
        {
            return cell1.Position.x == cell2.Position.x && cell1.Position.y == cell2.Position.y;
        }
        #endregion
        public int GetHashCode(LowEntropyCell obj)
        {
            return obj.GetHashCode();
        }
        public override int GetHashCode()
        {
            return Position.GetHashCode();
        }
    }
}