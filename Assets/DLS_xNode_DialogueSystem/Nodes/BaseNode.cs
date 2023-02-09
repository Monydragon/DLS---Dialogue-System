using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using DLS.Core;
using UnityEngine;
using UnityEngine.Scripting;
using XNode;

namespace DLS.Dialogue
{
    /// <summary>
    /// This abstract class is the base node for all other nodes.
    /// </summary>
    [Preserve]
    public abstract class BaseNode : Node
    {
        [SerializeField] 
        protected VariablesObject variables;
        protected GameObject sourceGameobject;
        protected GameObject targetGameobject;
        protected IActorData sourceActor;
        protected IActorData targetActor;

        public VariablesObject Variables { get => variables; set => variables = value; }
        public GameObject SourceGameobject { get => sourceGameobject; set => sourceGameobject = value; }
        public GameObject TargetGameobject { get => targetGameobject; set => targetGameobject = value; }
        public IActorData SourceActor { get => sourceActor; set => sourceActor = value; }
        public IActorData TargetActor { get => targetActor; set => targetActor = value; }

        /// <summary>
        /// This method returns back the string for the node.
        /// </summary>
        /// <returns></returns>
        public virtual string GetString()
        {
            return null;
        }

        /// <summary>
        /// This method returns back the sprite for the node.
        /// </summary>
        /// <returns></returns>
        public virtual Sprite GetSprite()
        {
            return null;
        }

        /// <summary>
        /// This method returns back the node type as a string.
        /// </summary>
        /// <returns></returns>
        public virtual string GetNodeType(){ 
            return this.GetType().Name;
        }
    
        /// <summary>
        /// This method returns the source gameobject.
        /// </summary>
        /// <returns></returns>
        public virtual GameObject GetSourceGameobject()
        {
            return sourceGameobject;
        }

        /// <summary>
        /// This method returns the target gameobject.
        /// </summary>
        /// <returns></returns>
        public virtual GameObject GetTargetGameobject()
        {
            return targetGameobject;
        }

        /// <summary>
        /// This method returns the target <see cref="IActorData"/>
        /// </summary>
        /// <returns></returns>
        public virtual IActorData GetTargetActor()
        {
            return targetActor;
        }

        /// <summary>
        /// This method returns the source <see cref="IActorData"/>
        /// </summary>
        /// <returns></returns>
        public virtual IActorData GetSourceActor()
        {
            return sourceActor;
        }

        /// <summary>
        /// This OnEnable method assigns <see cref="ActorController_onDialogueInteract(GameObject, GameObject)"/> 
        /// </summary>
        protected override void OnEnable()
        {
            base.OnEnable();
            ActorController.OnDialogueInteract += ActorController_onDialogueInteract;

        }
        /// <summary>
        /// This OnDisable method unassigns <see cref="ActorController_onDialogueInteract(GameObject, GameObject)"/> 
        /// </summary>
        protected virtual void OnDisable()
        {
            ActorController.OnDialogueInteract -= ActorController_onDialogueInteract;
        }

        /// <summary>
        /// This method is responsible for parsing through text provided it will parse everything inside { } it will then match properties and their values as strings in place of the text input.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>

