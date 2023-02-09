using UnityEngine;
using XNode;

namespace DLS.Dialogue
{
    public class ChoiceDialogueNode : BaseNode
    {
        [Input] public Connection input;
        [Output(dynamicPortList = true)] public string[] Answers;
        [SerializeField]
        protected string actorName;
        [SerializeField]
        protected Sprite sprite;
        [SerializeField,TextArea] 
        protected string dialogueText;

        public string ActorName { get => actorName; set => actorName = value; }
        public Sprite Sprite { get => sprite; set => sprite = value; }
        public string DialogueText { get => dialogueText; set => dialogueText = value; }

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
