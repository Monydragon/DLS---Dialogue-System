using UnityEngine;

namespace DLS.Core
{
    [System.Serializable]
    public class DoubleValue : IComparableValue<double>
    {
        [SerializeField]
        protected string _name;
        public string Name { 
            get { return _name; } 
            set { _name = value; } 
        }
        [SerializeField]
        protected double _value;
        public double Value { 
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

        public DoubleValue(string name, double value)
        {
            Name = name;
            Value = value;
            SortOrder = (int)value;
        }

        public int CompareTo(IComparableValue<double> other)
        {
            return Value.CompareTo(other.Value);
        }

        public bool GreaterThan(IComparableValue<double> other)
        {
            return Value > other.Value;
        }

        public bool LessThan(IComparableValue<double> other)
        {
            return Value < other.Value;
        }

        public bool EqualTo(IComparableValue<double> other)
        {
            return Value == other.Value;
        }

        public bool NotEqualTo(IComparableValue<double> other)
        {
            return Value != other.Value;
        }

        public bool GreaterThanOrEqualTo(IComparableValue<double> other)
        {
            return Value >= other.Value;
        }

        public bool LessThanOrEqualTo(IComparableValue<double> other)
        {
            return Value <= other.Value;
        }

        public override string ToString()
        {
            return _value.ToString();
        }
    }
}