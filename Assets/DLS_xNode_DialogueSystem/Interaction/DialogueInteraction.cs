using System;
using UnityEngine;

namespace DLS.Dialogue
{
    [CreateAssetMenu(fileName = "DialogueInteraction.asset", menuName = "DLS/Dialogue/Dialogue Interaction", order = 20)]
    public class DialogueInteraction : ScriptableObject
    {
        [SerializeField]
        protected string dialogueId;
        [SerializeField]
        protected int interactionWeight;
        [SerializeField]
        protected string referenceState;
        [SerializeField]
        protected DialogueGraph graph;
        [SerializeField]
        protected bool dialogueCompleted;
        [SerializeField]
        protected bool repeatableDialogue;

        public string DialogueId { get => dialogueId; set => dialogueId = value; }
        public int InteractionWeight { get => interactionWeight; set => interactionWeight = value; }
    
        public string ReferencedState { get => referenceState; set => referenceState = value; }
    
        public DialogueGraph Graph { get => graph; set => graph = value; }

        public bool DialogueCompleted
        {
            get => dialogueCompleted; 
        
            set
            {
                // Check to ensure that repeatable dialogues cannot be set to a 'Completed' state.
                if (!repeatableDialogue)
                {
                    dialogueCompleted = value;
                }
            }
        }
    
        public bool RepeatableDialogue { get => repeatableDialogue; set => repeatableDialogue = value; }

        private void OnEnable()
        {
            Application.quitting += ApplicationOnquitting;
        }

        private void ApplicationOnquitting()
        {
            dialogueCompleted = false;
        }

        private void OnDisable()
        {
            Application.quitting -= ApplicationOnquitting;
        }

        public void StartDialogue()
        {
            DialogueUi.Instance.StartDialogue(graph);            
        }
    }
}