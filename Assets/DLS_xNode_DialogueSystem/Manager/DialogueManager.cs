using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DLS.Dialogue
{
    [CreateAssetMenu(fileName = "New Dialogue Manager", menuName = "DLS/Dialogue/Dialogue Manager", order = 25)]
    public class DialogueManager : ScriptableObject
    {
        [SerializeField] 
        protected string currentReferenceState;
    
        [SerializeField]
        protected DialogueInteraction currentInteraction;
    
        [SerializeField]
        protected List<DialogueInteraction> interactions = new();

        [SerializeField] 
        protected bool useRandomDialogueSelectionByWeight = false;
    
        public string CurrentReferenceState { get => currentReferenceState; set => currentReferenceState = value; }
        public DialogueInteraction CurrentInteraction { get => currentInteraction; set => currentInteraction = value; }
    
        public List<DialogueInteraction> Interactions { get => interactions; set => interactions = value; }
        public bool UseRandomDialogueSelectionByWeight { get => useRandomDialogueSelectionByWeight; set => useRandomDialogueSelectionByWeight = value; }

        private void SetCurrentDialogue()
        {
            var matchingInteractions = interactions.FindAll(x => x.ReferencedState.Equals(currentReferenceState) && !x.DialogueCompleted).OrderByDescending(x=> x.InteractionWeight).ToList();
            if (matchingInteractions.Count > 1)
            {
                int totalWeight = 0;
                if (useRandomDialogueSelectionByWeight)
                {
                    for (int i = 0; i < matchingInteractions.Count; i++)
                    {
                        totalWeight += matchingInteractions[i].InteractionWeight;
                    }
                    int randomValue = Random.Range(0, totalWeight);
                
                    for (int i = 0; i < matchingInteractions.Count; i++)
                    {
                        if (randomValue < matchingInteractions[i].InteractionWeight)
                        {
                            currentInteraction = matchingInteractions[i];
                            return;
                        }
                        randomValue -= matchingInteractions[i].InteractionWeight;
                    }
                }
                else
                {
                    for (int i = 0; i < matchingInteractions.Count; i++)
                    {
                        currentInteraction = matchingInteractions[i];
                        return;;
                    }
                }
            
            }
            else
            {
                for (var i = 0; i < interactions.Count; i++)
                {
                    var interaction = interactions[i];
                    if (interaction.ReferencedState.Equals(currentReferenceState) && !interaction.DialogueCompleted)
                    {
                        currentInteraction = interaction;
                        return;
                    }
                    else
                    {
                        currentInteraction = interactions.FirstOrDefault(x => x.ReferencedState.Equals(string.Empty) && interaction.RepeatableDialogue);
                    }
                }
            }
        }

        public void StartDialogue()
        {
            SetCurrentDialogue();

            if (currentInteraction == null) { return; }
        
            currentInteraction.StartDialogue();
            currentInteraction.DialogueCompleted = true;
        }
    }
}
