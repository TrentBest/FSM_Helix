using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TheSingularityWorkshop.FSM_API;

using static System.Net.Mime.MediaTypeNames;
using static TheSingularityWorkshop.FSM_API.FSM_API;

namespace FSM_Helix.Models
{
    public abstract class HelixStateContext : IStateContext, IDisposable
    {
        private object _lock = new object();

        public bool IsValid { get; set; }
        public string Name { get; set; }

        public FSMHandle HelixStatus { get; }
        
        public Dictionary<string, FSMHandle> SubStatus { get; } = new Dictionary<string, FSMHandle>();

        public abstract void Dispose();

        // A simple method to trigger a transition on a sub-FSM.
        public void TriggerSubFSMTransition(string fsmName, string eventName)
        {
            if (SubStatus.TryGetValue(fsmName, out FSMHandle handle))
            {
                handle?.TransitionTo(eventName);
            }
        }

       
    }

}
