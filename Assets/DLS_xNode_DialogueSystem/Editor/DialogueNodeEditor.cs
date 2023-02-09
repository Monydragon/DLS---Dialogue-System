using DLS.Dialogue;
using UnityEngine;
using XNodeEditor;

[CustomNodeEditor(typeof(DialogueNode))]
public class DialogueNodeEditor : NodeEditor
{
    public override void OnBodyGUI()
    {

        serializedObject.Update();

        var segment = serializedObject.targetObject as DialogueNode;

        NodeEditorGUILayout.PortField(segment.GetPort("input"));
        NodeEditorGUILayout.PortField(segment.GetPort("exit"));
        GUILayout.Label("Variables");
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("variables"), GUIContent.none);
        GUILayout.Label("Display Actor Name");
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("actorName"), GUIContent.none);
        GUILayout.Label("Actor Portrait");
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("sprite"), GUIContent.none);
        GUILayout.Label("Dialogue Text");
        GUILayout.Label("", new GUILayoutOption[] { GUILayout.Height(-20), }); //Sets Answers and Dialogue textfield position
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("dialogueText"), GUIContent.none);
        serializedObject.ApplyModifiedProperties();
    }
}