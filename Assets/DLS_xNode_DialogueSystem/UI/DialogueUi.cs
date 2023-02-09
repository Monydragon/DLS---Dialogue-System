using System;
using System.Collections;
using DLS.Core;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XNode;

namespace DLS.Dialogue
{
    public class DialogueUi : MonoBehaviour
    {
        public static event Action OnDialogueEnded;
    
        public static DialogueUi Instance { get; private set; }

        [SerializeField]
        private float typewriterTextDelayTime = 0.05f;
        [SerializeField]
        private TMP_Text actorNameText, dialogueText, interactionText;
        [SerializeField]
        private Image actorImage;
        [SerializeField]
        private GameObject dialogueBox, dialogueChoiceButtonPrefab, dialogueChoiceContainer, continueButton;

        [SerializeField]
        private ChoiceDialogueNode activeSegment;
        [SerializeField]
        private DialogueGraph selectedGraph;

        private Coroutine parser, typeWriter;
        private EventSystem eventSystem;

        public float TypewriterTextDelayTime { get => typewriterTextDelayTime; set => typewriterTextDelayTime = value; }
        public TMP_Text ActorNameText { get => actorNameText; set => actorNameText = value; }
        public TMP_Text DialogueText { get => dialogueText; set => dialogueText = value; }
        public TMP_Text InteractionText { get => interactionText; set => interactionText = value; }
        public Image ActorImage { get => actorImage; set => actorImage = value; }
        public GameObject DialogueBox { get => dialogueBox; set => dialogueBox = value; }
        public GameObject DialogueChoiceButtonPrefab { get => dialogueChoiceButtonPrefab; set => dialogueChoiceButtonPrefab = value; }
        public GameObject DialogueChoiceContainer { get => dialogueChoiceContainer; set => dialogueChoiceContainer = value; }
        public ChoiceDialogueNode ActiveSegment { get => activeSegment; set => activeSegment = value; }
        public DialogueGraph SelectedGraph { get => selectedGraph; set => selectedGraph = value; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(Instance);
            }
            else
            {
                Instance = this;
            }
            DontDestroyOnLoad(this);
            eventSystem = FindObjectOfType<EventSystem>();
        }

        /// <summary>
        /// Shows the Interaction Text with the provided string
        /// </summary>
        /// <param name="text"></param>
        public void ShowInteractionText(string text)
        {
            if (!interactionText.gameObject.activeSelf)
            {
                interactionText.gameObject.SetActive(true);
            }
            if (!interactionText.text.Equals(text))
            {
                interactionText.text = text;
            }
        }

        /// <summary>
        /// Hides the Interaction Text
        /// </summary>
        public void HideInteractionText()
        {
            interactionText.gameObject.SetActive(false);
        }

        /// <summary>
        /// Starts a dialogue with the provided <see cref="DialogueGraph"/>
        /// </summary>
        /// <param name="graph"></param>
        public void StartDialogue(DialogueGraph graph)
        {
            try
            {
                if(graph != null)
                {
                    selectedGraph = graph;
                }

                if (selectedGraph != null)
                {
                    if (!dialogueBox.activeSelf)
                    {
                        dialogueBox.SetActive(true);
                    }

                    foreach (BaseNode b in selectedGraph.nodes)
                    {
                        if (b.GetNodeType() == typeof(StartNode).Name)
                        { //"b" is a reference to whatever node it's found next. It's an enumerator variable 
                            selectedGraph.current = b; //Make this node the starting point. The [g] sets what graph to Use in the array OnTriggerEnter
                            break;
                        }
                    }
                    parser = StartCoroutine(ParseNode());
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"ERROR: Selected Graph Exception\n {ex.Message}");
            }
        }

        /// <summary>
        /// Clears the Dialogue Choices.
        /// </summary>
        public void ClearChoiceSelection()
        {
            if (dialogueChoiceContainer.activeSelf)
            {
                foreach (Transform child in dialogueChoiceContainer.transform)
                {
                    Destroy(child.gameObject);
                }
            }
        }

