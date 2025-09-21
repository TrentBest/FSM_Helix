using HelixToolkit.Wpf.SharpDX;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TheSingularityWorkshop.FSM_API;

using static TheSingularityWorkshop.FSM_API.FSM_API;

namespace FSM_Helix.Models
{
    public class HelixViewport3DX_FSM : IStateContext
    {
        public bool IsValid { get; set; } = false;
        public string Name { get; set; }

        public Viewport3DX Internal { get; set; }
        public string LifecycleGroup { get; set; } = "ViewportLifecycleGroup";
        public string CameraGroup { get; private set; } = "ViewportCameraGroup";
        public string InteractionGroup { get; private set; } = "ViewportInteractionGroup";
        public string RenderingGroup { get; private set; } = "ViewportRenderingGroup";

        public FSMHandle LifecycleFSM { get; private set; }
        public FSMHandle CameraFSM { get; private set; }
        public FSMHandle InteractionFSM { get; private set; }
        public FSMHandle RenderingFSM { get; private set; }

        public HelixViewport3DX_FSM(Viewport3DX internalViewport)
        {
            Internal = internalViewport;
            Name = $"HelixViewport3DX_FSM:{Internal.Title}";

            // We define SubStatus for each distinct behavior of the viewport.
            // This allows for compositional state management.
            DefineFSMs();

            // We create a handle for each FSM, allowing external SubStatus to control them.
            this.LifecycleFSM = Create.CreateInstance("ViewportLifecycle_FSM", this, LifecycleGroup);
            this.CameraFSM = Create.CreateInstance("CameraControl_FSM", this, CameraGroup);
            this.InteractionFSM = Create.CreateInstance("ViewportInteraction_FSM", this, InteractionGroup);
            this.RenderingFSM = Create.CreateInstance("ViewportRendering_FSM", this, RenderingGroup);

            IsValid = true;
        }

        private void DefineFSMs()
        {
            if (!Interaction.Exists("ViewportLifecycle_FSM", LifecycleGroup))
            {
                Create.CreateProcessingGroup(LifecycleGroup);
                Create.CreateFiniteStateMachine("ViewportLifecycle_FSM", 0, LifecycleGroup)
                    .State("Unloaded", ViewportFSM_Behavior.OnEnterUnloaded, 
                    ViewportFSM_Behavior.OnUpdateUnloaded, 
                    ViewportFSM_Behavior.OnExitUnloaded)
                    .State("Loaded", ViewportFSM_Behavior.OnEnterLoaded, 
                    ViewportFSM_Behavior.OnUpdateLoaded, 
                    ViewportFSM_Behavior.OnExitLoaded)
                    .Transition("Unloaded", "Loaded", ViewportFSM_Behavior.ToLoaded)
                    .Transition("Loaded", "Unloaded", ViewportFSM_Behavior.ToUnloaded)
                    .BuildDefinition();
            }

            if (!Interaction.Exists("CameraControl_FSM", CameraGroup))
            {
                Create.CreateProcessingGroup(CameraGroup);
                Create.CreateFiniteStateMachine("CameraControl_FSM", 0, CameraGroup)
                    .State("Static", ViewportFSM_Behavior.OnEnterCameraStatic, 
                    ViewportFSM_Behavior.OnUpdateCameraStatic, 
                    ViewportFSM_Behavior.OnExitCameraStatic)
                    .State("Moving", ViewportFSM_Behavior.OnEnterCameraMoving, 
                    ViewportFSM_Behavior.OnUpdateCameraMoving, 
                    ViewportFSM_Behavior.OnExitCameraMoving)
                    .WithInitialState("Static")
                    .Transition("Static", "Moving", ViewportFSM_Behavior.WhenCameraMoves)
                    .Transition("Moving", "Static", ViewportFSM_Behavior.WhenCameraStops)
                    .BuildDefinition();
            }

            if (!Interaction.Exists("ViewportInteraction_FSM", InteractionGroup))
            {
                Create.CreateProcessingGroup(InteractionGroup);
                Create.CreateFiniteStateMachine("ViewportInteraction_FSM", 0, InteractionGroup)
                    .State("Passive", ViewportFSM_Behavior.OnEnterInteractionPassive,
                    ViewportFSM_Behavior.OnUpdateInteractionPassive, 
                    ViewportFSM_Behavior.OnExitInteractionPassive)
                    .State("Active", ViewportFSM_Behavior.OnEnterInteractionActive, 
                    ViewportFSM_Behavior.OnUpdateInteractionActive, 
                    ViewportFSM_Behavior.OnExitInteractionActive)
                    .WithInitialState("Passive")
                    .Transition("Passive", "Active", ViewportFSM_Behavior.WhenInputOccurs)
                    .Transition("Active", "Passive", ViewportFSM_Behavior.WhenInputStops)
                    .BuildDefinition();
            }

            if (!Interaction.Exists("ViewportRendering_FSM", RenderingGroup))
            {
                Create.CreateProcessingGroup(RenderingGroup);
                Create.CreateFiniteStateMachine("ViewportRendering_FSM", 0, RenderingGroup)
                    .State("Rendering", ViewportFSM_Behavior.OnEnterRendering, 
                    ViewportFSM_Behavior.OnUpdateRendering, 
                    ViewportFSM_Behavior.OnExitRendering)
                    .State("Suspended", ViewportFSM_Behavior.OnEnterSuspended, 
                    ViewportFSM_Behavior.OnUpdateSuspended, 
                    ViewportFSM_Behavior.OnExitSuspended)
                    .WithInitialState("Rendering")
                    .Transition("Rendering", "Suspended", ViewportFSM_Behavior.WhenRenderingSuspended)
                    .Transition("Suspended", "Rendering", ViewportFSM_Behavior.WhenRenderingResumed)
                    .BuildDefinition();
            }
        }
    }
}