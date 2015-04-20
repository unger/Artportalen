namespace Artportalen.Sample.Kustobsar.Models
{
    using System;

    public class SortField<T, TProperty>
    {
        public SortField(Func<T, TProperty> propertyFunc, bool descending = false)
        {
            this.PropertyFunc = propertyFunc;
            this.Descending = descending;
        }

        public Func<T, TProperty> PropertyFunc { get; set; }

        public bool Descending { get; set; }
    }
}