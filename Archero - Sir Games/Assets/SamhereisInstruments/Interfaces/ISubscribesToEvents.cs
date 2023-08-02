using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interfaces
{
    public interface ISubscribesToEvents
    {
        public void SubscribeToEvents();
        public void UnsubscribeFromEvents();
    }
}