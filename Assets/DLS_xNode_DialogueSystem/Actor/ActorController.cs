using System;
using DLS.Dialogue;
using UnityEngine;

namespace DLS.Core
{
    public abstract class ActorController : MonoBehaviour, IActorData
    {
        public static event Action<GameObject, GameObject> OnDialogueInteract;

        [SerializeField] 
        protected Guid id;
        [SerializeField]
        protected string actorName;

        [SerializeField] 
        protected Sprite portrait;
        [SerializeField] 
        protected string portraitPath;

        [SerializeField] 
        protected bool isInteracting;
        
        [SerializeField] 
        protected bool isMovementDisabled;
    
        [SerializeField]
        protected DialogueManager dialogueManager;

        protected GameObject targetGameObject;
    
        public Guid ID { get => id; set => id = value; }
        public GameObject GameObject { get; }

        public string ActorName { get => actorName; set => actorName = value; }
        
        public bool IsInteracting { get => isInteracting; set => isInteracting = value; }
        
        public bool IsMovementDisabled { get => isMovementDisabled; set => isMovementDisabled = value; }

        public Sprite Portrait { get => portrait; set => portrait = value; }

        public string PortraitPath { get => portraitPath; set => portraitPath = value; }
        public DialogueManager DialogueManager { get => dialogueManager; set => dialogueManager = value; }
        public GameObject TargetGameObject { get => targetGameObject; set => targetGameObject = value; }

        protected virtual void OnEnable()
        {
            OnDialogueInteract += OnOnDialogueInteract;
            DialogueUi.OnDialogueEnded += DialogueUiOnOnDialogueEnded;
        }



        protected virtual void OnDisable()
        {
            OnDialogueInteract -= OnOnDialogueInteract;
            DialogueUi.OnDialogueEnded -= DialogueUiOnOnDialogueEnded;

        }

        public virtual void Interact()
        {
            if (targetGameObject != null)
            {
                OnDialogueInteract?.Invoke(gameObject, targetGameObject);
            }
        }
    
        protected virtual void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.GetComponent<IActorData>() == null) { return; }
            targetGameObject = col.gameObject;
        }

        protected virtual void OnTriggerExit2D(Collider2D col)
        {
            if (targetGameObject != col.gameObject) { return; }
            targetGameObject = null;
        }
    
        protected virtual void OnOnDialogueInteract(GameObject source, GameObject target)
        {
            if (gameObject == target)
            {
                if (dialogueManager != null)
                {
                    if (!isInteracting)
                    {
                        dialogueManager.StartDialogue();
                        isInteracting = true;
                        isMovementDisabled = true;
                        DialogueUi.Instance.HideInteractionText();
                    }
                }
            }
        }
        
        protected virtual void DialogueUiOnOnDialogueEnded()
        {
            isInteracting = false;
            isMovementDisabled = false;
        }
    }
}