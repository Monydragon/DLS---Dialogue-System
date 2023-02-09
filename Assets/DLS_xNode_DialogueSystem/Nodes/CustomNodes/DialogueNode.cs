using UnityEngine;
using XNode;

namespace DLS.Dialogue
{
	public class DialogueNode : BaseNode
	{
		[Input] public Connection input;
		[Output] public Connection exit;
		[SerializeField]
		protected string actorName;
		[SerializeField,TextArea] 
		protected string dialogueText;
		[SerializeField]
		protected Sprite sprite;

		public string ActorName { get => actorName; set => actorName = value; }
		public string DialogueText { get => dialogueText; set => dialogueText = value; }
		public Sprite Sprite { get => sprite; set => sprite = value; }

		public override object GetValue(NodePort port)
		{
			return null;
		}

		public override Sprite GetSprite()
		{
			return sprite;
		}
	}
}