using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP.GoapDataClasses
{
    [Serializable]
    public class GInventory
    {
        public List<GameObject> items = new List<GameObject>();

        public void AddItem(GameObject i)
        {
            items.Add(i);
        }

        public GameObject FindItemWithTag(string tag)
        {
            return items.Find(x => x.tag == tag);
        }

        public void RemoveItem(GameObject i)
        {
            items.Remove(i);
        }
    }
}