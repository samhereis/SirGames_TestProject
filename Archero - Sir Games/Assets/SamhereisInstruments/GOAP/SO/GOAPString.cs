using Helpers;
using UnityEngine;

namespace SO.GOAP
{
    [CreateAssetMenu(fileName = "GOAPPreCondition", menuName = "ScriptableObjects/GOAP/GOAPPreCondition")]
    public class GOAPString : ScriptableObject
    {
        [field: SerializeField] public string preCondition { get; private set; } = "PreCondition";

        private void OnValidate()
        {
            if(preCondition != name)
            {
                preCondition = name;
                this.TrySetDirty();
            }
        }
    }
}