using DLS.Dialogue;
using XNodeEditor;

[CustomNodeEditor(typeof(ExitNode_NoLoop_toStart))]
public class ExitNode_NoLoop_ToStartNodeEditor : NodeEditor
{
    public override void OnBodyGUI()
    {

        serializedObject.Update();

        var segment = serializedObject.targetObject as ExitNode_NoLoop_toStart;

        NodeEditorGUILayout.PortField(segment.GetPort("entry"));
        NodeEditorGUILayout.PortField(segment.GetPort("exit"));

        serializedObject.ApplyModifiedProperties();
    }
}