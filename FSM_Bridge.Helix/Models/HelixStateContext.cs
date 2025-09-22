using System;
using System.Collections.Generic;

using TheSingularityWorkshop.FSM_API;

namespace TheSingularityWorkshop.FSM_Bridge.Helix
{
    public abstract class HelixStateContext : IStateContext, IDisposable
    {
        public bool IsValid { get; set; }
        public string Name { get; set; } = string.Empty;

        public FSMHandle? HelixStatus { get; protected set; }
        public Dictionary<string, FSMHandle> SubStatus { get; } = new Dictionary<string, FSMHandle>();

        public HelixStateContext(string name)
        {
            Name = name;
            IsValid = false;
            
        }

        public void TriggerSubFSMTransition(string fsmName, string eventName)
        {
            if (SubStatus.TryGetValue(fsmName, out FSMHandle? handle))
            {
                handle?.TransitionTo(eventName);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (IsValid)
            {
                if (disposing)
                {
                    if (HelixStatus != null && HelixStatus.IsValid)
                    {
                        TriggerSubFSMTransition("Lifecycle", "ToUnloaded");
                        FSM_API.FSM_API.Interaction.DestroyInstance(HelixStatus);
                    }

                    foreach (var handle in SubStatus.Values)
                    {
                        FSM_API.FSM_API.Interaction.DestroyInstance(handle);
                    }
                }
                IsValid = false;
            }
        }
    }
}