        /// <summary>
        /// This method updates the Choice Dialogue values and display.
        /// </summary>
        /// <param name="node"></param>
        public IEnumerator UpdateDialogue(ChoiceDialogueNode node)
        {
            activeSegment = node;
            actorNameText.text = node.ParseVariable(node.ActorName);
            if (typeWriter != null)
            {
                StopCoroutine(typeWriter);
                typeWriter = null;
            }
            typeWriter = StartCoroutine(TypewriterText(node.ParseVariable(node.DialogueText), typewriterTextDelayTime, dialogueText));
            if (node.GetSprite() != null)
            {
                actorImage.sprite = node.GetSprite();
                actorImage.color = Color.white;
            }
            else
            {
                actorImage.sprite = null;
                actorImage.color = new Color(0, 0, 0, 0);
            }

            ClearChoiceSelection();
            int selectedIndex = 0;

            yield return new WaitUntil(() => dialogueText.maxVisibleCharacters >= node.ParseVariable(node.DialogueText).Length);
            continueButton.SetActive(false);
            foreach (var answer in node.Answers)
            {
                var btnObj = Instantiate(dialogueChoiceButtonPrefab, dialogueChoiceContainer.transform); //spawns the buttons 
                btnObj.GetComponentInChildren<TMP_Text>().text = node.ParseVariable(answer);
                var index = selectedIndex;
                var btn = btnObj.GetComponentInChildren<Button>();
                if(btn != null)
                {
                    if(selectedIndex == 0) { SetSelectedGameobject(btnObj); }
                    btn.onClick.AddListener((() => { AnswerClicked(index); }));
                }

                selectedIndex++;
            }

        }

        /// <summary>
        /// This method gets the port for the Answers dialogue and sets the <see cref="selectedGraph"/> to that connection node.
        /// </summary>
        /// <param name="clickedIndex"></param>
        public void AnswerClicked(int clickedIndex)
        { //Function when the choices button is pressed 
            dialogueChoiceContainer.SetActive(false);
            BaseNode b = selectedGraph.current;
            var port = activeSegment.GetPort("Answers " + clickedIndex);

            if (port.IsConnected)
            {
                selectedGraph.current = port.Connection.node as BaseNode;
                parser = StartCoroutine(ParseNode());
            }
            else
            {
                DialogueBox.SetActive(false);
                NextNode("input");
                Debug.LogError("ERROR: ChoiceDialogue port is not connected");
                //NextNode("exit"); 

            }
        }

        private IEnumerator TypewriterText(string text,float delay, TMP_Text outputText)
        {
            //TODO: Find a way to not set the eventSystem.enabled to false?
            eventSystem.enabled = false;
            continueButton.SetActive(true);
            SetSelectedGameobject(continueButton);
            outputText.maxVisibleCharacters = 0;
            outputText.text = text;
            for (int i = 0; i < text.Length; i++)
            {
                outputText.maxVisibleCharacters++;
                yield return new WaitForSeconds(delay);
                //TODO: Find a way to not set the eventSystem.enabled to true to re-enable the submit input action?
                eventSystem.enabled = true;
            }
        }



