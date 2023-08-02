using DI;
using Helpers;
using UnityEditor;
using UnityEngine;

namespace Samhereis.DI
{
    [CustomPropertyDrawer(typeof(BindDIScene.SOToDI))]
    public class SOToDIDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.indentLevel = 2;

            var rects = position.Row(new[] { 2, 3f });

            EditorGUI.PropertyField(rects[0], property.FindPropertyRelative("id"), new GUIContent());
            EditorGUI.PropertyField(rects[1], property.FindPropertyRelative("Instance"), new GUIContent());
        }
    }
}