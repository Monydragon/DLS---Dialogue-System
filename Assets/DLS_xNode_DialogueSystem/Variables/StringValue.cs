using UnityEngine;

namespace DLS.Core
{
    [System.Serializable]
    public class StringValue : IComparableValue<string>
    {
        [SerializeField]
        protected string _name;
        public string Name { 
            get { return _name; } 
            set { _name = value; } 
        }
        [SerializeField]
        protected string _value;
        public string Value { 
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

        public StringValue(string name, string value)
        {
            Name = name;
            Value = value;
            SortOrder = value.GetHashCode();
        }

        public int CompareTo(IComparableValue<string> other)
        {
            return Value.CompareTo(other.Value);
        }

        public bool GreaterThan(IComparableValue<string> other)
        {
            return Value.CompareTo(other.Value) > 0;
        }

        public bool LessThan(IComparableValue<string> other)
        {
            return Value.CompareTo(other.Value) < 0;
        }

        public bool EqualTo(IComparableValue<string> other)
        {
            return Value == other.Value;
        }

        public bool NotEqualTo(IComparableValue<string> other)
        {
            return Value != other.Value;
        }

        public bool GreaterThanOrEqualTo(IComparableValue<string> other)
        {
            return Value.CompareTo(other.Value) >= 0;
        }

        public bool LessThanOrEqualTo(IComparableValue<string> other)
        {
            return Value.CompareTo(other.Value) <= 0;
        }
    
        public override string ToString()
        {
            return _value.ToString();
        }
    }
}