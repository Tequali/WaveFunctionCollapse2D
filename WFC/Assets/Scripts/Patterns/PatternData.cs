using UnityEngine;

namespace WaveFunctionCollapse
{
    public class PatternData
    {
        private Pattern _pattern;
        private int _frequency=1;
        private float _relativeFrequency;
        private float _relativeFrequencyLog2;

        #region accessors
        public Pattern Pattern { get => _pattern;  }
        public float RelativeFrequency { get => _relativeFrequency; }
        public float RelativeFrequencyLog2 { get => _relativeFrequencyLog2;  }
        #endregion

        #region constructor
        public PatternData(Pattern pattern)
        {
            _pattern = pattern;
        }
        #endregion

        public void AddToFrequency()
        {
            _frequency++;
        }

        public void CalculateRelativeFrequency(int total)
        {
            _relativeFrequency = (float)_frequency / total;
            _relativeFrequencyLog2 = Mathf.Log(_relativeFrequencyLog2, 2);
        }
        public bool CompareToGrid(Direction dir, PatternData data)
        {
            return _pattern.ComparePatternToAnotherPattern(dir, data.Pattern);
        }
    }
}