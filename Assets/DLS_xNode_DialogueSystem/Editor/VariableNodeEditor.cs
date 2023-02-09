using DLS.Dialogue;
using UnityEngine;
using XNodeEditor;

[CustomNodeEditor(typeof(VariableNode))]
public class VariableNodeEditor : NodeEditor
{
    public override void OnBodyGUI()
    {

        serializedObject.Update();

        var segment = serializedObject.targetObject as VariableNode;

        NodeEditorGUILayout.PortField(segment.GetPort("input"));
        NodeEditorGUILayout.PortField(segment.GetPort("exitTrue"));
        NodeEditorGUILayout.PortField(segment.GetPort("exitFalse"));
        GUILayout.Label("Variables");
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("variables"), GUIContent.none);
        GUILayout.Label("Variable Name");
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("variableName"), GUIContent.none);
        GUILayout.Label("Variable Type");
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("variableType"), GUIContent.none);
        GUILayout.Label("Value");
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("variableValue"), GUIContent.none);
        GUILayout.Label("Operator");
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("operatorType"), GUIContent.none);
        serializedObject.ApplyModifiedProperties();
    }
}