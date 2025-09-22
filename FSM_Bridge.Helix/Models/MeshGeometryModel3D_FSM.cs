using HelixToolkit.Wpf.SharpDX;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TheSingularityWorkshop.FSM_API;

namespace TheSingularityWorkshop.FSM_Bridge.Helix
{
    public class MeshGeometryModel3D_FSM : HelixStateContext
    {
        public MeshGeometryModel3D Internal { get; set; }
        public string MeshUpdateGroup { get; set; } = "MeshUpdateGroup";
        public string MeshTransformUpdateGroup { get; private set; } = "MeshTransformUpdateGroup";

        public MeshGeometryModel3D_FSM(MeshGeometryModel3D internalModel) : base($"MeshGeometryModel3D_FSM:{internalModel.GUID}")
        {
            Internal = internalModel;
            DefineFSMs();
            IsValid = true;
        }

        private void DefineFSMs()
        {
            if (!FSM_API.FSM_API.Interaction.Exists("MeshGeometryModel3D_FSM", MeshUpdateGroup))
            {
                FSM_API.FSM_API.Create.CreateFiniteStateMachine("MeshGeometryModel3D_FSM", 0, MeshUpdateGroup)
                    .State("Unloaded", MeshGeometryModel3D_FSM_Behavior.OnEnterUnloaded,
                    MeshGeometryModel3D_FSM_Behavior.OnUpdateUnloaded,
                    MeshGeometryModel3D_FSM_Behavior.OnExitUnloaded)
                    .State("Loaded", MeshGeometryModel3D_FSM_Behavior.OnEnterLoaded,
                    MeshGeometryModel3D_FSM_Behavior.OnUpdateLoaded,
                    MeshGeometryModel3D_FSM_Behavior.OnExitLoaded)
                    .Transition("Unloaded", "Loaded", MeshGeometryModel3D_FSM_Behavior.ToLoaded)
                    .Transition("Loaded", "Unloaded", MeshGeometryModel3D_FSM_Behavior.ToUnloaded)
                    .BuildDefinition();
            }

            if (!FSM_API.FSM_API.Interaction.Exists("MeshTransform_FSM", MeshTransformUpdateGroup))
            {
                FSM_API.FSM_API.Create.CreateFiniteStateMachine("MeshTransform_FSM", 0, MeshTransformUpdateGroup)
                .State("Composed", MeshGeometryModel3D_FSM_Behavior.OnEnterComposed,
                    MeshGeometryModel3D_FSM_Behavior.OnUpdateComposed,
                    MeshGeometryModel3D_FSM_Behavior.OnExitComposed)
                .State("Transforming", MeshGeometryModel3D_FSM_Behavior.OnEnterTransforming,
                    MeshGeometryModel3D_FSM_Behavior.OnUpdateTransforming,
                    MeshGeometryModel3D_FSM_Behavior.OnExitTransforming)
                .State("Deforming", MeshGeometryModel3D_FSM_Behavior.OnEnterDeforming,
                    MeshGeometryModel3D_FSM_Behavior.OnUpdateDeforming,
                    MeshGeometryModel3D_FSM_Behavior.OnExitDeforming)
                .WithInitialState("Composed")
                .Transition("Composed", "Transforming", MeshGeometryModel3D_FSM_Behavior.WhenTransforming)
                .Transition("Transforming", "Composed", MeshGeometryModel3D_FSM_Behavior.WhenCompositionIsApplied)
                .Transition("Composed", "Deforming", MeshGeometryModel3D_FSM_Behavior.WhenDeforming)
                .Transition("Deforming", "Composed", MeshGeometryModel3D_FSM_Behavior.WhenDeformationIsApplied)
                .BuildDefinition();
            }
        }
    }
}