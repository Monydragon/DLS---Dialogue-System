using XNode;

namespace DLS.Dialogue
{
    public class StartNode : BaseNode
    {

        [Output] public Connection exit;

        public override object GetValue(NodePort port)
        {
            return null;
        }


    }
}