using System;
using System.Collections.Generic;
using _02.Scirpts.Dictionary;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Editor.Inspector
{
    
    
    
    [CustomPropertyDrawer(typeof(SerializableData<,>))]
    public class SerializableDataDrawer : PropertyDrawer
    {
        private ReorderableList list;
        private string name = String.Empty;

        private SerializedProperty key, value;
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            property.Next(true);
            key = property.Copy();
            property.Next(true);
            value = property.Copy();

            Rect contentPos = EditorGUI.PrefixLabel(position, new GUIContent());

            GUI.skin.label.padding = new RectOffset(3, 3, 6, 6);
            EditorGUI.indentLevel = 0;
            float half = contentPos.width / 2;
            contentPos.width = contentPos.width / 3;
            EditorGUIUtility.labelWidth = 45f;
            EditorGUI.PropertyField(contentPos, key);
            contentPos.x += half;
            EditorGUI.PropertyField(contentPos, value);

            // EditorGUI.BeginProperty(contentPos, label, key);
            // {
            //     EditorGUI.PropertyField(contentPos, key);
            // }
            // EditorGUI.EndProperty();
            //
            // contentPos.x += half;
            //
            // EditorGUI.BeginProperty(contentPos, label, value);
            // {
            //     EditorGUI.PropertyField(contentPos, value);
            // }
            // EditorGUI.EndProperty();
            //
            //
        }
    }
}