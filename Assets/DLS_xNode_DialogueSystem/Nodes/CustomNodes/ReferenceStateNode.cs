using UnityEngine;
using XNode;

namespace DLS.Dialogue
{
    public class ReferenceStateNode : BaseNode
    {
        [Input] public Connection input;
        [Output] public Connection exitTrue;

        [SerializeField]
        protected string referenceState;
    
        public string ReferenceState { get => referenceState; set => referenceState = value; }
    
        public override object GetValue(NodePort port)
        {
            return null;
        }
    }
}