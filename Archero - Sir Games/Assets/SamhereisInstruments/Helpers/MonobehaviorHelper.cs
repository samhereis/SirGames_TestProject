using UnityEngine;

namespace Helpers
{
    public static class MonobehaviorHelper
    {
        public static void TrySetDirty(this MonoBehaviour monoBehaviour)
        {
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(monoBehaviour);
#endif
        }

        public static void TrySetDirty(this ScriptableObject scriptable)
        {
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(scriptable);
#endif
        }

        public static void TrySetDirty(this MonoBehaviour monoBehaviour, MonoBehaviour anotherMonoBehaviour)
        {
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(anotherMonoBehaviour);
#endif
        }
    }
}