        /// <summary>
        /// This IEnumerator method handles each node and their actions.
        /// </summary>
        /// <returns></returns>
        private IEnumerator ParseNode()
        {
            interactionText.gameObject.SetActive(false);
            BaseNode b = selectedGraph.current;
            string nodeName = b.GetNodeType();
            var targetActor = b.GetTargetActor();
            var sourceActor = b.GetSourceActor();
            var sourceGameObject = b.GetSourceGameobject();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            actorNameText.text = "";
            dialogueText.text = "";
            actorImage.sprite = null;
            actorImage.color = new Color(0, 0, 0, 0);


            ClearChoiceSelection();

            switch (nodeName)
            {
                case "StartNode":
                    NextNode("exit");
                    break;
                case "DialogueNode":
                    var dn = b as DialogueNode;
                    dialogueBox.SetActive(true);
                    actorNameText.text = dn.ParseVariable(dn.ActorName);
                    if (typeWriter != null)
                    {
                        StopCoroutine(typeWriter);
                        typeWriter = null;
                    }
                    typeWriter = StartCoroutine(TypewriterText(dn.ParseVariable(dn.DialogueText), typewriterTextDelayTime, dialogueText));
                    if (b.GetSprite() != null)
                    {
                        actorImage.sprite = dn.GetSprite();
                        actorImage.color = Color.white;
                    }
                    else
                    {
                        actorImage.sprite = null;
                        actorImage.color = new Color(0, 0, 0, 0);
                    }
                    if (actorNameText.text == "")
                    {
                        Debug.LogError("ERROR: Actor text for DialogueNode is empty");
                    }
                    if (dialogueText.text == "")
                    {
                        Debug.LogError("ERROR: Dialogue text for DialogueNode is empty");
                    }
                    break;
                case "ChoiceDialogueNode":
                    var cdn = b as ChoiceDialogueNode;
                    dialogueChoiceContainer.SetActive(true);
                    continueButton.SetActive(false);
                    StartCoroutine(UpdateDialogue(cdn)); //Instantiates the buttons 
                
                    if (actorNameText.text == "")
                    {
                        Debug.LogError("ERROR: Speaker text for ChoiceDialogueNode is empty");
                    }
                    if (dialogueText.text == "")
                    {
                        Debug.LogError("ERROR: Dialogue text for ChoiceDialogueNode is empty");
                    }
                    break;
                case "VariableNode":
                    var vn = b as VariableNode;
                    IntValue intvar = null;
                    DoubleValue doublevar = null;
                    FloatValue floatvar = null;
                    BoolValue boolvar = null;
                    StringValue stringvar = null;
                    var existingIntVar = vn.Variables.IntVariables[vn.VariableName];
                    var existingDoubleVar = vn.Variables.DoubleVariables[vn.VariableName];
                    var existingFloatVar = vn.Variables.FloatVariables[vn.VariableName];
                    var existingBoolVar = vn.Variables.BoolVariables[vn.VariableName];
                    var existingStringVar = vn.Variables.StringVariables[vn.VariableName];

                    try
                    {
                        switch (vn.VariableType)
                        {
                            case VariableType.Int:
                                intvar = new IntValue(vn.VariableName,int.Parse(vn.VariableValue));
                                break;
                            case VariableType.Double:
                                doublevar = new DoubleValue(vn.VariableName,double.Parse(vn.VariableValue));
                                break;
                            case VariableType.Float:
                                floatvar = new FloatValue(vn.VariableName,float.Parse(vn.VariableValue));
                                break;
                            case VariableType.Bool:
                                boolvar = new BoolValue(vn.VariableName,bool.Parse(vn.VariableValue));
                                break;
                            case VariableType.String:
                                stringvar = new StringValue(vn.VariableName,vn.VariableValue);
                                break;
                        }
                        switch (vn.OperatorType)
                        {
                            case Operator.Add:
                                switch (vn.VariableType)
                                {
                                    case VariableType.Int:
                                        if (existingIntVar != null)
                                        {
                                            existingIntVar.Value += intvar.Value;
                                        }
                                        else
                                        {
                                            vn.Variables.IntVariables.Variables.Add(intvar);
                                        }
                                        break;
                                    case VariableType.Double:
                                        if (existingDoubleVar != null)
                                        {
                                            existingDoubleVar.Value += doublevar.Value;
                                        }
                                        else
                                        {
                                            vn.Variables.DoubleVariables.Variables.Add(doublevar);
                                        }
                                        break;
                                    case VariableType.Float:
                                        if (existingFloatVar != null)
                                        {
                                            existingFloatVar.Value += floatvar.Value;
                                        }
                                        else
                                        {
                                            vn.Variables.FloatVariables.Variables.Add(floatvar);
                                        }
                                        break;
                                    case VariableType.Bool:
                                        if (existingBoolVar != null)
                                        {
                                            existingBoolVar.Value = true;
                                        }
                                        else
                                        {
                                            vn.Variables.BoolVariables.Variables.Add(boolvar);
                                        }
                                        break;
                                    case VariableType.String:
                                        if (existingStringVar != null)
                                        {
                                            existingStringVar.Value += stringvar.Value;
                                        }
                                        else
                                        {
                                            vn.Variables.StringVariables.Variables.Add(stringvar);
                                        }
                                        break;
                                }
                                NextNode("exitTrue");
                                break;
                            case Operator.Subtract:
                                switch (vn.VariableType)
                                {
                                    case VariableType.Int:
                                        if (existingIntVar != null)
                                        {
                                            existingIntVar.Value -= intvar.Value;
                                        }
                                        else
                                        {
                                            NextNode("exitFalse");
                                        }
                                        break;
                                    case VariableType.Double:
                                        if (existingDoubleVar != null)
                                        {
                                            existingDoubleVar.Value -= doublevar.Value;
                                        }
                                        else
                                        {
                                            NextNode("exitFalse");
                                        }
                                        break;
                                    case VariableType.Float:
                                        if (existingFloatVar != null)
                                        {
                                            existingFloatVar.Value -= floatvar.Value;
                                        }
                                        else
                                        {
                                            NextNode("exitFalse");
                                        }
                                        break;
                                    case VariableType.Bool:
                                        if (existingBoolVar != null)
                                        {
                                            existingBoolVar.Value = false;
                                        }
                                        else
                                        {
                                            NextNode("exitFalse");
                                        }
                                        break;
                                    case VariableType.String:
                                        if (existingStringVar != null)
                                        {
                                            existingStringVar.Value.Remove(stringvar.Value.Length);
                                        }
                                        else
                                        {
                                            NextNode("exitFalse");
                                        }
                                        break;
                                }
                                NextNode("exitTrue");
                                break;
                            case Operator.Multiply:
                                switch (vn.VariableType)
                                {
                                    case VariableType.Int:
                                        if (existingIntVar != null)
                                        {
                                            existingIntVar.Value *= intvar.Value;
                                        }
                                        else
                                        {
                                            vn.Variables.IntVariables.Variables.Add(intvar);
                                        }
                                        break;
                                    case VariableType.Double:
                                        if (existingDoubleVar != null)
                                        {
                                            existingDoubleVar.Value *= doublevar.Value;
                                        }
                                        else
                                        {
                                            vn.Variables.DoubleVariables.Variables.Add(doublevar);
                                        }
                                        break;
                                    case VariableType.Float:
                                        if (existingFloatVar != null)
                                        {
                                            existingFloatVar.Value *= floatvar.Value;
                                        }
                                        else
                                        {
                                            vn.Variables.FloatVariables.Variables.Add(floatvar);
                                        }
                                        break;
                                    case VariableType.Bool:
                                        if (existingBoolVar != null)
                                        {
                                            existingBoolVar.Value = true;
                                        }
                                        else
                                        {
                                            vn.Variables.BoolVariables.Variables.Add(boolvar);
                                        }
                                        break;
                                    case VariableType.String:
                                        NextNode("exitFalse");
                                        break;
                                }
                                NextNode("exitTrue");
                                break;
                            case Operator.Divide:
                                switch (vn.VariableType)
                                {
                                    case VariableType.Int:
                                        if (existingIntVar != null)
                                        {
                                            existingIntVar.Value /= intvar.Value;
                                        }
                                        else
                                        {
                                            NextNode("exitFalse");
                                        }
                                        break;
                                    case VariableType.Double:
                                        if (existingDoubleVar != null)
                                        {
                                            existingDoubleVar.Value /= doublevar.Value;
                                        }
                                        else
                                        {
                                            NextNode("exitFalse");
                                        }
                                        break;
                                    case VariableType.Float:
                                        if (existingFloatVar != null)
                                        {
                                            existingFloatVar.Value /= floatvar.Value;
                                        }
                                        else
                                        {
                                            NextNode("exitFalse");
                                        }
                                        break;
                                    case VariableType.Bool:
                                        if (existingBoolVar != null)
                                        {
                                            existingBoolVar.Value = false;
                                        }
                                        else
                                        {
                                            vn.Variables.BoolVariables.Variables.Add(boolvar);
                                        }
                                        break;
                                    case VariableType.String:
                                        NextNode("exitFalse");
                                        break;
                                }
                                NextNode("exitTrue");
                                break;
                            case Operator.Set:
                                switch (vn.VariableType)
                                {
                                    case VariableType.Int:
                                        if (existingIntVar != null)
                                        {
                                            existingIntVar.Value = intvar.Value;
                                        }
                                        else
                                        {
                                            vn.Variables.IntVariables.Variables.Add(intvar);
                                        }
                                        break;
                                    case VariableType.Double:
                                        if (existingDoubleVar != null)
                                        {
                                            existingDoubleVar.Value = doublevar.Value;
                                        }
                                        else
                                        {
                                            vn.Variables.DoubleVariables.Variables.Add(doublevar);
                                        }
                                        break;
                                    case VariableType.Float:
                                        if (existingFloatVar != null)
                                        {
                                            existingFloatVar.Value = floatvar.Value;
                                        }
                                        else
                                        {
                                            vn.Variables.FloatVariables.Variables.Add(floatvar);
                                        }
                                        break;
                                    case VariableType.Bool:
                                        if (existingBoolVar != null)
                                        {
                                            existingBoolVar.Value = boolvar.Value;
                                        }
                                        else
                                        {
                                            vn.Variables.BoolVariables.Variables.Add(boolvar);
                                        }
                                        break;
                                    case VariableType.String:
                                        if (existingStringVar != null)
                                        {
                                            existingStringVar.Value = stringvar.Value;
                                        }
                                        else
                                        {
                                            vn.Variables.StringVariables.Variables.Add(stringvar);
                                        }
                                        break;
                                }

                                NextNode("exitTrue");
                                break;
                            case Operator.EqualTo:
                                switch (vn.VariableType)
                                {
                                    case VariableType.Int:
                                        if (existingIntVar != null)
                                        {
                                            if (existingIntVar.EqualTo(intvar))
                                            {
                                                NextNode("exitTrue");
                                            }
                                            else
                                            {
                                                NextNode("exitFalse");
                                            }
                                        }
                                        else
                                        {
                                            NextNode("exitFalse");
                                        }
                                        break;
                                    case VariableType.Double:
                                        if (existingDoubleVar != null)
                                        {
                                            if (existingDoubleVar.EqualTo(doublevar))
                                            {
                                                NextNode("exitTrue");
                                            }
                                            else
                                            {
                                                NextNode("exitFalse");
                                            }
                                        }
                                        else
                                        {
                                            NextNode("exitFalse");
                                        }
                                        break;
                                    case VariableType.Float:
                                        if (existingFloatVar != null)
                                        {
                                            if (existingFloatVar.EqualTo(floatvar))
                                            {
                                                NextNode("exitTrue");
                                            }
                                            else
                                            {
                                                NextNode("exitFalse");
                                            }
                                        }
                                        else
                                        {
                                            NextNode("exitFalse");
                                        }
                                        break;
                                    case VariableType.Bool:
                                        if (existingBoolVar != null)
                                        {
                                            if (existingBoolVar.EqualTo(boolvar))
                                            {
                                                NextNode("exitTrue");
                                            }
                                            else
                                            {
                                                NextNode("exitFalse");
                                            }
                                        }
                                        else
                                        {
                                            NextNode("exitFalse");
                                        }
                                        break;
                                    case VariableType.String:
                                        if (existingStringVar != null)
                                        {
                                            if (existingStringVar.EqualTo(stringvar))
                                            {
                                                NextNode("exitTrue");
                                            }
                                            else
                                            {
                                                NextNode("exitFalse");
                                            }
                                        }
                                        else
                                        {
                                            NextNode("exitFalse");
                                        }
                                        break;
                                }

                                break;
                            case Operator.NotEqualTo:
                                switch (vn.VariableType)
                                {
                                    case VariableType.Int:
                                        if (existingIntVar != null)
                                        {
                                            if (existingIntVar.NotEqualTo(intvar))
                                            {
                                                NextNode("exitTrue");
                                            }
                                            else
                                            {
                                                NextNode("exitFalse");
                                            }
                                        }
                                        else
                                        {
                                            NextNode("exitFalse");
                                        }
                                        break;
                                    case VariableType.Double:
                                        if (existingDoubleVar != null)
                                        {
                                            if (existingDoubleVar.NotEqualTo(doublevar))
                                            {
                                                NextNode("exitTrue");
                                            }
                                            else
                                            {
                                                NextNode("exitFalse");
                                            }
                                        }
                                        else
                                        {
                                            NextNode("exitFalse");
                                        }
                                        break;
                                    case VariableType.Float:
                                        if (existingFloatVar != null)
                                        {
                                            if (existingFloatVar.NotEqualTo(floatvar))
                                            {
                                                NextNode("exitTrue");
                                            }
                                            else
                                            {
                                                NextNode("exitFalse");
                                            }
                                        }
                                        else
                                        {
                                            NextNode("exitFalse");
                                        }
                                        break;
                                    case VariableType.Bool:
                                        if (existingBoolVar != null)
                                        {
                                            if (existingBoolVar.NotEqualTo(boolvar))
                                            {
                                                NextNode("exitTrue");
                                            }
                                            else
                                            {
                                                NextNode("exitFalse");
                                            }
                                        }
                                        else
                                        {
                                            NextNode("exitFalse");
                                        }
                                        break;
                                    case VariableType.String:
                                        if (existingStringVar != null)
                                        {
                                            if (existingStringVar.NotEqualTo(stringvar))
                                            {
                                                NextNode("exitTrue");
                                            }
                                            else
                                            {
                                                NextNode("exitFalse");
                                            }
                                        }
                                        else
                                        {
                                            NextNode("exitFalse");
                                        }
                                        break;
                                }
                                break;
                            case Operator.GreaterThan:
                                switch (vn.VariableType)
                                {
                                    case VariableType.Int:
                                        if (existingIntVar != null)
                                        {
                                            if (existingIntVar.GreaterThan(intvar))
                                            {
                                                NextNode("exitTrue");
                                            }
                                            else
                                            {
                                                NextNode("exitFalse");
                                            }
                                        }
                                        else
                                        {
                                            NextNode("exitFalse");
                                        }
                                        break;
                                    case VariableType.Double:
                                        if (existingDoubleVar != null)
                                        {
                                            if (existingDoubleVar.GreaterThan(doublevar))
                                            {
                                                NextNode("exitTrue");
                                            }
                                            else
                                            {
                                                NextNode("exitFalse");
                                            }
                                        }
                                        else
                                        {
                                            NextNode("exitFalse");
                                        }
                                        break;
                                    case VariableType.Float:
                                        if (existingFloatVar != null)
                                        {
                                            if (existingFloatVar.GreaterThan(floatvar))
                                            {
                                                NextNode("exitTrue");
                                            }
                                            else
                                            {
                                                NextNode("exitFalse");
                                            }
                                        }
                                        else
                                        {
                                            NextNode("exitFalse");
                                        }
                                        break;
                                    case VariableType.Bool:
                                        if (existingBoolVar != null)
                                        {
                                            if (existingBoolVar.GreaterThan(boolvar))
                                            {
                                                NextNode("exitTrue");
                                            }
                                            else
                                            {
                                                NextNode("exitFalse");
                                            }
                                        }
                                        else
                                        {
                                            NextNode("exitFalse");
                                        }
                                        break;
                                    case VariableType.String:
                                        if (existingStringVar != null)
                                        {
                                            if (existingStringVar.GreaterThan(stringvar))
                                            {
                                                NextNode("exitTrue");
                                            }
                                            else
                                            {
                                                NextNode("exitFalse");
                                            }
                                        }
                                        else
                                        {
                                            NextNode("exitFalse");
                                        }
                                        break;
                                }
                                break;
                            case Operator.GreaterThanOrEqual:
                                switch (vn.VariableType)
                                {
                                    case VariableType.Int:
                                        if (existingIntVar != null)
                                        {
                                            if (existingIntVar.GreaterThanOrEqualTo(intvar))
                                            {
                                                NextNode("exitTrue");
                                            }
                                            else
                                            {
                                                NextNode("exitFalse");
                                            }
                                        }
                                        else
                                        {
                                            NextNode("exitFalse");
                                        }
                                        break;
                                    case VariableType.Double:
                                        if (existingDoubleVar != null)
                                        {
                                            if (existingDoubleVar.GreaterThanOrEqualTo(doublevar))
                                            {
                                                NextNode("exitTrue");
                                            }
                                            else
                                            {
                                                NextNode("exitFalse");
                                            }
                                        }
                                        else
                                        {
                                            NextNode("exitFalse");
                                        }
                                        break;
                                    case VariableType.Float:
                                        if (existingFloatVar != null)
                                        {
                                            if (existingFloatVar.GreaterThanOrEqualTo(floatvar))
                                            {
                                                NextNode("exitTrue");
                                            }
                                            else
                                            {
                                                NextNode("exitFalse");
                                            }
                                        }
                                        else
                                        {
                                            NextNode("exitFalse");
                                        }
                                        break;
                                    case VariableType.Bool:
                                        if (existingBoolVar != null)
                                        {
                                            if (existingBoolVar.GreaterThanOrEqualTo(boolvar))
                                            {
                                                NextNode("exitTrue");
                                            }
                                            else
                                            {
                                                NextNode("exitFalse");
                                            }
                                        }
                                        else
                                        {
                                            NextNode("exitFalse");
                                        }
                                        break;
                                    case VariableType.String:
                                        if (existingStringVar != null)
                                        {
                                            if (existingStringVar.GreaterThanOrEqualTo(stringvar))
                                            {
                                                NextNode("exitTrue");
                                            }
                                            else
                                            {
                                                NextNode("exitFalse");
                                            }
                                        }
                                        else
                                        {
                                            NextNode("exitFalse");
                                        }
                                        break;
                                }
                                break;
                            case Operator.LessThan:
                                switch (vn.VariableType)
                                {
                                    case VariableType.Int:
                                        if (existingIntVar != null)
                                        {
                                            if (existingIntVar.LessThan(intvar))
                                            {
                                                NextNode("exitTrue");
                                            }
                                            else
                                            {
                                                NextNode("exitFalse");
                                            }
                                        }
                                        else
                                        {
                                            NextNode("exitFalse");
                                        }
                                        break;
                                    case VariableType.Double:
                                        if (existingDoubleVar != null)
                                        {
                                            if (existingDoubleVar.LessThan(doublevar))
                                            {
                                                NextNode("exitTrue");
                                            }
                                            else
                                            {
                                                NextNode("exitFalse");
                                            }
                                        }
                                        else
                                        {
                                            NextNode("exitFalse");
                                        }
                                        break;
                                    case VariableType.Float:
                                        if (existingFloatVar != null)
                                        {
                                            if (existingFloatVar.LessThan(floatvar))
                                            {
                                                NextNode("exitTrue");
                                            }
                                            else
                                            {
                                                NextNode("exitFalse");
                                            }
                                        }
                                        else
                                        {
                                            NextNode("exitFalse");
                                        }
                                        break;
                                    case VariableType.Bool:
                                        if (existingBoolVar != null)
                                        {
                                            if (existingBoolVar.LessThan(boolvar))
                                            {
                                                NextNode("exitTrue");
                                            }
                                            else
                                            {
                                                NextNode("exitFalse");
                                            }
                                        }
                                        else
                                        {
                                            NextNode("exitFalse");
                                        }
                                        break;
                                    case VariableType.String:
                                        if (existingStringVar != null)
                                        {
                                            if (existingStringVar.LessThan(stringvar))
                                            {
                                                NextNode("exitTrue");
                                            }
                                            else
                                            {
                                                NextNode("exitFalse");
                                            }
                                        }
                                        else
                                        {
                                            NextNode("exitFalse");
                                        }
                                        break;
                                }
                                break;
                            case Operator.LessThanOrEqual:
                                switch (vn.VariableType)
                                {
                                    case VariableType.Int:
                                        if (existingIntVar != null)
                                        {
                                            if (existingIntVar.LessThanOrEqualTo(intvar))
                                            {
                                                NextNode("exitTrue");
                                            }
                                            else
                                            {
                                                NextNode("exitFalse");
                                            }
                                        }
                                        else
                                        {
                                            NextNode("exitFalse");
                                        }
                                        break;
                                    case VariableType.Double:
                                        if (existingDoubleVar != null)
                                        {
                                            if (existingDoubleVar.LessThanOrEqualTo(doublevar))
                                            {
                                                NextNode("exitTrue");
                                            }
                                            else
                                            {
                                                NextNode("exitFalse");
                                            }
                                        }
                                        else
                                        {
                                            NextNode("exitFalse");
                                        }
                                        break;
                                    case VariableType.Float:
                                        if (existingFloatVar != null)
                                        {
                                            if (existingFloatVar.LessThanOrEqualTo(floatvar))
                                            {
                                                NextNode("exitTrue");
                                            }
                                            else
                                            {
                                                NextNode("exitFalse");
                                            }
                                        }
                                        else
                                        {
                                            NextNode("exitFalse");
                                        }
                                        break;
                                    case VariableType.Bool:
                                        if (existingBoolVar != null)
                                        {
                                            if (existingBoolVar.LessThanOrEqualTo(boolvar))
                                            {
                                                NextNode("exitTrue");
                                            }
                                            else
                                            {
                                                NextNode("exitFalse");
                                            }
                                        }
                                        else
                                        {
                                            NextNode("exitFalse");
                                        }
                                        break;
                                    case VariableType.String:
                                        if (existingStringVar != null)
                                        {
                                            if (existingStringVar.LessThanOrEqualTo(stringvar))
                                            {
                                                NextNode("exitTrue");
                                            }
                                            else
                                            {
                                                NextNode("exitFalse");
                                            }
                                        }
                                        else
                                        {
                                            NextNode("exitFalse");
                                        }
                                        break;
                                }
                                break;
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"Error: {e.Message}");
                    }

                    break;
            
                case "ReferenceStateNode":
                    var rsn = b as ReferenceStateNode;
                    targetActor.DialogueManager.CurrentReferenceState = rsn.ReferenceState;
                    NextNode("exitTrue");
                    break;
                case "ExitNode_NoLoop_toStart":

                    DialogueBox.SetActive(false);
                    interactionText.gameObject.SetActive(true);
                    //TODO: Remove possibly? If this is not in here controllers activate the dialogue again on a single press
                    yield return new WaitForSeconds(0.05f);
                    OnDialogueEnded?.Invoke();
                    break;
                case "ExitNode":
                    DialogueBox.SetActive(false);
                    interactionText.gameObject.SetActive(true);
                    selectedGraph.Start();
                    //TODO: Remove possibly? If this is not in here controllers activate the dialogue again on a single press
                    yield return new WaitForSeconds(0.05f);
                    OnDialogueEnded?.Invoke();
                    break;
                default:
                    break;
            }
            yield break;
        }