        public virtual string ParseVariable(string text)
        {
            try
            {
                var pattern = @"\{([^}]+)\}";
                var matches = Regex.Matches(text, pattern);

                //iterate through matches in reverse so as not to change the match index
                foreach (Match match in matches.Reverse())
                {
                    string property = "";
                    //remove curly braces
                    string value = match.Value.Substring(1, match.Value.Length - 2);
                    //split
                    string[] s = value.Split('.');

                    //use reflection to find fields of matching names
                    //assuming source starts with a property on this object
                    object currentObject = this;
                    for (int i = 0; i < s.Length; i++)
                    {
                        // Check if current segment is an indexer
                        if (s[i].Contains("["))
                        {
                            // Get the parameter for the indexer
                            var parameter = s[i].Substring(s[i].IndexOf("[") + 2, s[i].IndexOf("]") - s[i].IndexOf("[") - 3);
                            // Get the indexer property
                            if (currentObject.GetType().GetProperty("Variables") != null)
                            {
                                PropertyInfo field = currentObject.GetType().GetProperty("Variables");
                                currentObject = field.GetValue(currentObject, null);
                                // Check the type of the variable being referenced
                                if (field.PropertyType == typeof(List<IComparableValue<int>>))
                                {
                                    var list = (List<IComparableValue<int>>)currentObject;

                                    var variableFound = list.FirstOrDefault(x => x.Name == parameter);
                                    if (variableFound != null)
                                    {
                                        currentObject = variableFound;
                                    }
                                    else
                                    {
                                        Debug.LogWarning($"Failed to find variable of type for {parameter}");
                                        currentObject = $"Variable {parameter} Not Found";
                                    }
                                }
                                else if (field.PropertyType == typeof(List<IComparableValue<double>>))
                                {
                                    var list = (List<IComparableValue<double>>)currentObject;

                                    var variableFound = list.FirstOrDefault(x => x.Name == parameter);
                                    if (variableFound != null)
                                    {
                                        currentObject = variableFound;
                                    }
                                    else
                                    {
                                        Debug.LogWarning($"Failed to find variable of type for {parameter}");
                                        currentObject = $"Variable {parameter} Not Found";
                                    }
                                }
                                else if (field.PropertyType == typeof(List<IComparableValue<float>>))
                                {
                                    var list = (List<IComparableValue<float>>)currentObject;

                                    var variableFound = list.FirstOrDefault(x => x.Name == parameter);
                                    if (variableFound != null)
                                    {
                                        currentObject = variableFound;
                                    }
                                    else
                                    {
                                        Debug.LogWarning($"Failed to find variable of type for {parameter}");
                                        currentObject = $"Variable {parameter} Not Found";
                                    }
                                }
                                else if (field.PropertyType == typeof(List<IComparableValue<bool>>))
                                {
                                    var list = (List<IComparableValue<bool>>)currentObject;

                                    var variableFound = list.FirstOrDefault(x => x.Name == parameter);
                                    if (variableFound != null)
                                    {
                                        currentObject = variableFound;
                                    }
                                    else
                                    {
                                        Debug.LogWarning($"Failed to find variable of type for {parameter}");
                                        currentObject = $"Variable {parameter} Not Found";
                                    }
                                }
                                else if (field.PropertyType == typeof(List<IComparableValue<string>>))
                                {
                                    var list = (List<IComparableValue<string>>)currentObject;

                                    var variableFound = list.FirstOrDefault(x => x.Name == parameter);
                                    if (variableFound != null)
                                    {
                                        currentObject = variableFound;
                                    }
                                    else
                                    {
                                        Debug.LogWarning($"Failed to find variable of type for {parameter}");
                                        currentObject = $"Variable {parameter} Not Found";
                                    }
                                }
                            }
                            else
                            {
                                Debug.LogWarning("Failed to find indexer or indexer does not have any parameters");
                                currentObject = $"Indexer {parameter} not found or does not have any parameters";
                            }
                        }
                        else
                        {
                            // Get the next property or method
                            if (currentObject.GetType().GetProperty(s[i]) != null)
                            {
                                PropertyInfo field = currentObject.GetType().GetProperty(s[i]);
                                currentObject = field.GetValue(currentObject, null);
                            }
                            else
                            {
                                MethodInfo field = currentObject.GetType().GetMethod(s[i]);
                                currentObject = field.Invoke(currentObject, null);
                            }
                        }
                    }

                    //Cast to string
                    property = currentObject.ToString();
                    //remove the old text + insert new
                    // since we are processing the matches in reverse, this shouldn't mess up indexes
                    text = text.Remove(match.Index, match.Length);
                    text = text.Insert(match.Index, property);
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"Failed to parse text: {text}\n {ex.Message}");
            }

            return text;
        }


        /// <summary>
        /// This method assigns the source and target GameObjects and <see cref="IActorData"/>'s for the base node.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        protected virtual void ActorController_onDialogueInteract(GameObject source, GameObject target)
        {
            var sourceActor = source.GetComponent<IActorData>();
            var targetActor = target.GetComponent<IActorData>();
            if (sourceActor != null)
            {
                this.sourceActor = sourceActor;
            }
            if(targetActor != null)
            {
                this.targetActor = targetActor;
            }
            sourceGameobject = source;
            targetGameobject = target;
        }
    }
}