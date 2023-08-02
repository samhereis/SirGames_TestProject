using SO.GOAP;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    public class UpdateWorld : MonoBehaviour
    {
        [TextArea] public string states;

        private void LateUpdate()
        {
            Dictionary<GOAPString, int> worldStates = GWorld.worldStates.GetStates();
            states = "";

            foreach (KeyValuePair<GOAPString, int> s in worldStates)
            {
                states += s.Key + ", " + s.Value + "\n";
            }
        }
    }
}