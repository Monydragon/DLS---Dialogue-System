using XNode;

namespace DLS.Dialogue
{
	public class ExitNode_NoLoop_toStart : BaseNode
	{
		[Input] public Connection entry;
		[Output] public Connection exit;

		public override object GetValue(NodePort port)
		{
			return null;
		}
	}
}
