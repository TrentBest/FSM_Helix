using HelixToolkit.Wpf.SharpDX;

using System;
using System.Collections.Generic;

namespace TheSingularityWorkshop.FSM_Bridge.Helix
{
    public class LineGeometryModel3D_FSM : HelixStateContext
    {
        public LineGeometryModel3D Internal { get; private set; }

        public LineGeometryModel3D_FSM(LineGeometryModel3D internalModel, string name) : base(name)
        {
            Internal = internalModel;
            DefineFSMs();

            this.HelixStatus = FSM_API.FSM_API.Create.CreateInstance("LineLifecycle_FSM", this, "LineLifecycleGroup");
            this.SubStatus["Appearance"] = FSM_API.FSM_API.Create.CreateInstance("LineAppearance_FSM", this, "LineAppearanceGroup");

            this.IsValid = true;
        }

        private void DefineFSMs()
        {
            // FSM for the lifecycle of the line component
            if (!FSM_API.FSM_API.Interaction.Exists("LineLifecycle_FSM", "LineLifecycleGroup"))
            {
                FSM_API.FSM_API.Create.CreateProcessingGroup("LineLifecycleGroup");
                FSM_API.FSM_API.Create.CreateFiniteStateMachine("LineLifecycle_FSM", 0, "LineLifecycleGroup")
                    .State("Unloaded", LineGeometryModel3D_FSM_Behavior.OnEnterUnloaded, LineGeometryModel3D_FSM_Behavior.OnUpdateUnloaded, LineGeometryModel3D_FSM_Behavior.OnExitUnloaded)
                    .State("Loaded", LineGeometryModel3D_FSM_Behavior.OnEnterLoaded, LineGeometryModel3D_FSM_Behavior.OnUpdateLoaded, LineGeometryModel3D_FSM_Behavior.OnExitLoaded)
                    .Transition("Unloaded", "Loaded", LineGeometryModel3D_FSM_Behavior.WhenLoaded)
                    .Transition("Loaded", "Unloaded", LineGeometryModel3D_FSM_Behavior.WhenUnloaded)
                    .BuildDefinition();
            }

            // FSM for the visual appearance/behavior of the line
            if (!FSM_API.FSM_API.Interaction.Exists("LineAppearance_FSM", "LineAppearanceGroup"))
            {
                FSM_API.FSM_API.Create.CreateProcessingGroup("LineAppearanceGroup");
                FSM_API.FSM_API.Create.CreateFiniteStateMachine("LineAppearance_FSM", 0, "LineAppearanceGroup")
                    .State("Visible", LineGeometryModel3D_FSM_Behavior.OnEnterVisible, LineGeometryModel3D_FSM_Behavior.OnUpdateVisible, LineGeometryModel3D_FSM_Behavior.OnExitVisible)
                    .State("Hidden", LineGeometryModel3D_FSM_Behavior.OnEnterHidden, LineGeometryModel3D_FSM_Behavior.OnUpdateHidden, LineGeometryModel3D_FSM_Behavior.OnExitHidden)
                    .State("Highlighted", LineGeometryModel3D_FSM_Behavior.OnEnterHighlighted, LineGeometryModel3D_FSM_Behavior.OnUpdateHighlighted, LineGeometryModel3D_FSM_Behavior.OnExitHighlighted)
                    .WithInitialState("Visible")
                    .Transition("Visible", "Hidden", LineGeometryModel3D_FSM_Behavior.WhenHidden)
                    .Transition("Hidden", "Visible", LineGeometryModel3D_FSM_Behavior.WhenVisible)
                    .Transition("Visible", "Highlighted", LineGeometryModel3D_FSM_Behavior.WhenHighlighted)
                    .Transition("Highlighted", "Visible", LineGeometryModel3D_FSM_Behavior.WhenUnhighlighted)
                    .BuildDefinition();
            }
        }
    }
}