using GOAP.GoapDataClasses;
using SO.GOAP;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GOAP
{
#if UNITY_EDITOR
    [CustomEditor(typeof(GAgentVisual))]
    [CanEditMultipleObjects]
    public class GAgentVisualEditor : Editor
    {
        //Display properties of the GAgent in the Inspector
        public override void OnInspectorGUI()
        {
            //Draw the default items in the Inspector as Unity would without
            //this script
            DrawDefaultInspector();

            //syncronise values from the running of the code eith the script properties
            serializedObject.Update();

            //get the agent game object so the GAgent and associated properties can
            //be displayed
            GAgentVisual agent = (GAgentVisual)target;
            GUILayout.Label("Name: " + agent.name);
            GUILayout.Label("Current Action: " + agent.gameObject.GetComponent<GAgent>().baseSettings.currentAction);
            GUILayout.Label("Actions: ");
            foreach (GAction a in agent.gameObject.GetComponent<GAgent>().baseSettings.actions)
            {
                string pre = "";
                string eff = "";

                foreach (KeyValuePair<GOAPString, int> p in a.baseSettings.preConditions)
                    pre += p.Key + ", ";
                foreach (KeyValuePair<GOAPString, int> e in a.baseSettings.afterEffects)
                    eff += e.Key + ", ";

                GUILayout.Label("====  " + a.baseSettings.actionName + "(" + pre + ")(" + eff + ")");
            }
            GUILayout.Label("Goals: ");
            foreach (KeyValuePair<SubGoals, int> g in agent.gameObject.GetComponent<GAgent>().baseSettings.goals)
            {
                foreach (KeyValuePair<GOAPString, int> sg in g.Key.subGoals)
                    GUILayout.Label("=====  " + sg.Key + " - " + sg.Value);
            }
            GUILayout.Label("Beliefs: ");
            foreach (KeyValuePair<GOAPString, int> sg in agent.gameObject.GetComponent<GAgent>().baseSettings.localStates.GetStates())
            {
                GUILayout.Label("=====  " + sg.Key + " - " + sg.Value);
            }

            GUILayout.Label("Inventory: ");
            foreach (GameObject g in agent.gameObject.GetComponent<GAgent>().baseSettings.inventory.items)
            {
                GUILayout.Label("====  " + g.tag);
            }


            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}