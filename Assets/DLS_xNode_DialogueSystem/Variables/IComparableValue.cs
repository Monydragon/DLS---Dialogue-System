using System;

namespace DLS.Core
{
    public interface IComparableValue<T> : IComparable<IComparableValue<T>>
    {
        string Name { get; set; }
        T Value { get; set; }
        int SortOrder { get; set; }
        bool GreaterThan(IComparableValue<T> other);
        bool LessThan(IComparableValue<T> other);
        bool EqualTo(IComparableValue<T> other);
        bool NotEqualTo(IComparableValue<T> other);
        bool GreaterThanOrEqualTo(IComparableValue<T> other);
        bool LessThanOrEqualTo(IComparableValue<T> other);
    }
}
