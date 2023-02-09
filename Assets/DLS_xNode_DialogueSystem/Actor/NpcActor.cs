using DLS.Dialogue;
using UnityEngine;

namespace DLS.Core
{
    public class NpcActor : ActorController
    {
        protected override void OnTriggerEnter2D(Collider2D col)
        {
            base.OnTriggerEnter2D(col);
            if (col.CompareTag("Player"))
            {
                DialogueUi.Instance.ShowInteractionText("Press E to Interact");
            }
        }
    
        protected override void OnTriggerExit2D(Collider2D col)
        {
            base.OnTriggerExit2D(col);
            if (col.CompareTag("Player"))
            {
                DialogueUi.Instance.HideInteractionText();
            }
        }
    
    }
}