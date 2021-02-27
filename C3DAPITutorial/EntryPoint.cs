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

namespace C3DAPITutorial
{
    public class EntryPoint
    {
        [CommandMethod("CreateSampleLineCmd")]
        public void CreateSampleLineCmd()
        {
            // get currunt document 
            var acDoc = aApp.Application.DocumentManager.MdiActiveDocument;
            var database = acDoc.Database;
            var editor = acDoc.Editor;
            var civilDoc = cApp.CivilApplication.ActiveDocument;

            // Ask the user to select an alignment
            var promptAlignmentOpt = new PromptEntityOptions("\nSelect an Alignment");
            promptAlignmentOpt.SetRejectMessage("\nObject must be a alignmetnt.");
            promptAlignmentOpt.AddAllowedClass(typeof(cDB.Alignment), false);
            var promptAlignment = editor.GetEntity(promptAlignmentOpt);
            if (promptAlignment.Status != PromptStatus.OK)
            {
                editor.WriteMessage("\nNo Selection found");
                return;
            }

            // start transaction
            using (var ts = database.TransactionManager.StartTransaction())
            {
                // open selected alignment for write
                var alignment = promptAlignment.ObjectId.GetObject(aDB.OpenMode.ForWrite) as cDB.Alignment;

                // create sample line group
                var slGroupId = cDB.SampleLineGroup.Create("SL-Group-1", alignment.Id);
                var slGroup = alignment.GetSampleLineGroupIds().Add(slGroupId);

                // calculate sample line
                var p1 = new aGeom.Point2d();
                var p2 = new aGeom.Point2d(20, 0);
                var slPoints = new aGeom.Point2dCollection() { p1, p2 };

                // create sample lines
                var sampleLineId = cDB.SampleLine.Create("STA-", slGroupId, slPoints);

                ts.Commit();
            }

        }


        [CommandMethod("CreateSampleByStationCmd")]
        public void CreateSampleByStationCmd()
        {
            // get currunt document 
            var acDoc = aApp.Application.DocumentManager.MdiActiveDocument;
            var database = acDoc.Database;
            var editor = acDoc.Editor;
            var civilDoc = cApp.CivilApplication.ActiveDocument;

            /// Ask the user to select an alignment
            var promptAlignmentOpt = new PromptEntityOptions("\nSelect an Alignment");
            promptAlignmentOpt.SetRejectMessage("\nObject must be a polyline.");
            promptAlignmentOpt.AddAllowedClass(typeof(cDB.Alignment), false);
            var promptAlignment = editor.GetEntity(promptAlignmentOpt);
            if (promptAlignment.Status != PromptStatus.OK)
            {
                editor.WriteMessage("\nNo Selection found");
                return;
            }

            double leftWidth = 25;
            double righttWidth = 35;
            double station = 100;

            using (var ts = database.TransactionManager.StartTransaction())
            {
                // Ask the user to select an alignment
                var alignment = promptAlignment.ObjectId.GetObject(aDB.OpenMode.ForWrite) as cDB.Alignment;

                // create sample line group
                var slGroupId = cDB.SampleLineGroup.Create("SL - Group1", alignment.Id);
                var slGroup = alignment.GetSampleLineGroupIds().Add(slGroupId);

                // compute sample line
                double stationX = 0, stationY = 0;

                alignment.PointLocation(station, righttWidth, ref stationX, ref stationY);
                var rightPoint = new aGeom.Point2d(stationX, stationY);

                alignment.PointLocation(station, -1 * leftWidth, ref stationX, ref stationY);
                var leftPoint = new aGeom.Point2d(stationX, stationY);

                var slPoints = new aGeom.Point2dCollection() { rightPoint, leftPoint };

                // create sample line
                var sampleLineId = cDB.SampleLine.Create("STA-100", slGroupId, slPoints);

                ts.Commit();
            }
        }

    }
}