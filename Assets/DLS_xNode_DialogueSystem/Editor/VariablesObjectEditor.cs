using DLS.Core;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(VariablesObject))]
public class VariablesObjectEditor : Editor
{
    private VariablesObject _target;
    private void OnEnable()
    {
        _target = (VariablesObject)target;
    }

    private void DrawIntVariables()
    {
        var intVariables = _target.IntVariables.Variables;
        for (int i = 0; i < intVariables.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            intVariables[i].Name = EditorGUILayout.TextField(intVariables[i].Name);
            intVariables[i].Value = EditorGUILayout.IntField(intVariables[i].Value);
            if (GUILayout.Button("-"))
            {
                intVariables.RemoveAt(i);
            }

            EditorGUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Add Int Variable"))
        {
            intVariables.Add(new IntValue("New Int Variable", 0));
        }
    }

    private void DrawDoubleVariables()
    {
        var doubleVariables = _target.DoubleVariables.Variables;
        for (int i = 0; i < doubleVariables.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            doubleVariables[i].Name = EditorGUILayout.TextField(doubleVariables[i].Name);
            doubleVariables[i].Value = EditorGUILayout.DoubleField(doubleVariables[i].Value);
            if (GUILayout.Button("-"))
            {
                doubleVariables.RemoveAt(i);
            }

            EditorGUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Add Double Variable"))
        {
            doubleVariables.Add(new DoubleValue("New Double Variable", 0.0));
        }
    }

    private void DrawFloatVariables()
    {
        var floatVariables = _target.FloatVariables.Variables;
        for (int i = 0; i < floatVariables.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            floatVariables[i].Name = EditorGUILayout.TextField(floatVariables[i].Name);
            floatVariables[i].Value = EditorGUILayout.FloatField(floatVariables[i].Value);
            if (GUILayout.Button("-"))
            {
                floatVariables.RemoveAt(i);
            }

            EditorGUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Add Float Variable"))
        {
            floatVariables.Add(new FloatValue("New Float Variable", 0f));
        }
    }

    private void DrawBoolVariables()
    {
        var boolVariables = _target.BoolVariables.Variables;
        for (int i = 0; i < boolVariables.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            boolVariables[i].Name = EditorGUILayout.TextField(boolVariables[i].Name);
            boolVariables[i].Value = EditorGUILayout.Toggle(boolVariables[i].Value);
            if (GUILayout.Button("-"))
            {
                boolVariables.RemoveAt(i);
            }

            EditorGUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Add Bool Variable"))
        {
            boolVariables.Add(new BoolValue("New Bool Variable", false));
        }
    }

    private void DrawStringVariables()
    {
        var stringVariables = _target.StringVariables.Variables;
        for (int i = 0; i < stringVariables.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            stringVariables[i].Name = EditorGUILayout.TextField(stringVariables[i].Name);
            stringVariables[i].Value = EditorGUILayout.TextField(stringVariables[i].Value);
            if (GUILayout.Button("-"))
            {
                stringVariables.RemoveAt(i);
            }

            EditorGUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Add String Variable"))
        {
            stringVariables.Add(new StringValue("New String Variable", ""));
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawIntVariables();
        DrawDoubleVariables();
        DrawFloatVariables();
        DrawBoolVariables();
        DrawStringVariables();
        serializedObject.ApplyModifiedProperties();
    }
}