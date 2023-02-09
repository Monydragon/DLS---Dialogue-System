using System.Collections.Generic;
using UnityEngine;

namespace DLS.Core
{
    [CreateAssetMenu(fileName = "New Variables", menuName = "DLS/Variables", order = 0)]
    public class VariablesObject : ScriptableObject
    {
        [SerializeField] 
        protected VariableContainer<int> intVariables = new VariableContainer<int>(new List<IComparableValue<int>>());
        [SerializeField] 
        protected VariableContainer<double> doubleVariables = new VariableContainer<double>(new List<IComparableValue<double>>());
        [SerializeField] 
        protected VariableContainer<float> floatVariables = new VariableContainer<float>(new List<IComparableValue<float>>());
        [SerializeField] 
        protected VariableContainer<bool> boolVariables = new VariableContainer<bool>(new List<IComparableValue<bool>>());
        [SerializeField] 
        protected VariableContainer<string> stringVariables = new VariableContainer<string>(new List<IComparableValue<string>>());

        public VariableContainer<int> IntVariables
        {
            get => intVariables;
            set => intVariables = value;
        }
        public VariableContainer<double> DoubleVariables
        {
            get => doubleVariables;
            set => doubleVariables = value;
        }

        public VariableContainer<float> FloatVariables
        {
            get => floatVariables;
            set => floatVariables = value;
        }

        public VariableContainer<bool> BoolVariables
        {
            get => boolVariables;
            set => boolVariables = value;
        }

        public VariableContainer<string> StringVariables
        {
            get => stringVariables;
            set => stringVariables = value;
        }
    }
}
