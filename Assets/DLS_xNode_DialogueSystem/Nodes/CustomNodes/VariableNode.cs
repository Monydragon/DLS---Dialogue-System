using DLS.Core;
using UnityEngine;
using XNode;

namespace DLS.Dialogue
{
    public class VariableNode : BaseNode
    {
        [Input] public Connection input;
        [Output] public Connection exitTrue;
        [Output] public Connection exitFalse;

        [SerializeField]
        protected string variableName;

        [SerializeField] 
        protected VariableType variableType;
    
        [SerializeField]
        protected string variableValue;
    
        [SerializeField]
        protected Operator operatorType;

        public string VariableName { get => variableName; set => variableName = value; }
        public VariableType VariableType { get => variableType; set => variableType = value; }
        public string VariableValue { get => variableValue; set => variableValue = value; }
        public Operator OperatorType { get => operatorType; set => operatorType = value; }

        public override object GetValue(NodePort port)
        {
            return null;
        }
    }
}