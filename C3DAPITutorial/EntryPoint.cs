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
        [CommandMethod("CreateSamplesCmd")]
        public void CreateSamplesCmd()
        {
            // get currunt document 
            var acDoc = aApp.Application.DocumentManager.MdiActiveDocument;
            var database = acDoc.Database;
            var editor = acDoc.Editor;
            var civilDoc = cApp.CivilApplication.ActiveDocument;
            double accuracy = 0.001;

            //----------------------------------- Step 10:  get user inputs ----------------------------------------------------- 
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

            // Ask the user to enter numbers
            double leftWidth = 25;
            double righttWidth = 35;
            double interval = 20;
            // left width
            var leftWidthPrompt = editor.GetDistance("\n Enter Left Width:");
            if (leftWidthPrompt.Status != PromptStatus.OK)
            {
                editor.WriteMessage("\nNo Distance found");
                return;
            }
            // left width
            var righttWidthPrompt = editor.GetDistance("\n Enter Right Width:");
            if (righttWidthPrompt.Status != PromptStatus.OK)
            {
                editor.WriteMessage("\nNo Distance found");
                return;
            }
            // interval
            var intervalPrompt = editor.GetDistance("\n Enter sample lines interval:");
            if (intervalPrompt.Status != PromptStatus.OK)
            {
                editor.WriteMessage("\nNo Distance found");
                return;
            }

            // get numbers
            leftWidth = leftWidthPrompt.Value;
            righttWidth = righttWidthPrompt.Value;
            interval = intervalPrompt.Value;

            using (var ts = database.TransactionManager.StartTransaction())
            {
                //----------------------------------- Step20:  get alignment ---------------------------------------------------
                var alignment = promptAlignment.ObjectId.GetObject(aDB.OpenMode.ForWrite) as cDB.Alignment;

                //----------------------------------- Step30: create sample line group -----------------------------------------
                var slGroupId = cDB.SampleLineGroup.Create("SL - Group2", alignment.Id);
                var slGroup = alignment.GetSampleLineGroupIds().Add(slGroupId);

                //----------------------------------- Step30: create sample line  ----------------------------------------------
                // compute sample line
                double station = alignment.StartingStation, stationX = 0, stationY = 0;

                bool endStationAdded = false;
                while (station < (alignment.EndingStation + accuracy))
                {
                    // calculate left point
                    alignment.PointLocation(station, righttWidth, ref stationX, ref stationY);
                    var rightPoint = new aGeom.Point2d(stationX, stationY);
                    // calculate right point
                    alignment.PointLocation(station, -1 * leftWidth, ref stationX, ref stationY);
                    var leftPoint = new aGeom.Point2d(stationX, stationY);

                    var slPoints = new aGeom.Point2dCollection() { rightPoint, leftPoint };
                    // create sample line
                    var sampleLineId = cDB.SampleLine.Create($"STA-{station:f2}", slGroupId, slPoints);

                    // loop increment
                    station += interval;
                    if ((station > alignment.EndingStation) && (endStationAdded == false))
                    {
                        station = alignment.EndingStation;
                        endStationAdded = true;
                    }
                }

                ts.Commit();
            }
        }
    }
}