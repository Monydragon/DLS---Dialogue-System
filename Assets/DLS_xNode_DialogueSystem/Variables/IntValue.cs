using UnityEngine;

namespace DLS.Core
{
    [System.Serializable]
    public class IntValue : IComparableValue<int>
    {
        [SerializeField]
        protected string _name;
        public string Name { 
            get { return _name; } 
            set { _name = value; } 
        }
        [SerializeField]
        protected int _value;
        public int Value { 
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

        public IntValue()
        {
        
        }

        public IntValue(string name, int value)
        {
            Name = name;
            Value = value;
            SortOrder = value;
        }

        public int CompareTo(IComparableValue<int> other)
        {
            return Value.CompareTo(other.Value);
        }

        public bool GreaterThan(IComparableValue<int> other)
        {
            return Value > other.Value;
        }

        public bool LessThan(IComparableValue<int> other)
        {
            return Value < other.Value;
        }

        public bool EqualTo(IComparableValue<int> other)
        {
            return Value == other.Value;
        }

        public bool NotEqualTo(IComparableValue<int> other)
        {
            return Value != other.Value;
        }

        public bool GreaterThanOrEqualTo(IComparableValue<int> other)
        {
            return Value >= other.Value;
        }

        public bool LessThanOrEqualTo(IComparableValue<int> other)
        {
            return Value <= other.Value;
        }

        public override string ToString()
        {
            return _value.ToString();
        }
    }
}