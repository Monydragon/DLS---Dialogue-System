using XNode;

namespace DLS.Dialogue
{
	public abstract class CustomNode : BaseNode{

		[Input] public Connection input;
		[Output] public Connection exit;

		public override object GetValue(NodePort port)
		{
			return null;
		}
	}
}
