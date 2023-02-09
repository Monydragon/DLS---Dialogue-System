using DLS.Dialogue;
using XNodeEditor;

[CustomNodeEditor(typeof(ExitNode))]
public class ExitNodeEditor : NodeEditor
{
    public override void OnBodyGUI()
    {

        serializedObject.Update();

        var segment = serializedObject.targetObject as ExitNode;

        NodeEditorGUILayout.PortField(segment.GetPort("entry"));

        serializedObject.ApplyModifiedProperties();
    }
}
