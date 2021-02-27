using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using aDB = Autodesk.AutoCAD.DatabaseServices;
using aApp = Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using aGeom = Autodesk.AutoCAD.Geometry;
using cDB = Autodesk.Civil.DatabaseServices;
using cApp = Autodesk.Civil.ApplicationServices;
using ACAD;
using consts = C3DAPITutorial.Data.Constants;

namespace C3DAPITutorial
{
    public class EntryPoint
    {
        [CommandMethod("CreateSamplesCmd3")]
        public void CreateSamplesCmd3()
        {
            /// Ask the user to select an alignment
            var isAlignmentFound = ACAD.Interaction.TrySelectObject<cDB.Alignment>(out aDB.ObjectId alignmentId);
            if (isAlignmentFound == false) return;

            // Ask the user to enter numbers
            // left width
            var isLeftWidthFound = ACAD.Interaction.AskForDisatance(out double leftWidth, "Enter Left Width");
            if (isLeftWidthFound == false) return;
            // right width
            var isRighttWidthFound = ACAD.Interaction.AskForDisatance(out double righttWidth, "Enter Right Width");
            if (isRighttWidthFound == false) return;
            // interval
            var isIntervalFound = ACAD.Interaction.AskForDisatance(out double interval, "Enter Interval");
            if (isIntervalFound == false) return;
            // Ask the user to enter numbers
            // left width
            var isSampleLineGroupNameFound = ACAD.Interaction.AskForString(out string slGroupName, "Enter name");
            if (isSampleLineGroupNameFound == false) return;

            using (var ts = CurrentDrawing.TransM.StartTransaction())
            {
                //----------------------------------- Step20:  get alignment ---------------------------------------------------
                var alignment = alignmentId.GetObject(aDB.OpenMode.ForWrite) as cDB.Alignment;

                //----------------------------------- Step30: create sample line group -----------------------------------------
                var slGroupId = alignment.AddSampleLineGroup(slGroupName, out string slGroupValidName);

                //----------------------------------- Step30: create sample line  ----------------------------------------------
                var stations = ACAD.General.GetRange(alignment.StartingStation, alignment.EndingStation, interval, consts.Epsilon);
                alignment.AddSampleLines(slGroupId, slGroupValidName, stations, leftWidth, righttWidth);

                ts.Commit();
            }

        }


    }
}

