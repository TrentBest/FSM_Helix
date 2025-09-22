using HelixToolkit.Wpf.SharpDX;

using System;
using System.Collections.Generic;
using System.Windows.Media;

using TheSingularityWorkshop.FSM_API;

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
                    .State("Unloaded", FSM_Behavior.OnEnterUnloaded, FSM_Behavior.OnUpdateUnloaded, FSM_Behavior.OnExitUnloaded)
                    .State("Loaded", FSM_Behavior.OnEnterLoaded, FSM_Behavior.OnUpdateLoaded, FSM_Behavior.OnExitLoaded)
                    .Transition("Unloaded", "Loaded", FSM_Behavior.WhenLoaded)
                    .Transition("Loaded", "Unloaded", FSM_Behavior.WhenUnloaded)
                    .BuildDefinition();
            }

            // FSM for the visual appearance/behavior of the line
            if (!FSM_API.FSM_API.Interaction.Exists("LineAppearance_FSM", "LineAppearanceGroup"))
            {
                FSM_API.FSM_API.Create.CreateProcessingGroup("LineAppearanceGroup");
                FSM_API.FSM_API.Create.CreateFiniteStateMachine("LineAppearance_FSM", 0, "LineAppearanceGroup")
                    .State("Visible", FSM_Behavior.OnEnterVisible, FSM_Behavior.OnUpdateVisible, FSM_Behavior.OnExitVisible)
                    .State("Hidden", FSM_Behavior.OnEnterHidden, FSM_Behavior.OnUpdateHidden, FSM_Behavior.OnExitHidden)
                    .State("Highlighted", FSM_Behavior.OnEnterHighlighted, FSM_Behavior.OnUpdateHighlighted, FSM_Behavior.OnExitHighlighted)
                    .WithInitialState("Visible")
                    .Transition("Visible", "Hidden", FSM_Behavior.WhenHidden)
                    .Transition("Hidden", "Visible", FSM_Behavior.WhenVisible)
                    .Transition("Visible", "Highlighted", FSM_Behavior.WhenHighlighted)
                    .Transition("Highlighted", "Visible", FSM_Behavior.WhenUnhighlighted)
                    .BuildDefinition();
            }
        }
    }

    // A static class to hold all the FSM behavior methods.
    public static class FSM_Behavior
    {
        // ... (Implementations for all the state and transition methods would go here.)
        // For example:
        public static void OnEnterLoaded(IStateContext context) { if (context is LineGeometryModel3D_FSM c) c.Internal.IsRendering = true; }
        public static bool WhenLoaded(IStateContext context) { return true; }
        public static bool WhenHidden(IStateContext context) { return true; }
        public static bool WhenVisible(IStateContext context) { return true; }
        public static bool WhenHighlighted(IStateContext context) { return false; }
        public static bool WhenUnhighlighted(IStateContext context) { return false; }

        // Placeholder methods to avoid compilation errors
        public static void OnEnterUnloaded(IStateContext context) { if (context is LineGeometryModel3D_FSM c) c.Internal.IsRendering = false; }
        public static void OnUpdateUnloaded(IStateContext context) { }
        public static void OnExitUnloaded(IStateContext context) { }
        public static void OnUpdateLoaded(IStateContext context) { }
        public static void OnExitLoaded(IStateContext context) { }
        public static void OnEnterVisible(IStateContext context) { }
        public static void OnUpdateVisible(IStateContext context) { }
        public static void OnExitVisible(IStateContext context) { }
        public static void OnEnterHidden(IStateContext context)
        {
            if (context is LineGeometryModel3D_FSM c)
            {
                c.Internal.IsRendering = false;
            }
        }
        public static void OnUpdateHidden(IStateContext context) { }
        public static void OnExitHidden(IStateContext context) { }
        public static void OnEnterHighlighted(IStateContext context)
        {
            if (context is LineGeometryModel3D_FSM c)
            {
                c.Internal.IsRendering = true;
                c.Internal.Color = Colors.Red;
                c.Internal.Thickness = 2.0;
            }
        }
        public static void OnUpdateHighlighted(IStateContext context) { }
        public static void OnExitHighlighted(IStateContext context) { }
        public static bool WhenUnloaded(IStateContext context) { return false; }

    }
}