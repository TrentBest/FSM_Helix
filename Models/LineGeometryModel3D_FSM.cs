using Cyotek.Drawing.BitmapFont;

using HelixToolkit.Wpf.SharpDX;
using HelixToolkit.Wpf.SharpDX.Model.Scene2D;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TheSingularityWorkshop.FSM_API;

using static System.Runtime.InteropServices.JavaScript.JSType;
using static TheSingularityWorkshop.FSM_API.FSM_API;

namespace FSM_Helix.Models
{
    public class LineGeometryModel3D_FSM : IStateContext
    {
        public bool IsValid { get; set; } = false;
        public string Name { get; set; } = "LineGeometryModel3D_FSM";


        public LineGeometryModel3D Internal { get; set; }
        public string LineUpdateGroup { get; set; } = "LineUpdateGroup";

        public LineGeometryModel3D_FSM(LineGeometryModel3D internalModel)
        {
            Internal = internalModel;
            Name = $"LineGeometryModel3D_FSM:{Internal.GUID}";//ToDo:  This might be bloating.
            if (!FSM_API.Interaction.Exists("LineGeometryModel3D_FSM", LineUpdateGroup))
            {
                FSM_API.Create.CreateProcessingGroup(LineUpdateGroup);
                FSM_API.Create.CreateFiniteStateMachine("LineGeometryModel3D_FSM", 0, LineUpdateGroup)
                // 1. Create the LineGeometry3D: A LineBuilder class can be used to programmatically
                // generate a large collection of points and indices for your grid.Alternatively, you
                // can manually define the points and indices for something simple, like a stick figure.
                .State("Creating", LineGeometryModel3D_FSM_Behavior.OnEnterCreating,
                    LineGeometryModel3D_FSM_Behavior.OnUpdateCreating,
                    LineGeometryModel3D_FSM_Behavior.OnExitCreating)
                // 2. Create the LineGeometryModel3D: You create an instance of LineGeometryModel3D and
                // assign the geometry you just created to its Geometry property.
                .State("Created", LineGeometryModel3D_FSM_Behavior.OnEnterCreated,
                    LineGeometryModel3D_FSM_Behavior.OnUpdateCreated,
                    LineGeometryModel3D_FSM_Behavior.OnExitCreated)
                // 3.  Set the Material: You then assign a LineMaterial to the Material property,
                // setting its color and thickness.
                .State("MaterialSet", LineGeometryModel3D_FSM_Behavior.OnEnterMaterialSet,
                    LineGeometryModel3D_FSM_Behavior.OnUpdateMaterialSet,
                    LineGeometryModel3D_FSM_Behavior.OnExitMaterialSet)
                // 4.  Add to the Scene: Finally, you add the LineGeometryModel3D instance to the
                // scene graph in your Viewport3DX.
                .State("AddedToScene", LineGeometryModel3D_FSM_Behavior.OnEnterAddedToScene,
                    LineGeometryModel3D_FSM_Behavior.OnUpdateAddedToScene,
                    LineGeometryModel3D_FSM_Behavior.OnExitAddedToScene)
                .BuildDefinition();

            }
            
            IsValid = true;
        }
    }
}
