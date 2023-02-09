using System;
using DLS.Dialogue;
using UnityEngine;

namespace DLS.Core
{
    /// <summary>
    /// This interface represents the actor data.
    /// </summary>
    public interface IActorData
    {
        /// <summary>
        /// The GUID of the Actor.
        /// </summary>
        Guid ID { get; set; }

        /// <summary>
        /// The Gameobject for the Actor.
        /// </summary>
        GameObject GameObject { get; }
    
        GameObject TargetGameObject { get; set; }
        
        bool IsInteracting { get; set; }
        
        bool IsMovementDisabled { get; set; }
        
        /// <summary>
        /// The name of the Actor.
        /// </summary>
        string ActorName { get; set; }

        /// <summary>
        /// A Sprite image of the Actor.
        /// </summary>
        Sprite Portrait { get; set; }

        /// <summary>
        /// The Path for the portrait sprite for the Actor.
        /// </summary>
        string PortraitPath { get; set; }

        /// <summary>
        /// The Dialogue for the Actor.
        /// </summary>
        DialogueManager DialogueManager { get; set; }

        void Interact();
    }
}