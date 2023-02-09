using UnityEngine;

namespace DLS.Core
{
    [System.Serializable]
    public class FloatValue : IComparableValue<float>
    {
        [SerializeField]
        protected string _name;
        public string Name { 
            get { return _name; } 
            set { _name = value; } 
        }
        [SerializeField]
        protected float _value;
        public float Value { 
            get { return _value; } 
            set { _value = value; } 
        }
        [SerializeField]
        protected int _sortOrder;
        public int SortOrder
        {
            get { return _sortOrder; }
            set { _sortOrder = value; }
        }

        public FloatValue(string name, float value)
        {
            Name = name;
            Value = value;
            SortOrder = (int)value;
        }

        public int CompareTo(IComparableValue<float> other)
        {
            return Value.CompareTo(other.Value);
        }

        public bool GreaterThan(IComparableValue<float> other)
        {
            return Value > other.Value;
        }

        public bool LessThan(IComparableValue<float> other)
        {
            return Value < other.Value;
        }

        public bool EqualTo(IComparableValue<float> other)
        {
            return Value == other.Value;
        }

        public bool NotEqualTo(IComparableValue<float> other)
        {
            return Value != other.Value;
        }

        public bool GreaterThanOrEqualTo(IComparableValue<float> other)
        {
            return Value >= other.Value;
        }

        public bool LessThanOrEqualTo(IComparableValue<float> other)
        {
            return Value <= other.Value;
        }
        public override string ToString()
        {
            return _value.ToString();
        }
    }
}