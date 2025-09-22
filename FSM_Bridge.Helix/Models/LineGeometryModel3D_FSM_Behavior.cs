using System.Windows.Media;

using TheSingularityWorkshop.FSM_API;

namespace TheSingularityWorkshop.FSM_Bridge.Helix
{
    // A static class to hold all the FSM behavior methods.
    public static class LineGeometryModel3D_FSM_Behavior
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