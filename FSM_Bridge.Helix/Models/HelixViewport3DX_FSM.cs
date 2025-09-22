using HelixToolkit.Wpf.SharpDX;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TheSingularityWorkshop.FSM_API;

using static TheSingularityWorkshop.FSM_API.FSM_API;

namespace TheSingularityWorkshop.FSM_Bridge.Helix
{
    public class HelixViewport3DX_FSM : HelixStateContext
    {
        public Viewport3DX Internal { get; set; }
        public string LifecycleGroup { get; set; } = "ViewportLifecycleGroup";
        public string CameraGroup { get; private set; } = "ViewportCameraGroup";
        public string InteractionGroup { get; private set; } = "ViewportInteractionGroup";
        public string RenderingGroup { get; private set; } = "ViewportRenderingGroup";

        public FSMHandle LifecycleFSM { get; private set; }
        public FSMHandle CameraFSM { get; private set; }
        public FSMHandle InteractionFSM { get; private set; }
        public FSMHandle RenderingFSM { get; private set; }
        public bool IsCameraMoving { get; set; } = false; // Add this property to make your transition logic work

        public HelixViewport3DX_FSM(Viewport3DX internalViewport) : base($"HelixViewport3DX_FSM:{internalViewport.Title}")
        {
            Internal = internalViewport;

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
                    .State("Unloaded", Viewport_FSM_Behavior.OnEnterUnloaded,
                    Viewport_FSM_Behavior.OnUpdateUnloaded,
                    Viewport_FSM_Behavior.OnExitUnloaded)
                    .State("Loaded", Viewport_FSM_Behavior.OnEnterLoaded,
                    Viewport_FSM_Behavior.OnUpdateLoaded,
                    Viewport_FSM_Behavior.OnExitLoaded)
                    .Transition("Unloaded", "Loaded", Viewport_FSM_Behavior.ToLoaded)
                    .Transition("Loaded", "Unloaded", Viewport_FSM_Behavior.ToUnloaded)
                    .BuildDefinition();
            }

            if (!Interaction.Exists("CameraControl_FSM", CameraGroup))
            {
                Create.CreateProcessingGroup(CameraGroup);
                Create.CreateFiniteStateMachine("CameraControl_FSM", 0, CameraGroup)
                    .State("Static", Viewport_FSM_Behavior.OnEnterCameraStatic,
                    Viewport_FSM_Behavior.OnUpdateCameraStatic,
                    Viewport_FSM_Behavior.OnExitCameraStatic)
                    .State("Moving", Viewport_FSM_Behavior.OnEnterCameraMoving,
                    Viewport_FSM_Behavior.OnUpdateCameraMoving,
                    Viewport_FSM_Behavior.OnExitCameraMoving)
                    .WithInitialState("Static")
                    .Transition("Static", "Moving", Viewport_FSM_Behavior.WhenCameraMoves)
                    .Transition("Moving", "Static", Viewport_FSM_Behavior.WhenCameraStops)
                    .BuildDefinition();
            }

            if (!Interaction.Exists("ViewportInteraction_FSM", InteractionGroup))
            {
                Create.CreateProcessingGroup(InteractionGroup);
                Create.CreateFiniteStateMachine("ViewportInteraction_FSM", 0, InteractionGroup)
                    .State("Passive", Viewport_FSM_Behavior.OnEnterInteractionPassive,
                    Viewport_FSM_Behavior.OnUpdateInteractionPassive,
                    Viewport_FSM_Behavior.OnExitInteractionPassive)
                    .State("Active", Viewport_FSM_Behavior.OnEnterInteractionActive,
                    Viewport_FSM_Behavior.OnUpdateInteractionActive,
                    Viewport_FSM_Behavior.OnExitInteractionActive)
                    .WithInitialState("Passive")
                    .Transition("Passive", "Active", Viewport_FSM_Behavior.WhenInputOccurs)
                    .Transition("Active", "Passive", Viewport_FSM_Behavior.WhenInputStops)
                    .BuildDefinition();
            }

            if (!Interaction.Exists("ViewportRendering_FSM", RenderingGroup))
            {
                Create.CreateProcessingGroup(RenderingGroup);
                Create.CreateFiniteStateMachine("ViewportRendering_FSM", 0, RenderingGroup)
                    .State("Rendering", Viewport_FSM_Behavior.OnEnterRendering,
                    Viewport_FSM_Behavior.OnUpdateRendering,
                    Viewport_FSM_Behavior.OnExitRendering)
                    .State("Suspended", Viewport_FSM_Behavior.OnEnterSuspended,
                    Viewport_FSM_Behavior.OnUpdateSuspended,
                    Viewport_FSM_Behavior.OnExitSuspended)
                    .WithInitialState("Rendering")
                    .Transition("Rendering", "Suspended", Viewport_FSM_Behavior.WhenRenderingSuspended)
                    .Transition("Suspended", "Rendering", Viewport_FSM_Behavior.WhenRenderingResumed)
                    .BuildDefinition();
            }
        }
    }
}