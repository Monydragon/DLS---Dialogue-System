using DLS.Dialogue;
using XNodeEditor;

[CustomNodeEditor(typeof(StartNode))]
public class StartNodeEditor : NodeEditor
{
    public override void OnBodyGUI()
    {

        serializedObject.Update();

        var segment = serializedObject.targetObject as StartNode;

        NodeEditorGUILayout.PortField(segment.GetPort("exit"));

        serializedObject.ApplyModifiedProperties();
    }
}
