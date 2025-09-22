using TheSingularityWorkshop.FSM_API;
using HelixToolkit.Wpf.SharpDX;
using System.Windows.Media.Media3D;

namespace TheSingularityWorkshop.FSM_Bridge.Helix
{
    internal class Viewport_FSM_Behavior
    {
        // Viewport Lifecycle
        internal static void OnEnterUnloaded(IStateContext context)
        {
            if (context is HelixViewport3DX_FSM c)
            {
                c.Internal.Visibility = System.Windows.Visibility.Collapsed;
            }
        }
        internal static void OnUpdateUnloaded(IStateContext context) { }
        internal static void OnExitUnloaded(IStateContext context) { }
        internal static void OnEnterLoaded(IStateContext context)
        {
            if (context is HelixViewport3DX_FSM c)
            {
                c.Internal.Visibility = System.Windows.Visibility.Visible;
            }
        }
        internal static void OnUpdateLoaded(IStateContext context) { }
        internal static void OnExitLoaded(IStateContext context) { }

        // Rendering States
        internal static void OnEnterRendering(IStateContext context)
        {
            if (context is HelixViewport3DX_FSM c)
            {
                c.Internal.Visibility = System.Windows.Visibility.Visible;
            }
        }
        internal static void OnUpdateRendering(IStateContext context) { }
        internal static void OnExitRendering(IStateContext context) { }
        internal static void OnEnterSuspended(IStateContext context)
        {
            if (context is HelixViewport3DX_FSM c)
            {
                c.Internal.Visibility = System.Windows.Visibility.Collapsed;
            }
        }
        internal static void OnUpdateSuspended(IStateContext context) { }
        internal static void OnExitSuspended(IStateContext context) { }

        // Camera and Interaction States
        internal static void OnEnterCameraMoving(IStateContext context)
        {
            if (context is HelixViewport3DX_FSM c)
            {
                // This is a placeholder for your camera movement logic.
            }
        }
        internal static void OnUpdateCameraMoving(IStateContext context) { }
        internal static void OnExitCameraMoving(IStateContext context) { }
        internal static void OnEnterCameraStatic(IStateContext context)
        {
            if (context is HelixViewport3DX_FSM c)
            {
                // Logic to handle camera static state
            }
        }
        internal static void OnUpdateCameraStatic(IStateContext context) { }
        internal static void OnExitCameraStatic(IStateContext context) { }
        internal static void OnEnterInteractionActive(IStateContext context)
        {
            if (context is HelixViewport3DX_FSM c)
            {
                // Logic for active user interaction
            }
        }
        internal static void OnUpdateInteractionActive(IStateContext context) { }
        internal static void OnExitInteractionActive(IStateContext context) { }
        internal static void OnEnterInteractionPassive(IStateContext context)
        {
            if (context is HelixViewport3DX_FSM c)
            {
                // Logic for passive user interaction
            }
        }
        internal static void OnUpdateInteractionPassive(IStateContext context) { }
        internal static void OnExitInteractionPassive(IStateContext context) { }

        // Transition Conditions
        internal static bool ToLoaded(IStateContext context)
        {
            return true;
        }
        internal static bool ToUnloaded(IStateContext context)
        {
            return false;
        }
        internal static bool WhenCameraMoves(IStateContext context)
        {
            if (context is HelixViewport3DX_FSM c)
            {
                return c.IsCameraMoving;
            }
            return false;
        }
        internal static bool WhenCameraStops(IStateContext context)
        {
            if (context is HelixViewport3DX_FSM c)
            {
                return !c.IsCameraMoving;
            }
            return false;
        }
        internal static bool WhenInputOccurs(IStateContext context)
        {
            return false; // Placeholder for actual input detection
        }
        internal static bool WhenInputStops(IStateContext context)
        {
            return false; // Placeholder for actual input stop detection
        }
        internal static bool WhenRenderingResumed(IStateContext context)
        {
            return false; // Placeholder for rendering resumed event
        }
        internal static bool WhenRenderingSuspended(IStateContext context)
        {
            return false; // Placeholder for rendering suspended event
        }
    }
}