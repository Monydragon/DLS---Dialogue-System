using System.Collections.Generic;
using UnityEngine;

namespace DLS.Core
{
    [System.Serializable]
    public class VariableContainer<T>
    {
        [SerializeField]
        protected List<IComparableValue<T>> variables = new List<IComparableValue<T>>();

        public List<IComparableValue<T>> Variables
        {
            get => variables;
            set => variables = value;
        }

        public VariableContainer(List<IComparableValue<T>> variables)
        {
            this.variables = variables;
        }

        public IComparableValue<T> this[string variableName] => variables.Find(x => x.Name == variableName);

        public IComparableValue<T> GetValue(string variableName)
        {
            return variables.Find(x => x.Name == variableName);
        }
    }
}

