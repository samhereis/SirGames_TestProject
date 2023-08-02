using UnityEditor;
using UnityEngine.UI;

namespace UI.Interaction
{
    public class BetterButton : Button
    {

    }

# if UNITY_EDITOR
    [CustomEditor(typeof(BetterButton))]
    public class BetterButtonEditor : UnityEditor.UI.ButtonEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();

            serializedObject.Update();
        }
    }
#endif
}