using UnityEngine;

namespace DLS.Core
{
    [System.Serializable]
    public class BoolValue : IComparableValue<bool>
    {
        [SerializeField]
        protected string _name;
        public string Name { 
            get { return _name; } 
            set { _name = value; } 
        }
        [SerializeField]
        protected bool _value;
        public bool Value { 
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

        public BoolValue(string name, bool value)
        {
            Name = name;
            Value = value;
            SortOrder = value ? 1 : 0;
        }

        public int CompareTo(IComparableValue<bool> other)
        {
            return Value.CompareTo(other.Value);
        }

        public bool GreaterThan(IComparableValue<bool> other)
        {
            return SortOrder > other.SortOrder;
        }

        public bool LessThan(IComparableValue<bool> other)
        {
            return SortOrder < other.SortOrder;
        }

        public bool EqualTo(IComparableValue<bool> other)
        {
            return Value == other.Value;
        }

        public bool NotEqualTo(IComparableValue<bool> other)
        {
            return Value != other.Value;
        }

        public bool GreaterThanOrEqualTo(IComparableValue<bool> other)
        {
            return SortOrder >= other.SortOrder;
        }

        public bool LessThanOrEqualTo(IComparableValue<bool> other)
        {
            return SortOrder <= other.SortOrder;
        }
        public override string ToString()
        {
            return _value.ToString();
        }
    }
}