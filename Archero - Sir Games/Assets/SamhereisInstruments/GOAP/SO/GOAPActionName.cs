using UnityEngine;

namespace SO.GOAP
{
    [CreateAssetMenu(fileName = "GOAPActionNames", menuName = "ScriptableObjects/GOAP/GOAPActionNames")]
    public class GOAPActionName : ScriptableObject
    {
        [field: SerializeField] public string actionName { get; private set; } = "DefaultActionName";
    }
}