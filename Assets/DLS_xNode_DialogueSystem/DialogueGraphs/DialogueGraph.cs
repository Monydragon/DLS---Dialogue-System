using UnityEngine;
using XNode;

namespace DLS.Dialogue
{
    /// <summary>
    /// This class represents a Dialogue Graph.
    /// </summary>
    [CreateAssetMenu(fileName = "New Dialogue Graph", menuName = "DLS/Dialogue/Dialogue Graph")]
    public class DialogueGraph : NodeGraph 
    {
        public BaseNode start;
        public BaseNode current; //very similar to function declaration
        public BaseNode initNode;

        public void Start(){
            start = initNode; //loops back to the start node
            current = initNode;
        }
    }
}