using DLS.Dialogue;
using UnityEngine;
using XNodeEditor;

[CustomNodeEditor(typeof(ReferenceStateNode))]
public class ReferenceStateNodeEditor : NodeEditor
{
    public override void OnBodyGUI()
    {

        serializedObject.Update();

        var segment = serializedObject.targetObject as ReferenceStateNode;

        NodeEditorGUILayout.PortField(segment.GetPort("input"));
        NodeEditorGUILayout.PortField(segment.GetPort("exitTrue"));
        GUILayout.Label("Reference State");
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("referenceState"), GUIContent.none);
        serializedObject.ApplyModifiedProperties();
    }
}