        /// <summary>
        /// This method handles navigation to the next node based on the exit node string value.
        /// </summary>
        /// <param name="fieldName"></param>
        public void NextNode(string fieldName)
        {
            actorNameText.text = "";
            dialogueText.text = "";
            ClearChoiceSelection();
            if (parser != null)
            {
                StopCoroutine(parser);
                parser = null;
            }
            try
            {
                foreach (NodePort p in selectedGraph.current.Ports)
                {
                    try
                    {
                        if (p.fieldName == fieldName)
                        {
                            selectedGraph.current = p.Connection.node as BaseNode;
                            break;
                        }
                    }
                    catch (NullReferenceException)
                    {
                        Debug.LogError("ERROR: Port is not connected");
                    }
                }
            }
            catch (NullReferenceException)
            {
                Debug.LogError("ERROR: One of the elements on the Graph array at NodeParser is empty. Or, the Dialogue Graph is empty");
            }

            parser = StartCoroutine(ParseNode());

        }


        public void NextDialogue()
        {
            if (dialogueBox.activeSelf)
            {
                var dialogueNode = selectedGraph.current as DialogueNode;
                var choiceNode = selectedGraph.current as ChoiceDialogueNode;
                if(dialogueNode != null)
                {
                    if(dialogueText.maxVisibleCharacters < dialogueNode.ParseVariable(dialogueNode.DialogueText).Length)
                    {
                        if (typeWriter != null)
                        {
                            StopCoroutine(typeWriter);
                            typeWriter = null;
                        }
                        dialogueText.maxVisibleCharacters = dialogueNode.ParseVariable(dialogueNode.DialogueText).Length;
                    }
                    else
                    {
                        NextNode("exit");
                    }
                }
                if(choiceNode != null)
                {
                    if (dialogueText.maxVisibleCharacters < choiceNode.ParseVariable(choiceNode.DialogueText).Length)
                    {
                        if (typeWriter != null)
                        {
                            StopCoroutine(typeWriter);
                            typeWriter = null;
                        }
                        dialogueText.maxVisibleCharacters = choiceNode.ParseVariable(choiceNode.DialogueText).Length;
                    }
                    else
                    {
                        NextNode("exit");
                    }
                }
            }
        }

        public void SetSelectedGameobject(GameObject go)
        {
            eventSystem.SetSelectedGameObject(null);
            eventSystem.SetSelectedGameObject(go);
        }
    }
}