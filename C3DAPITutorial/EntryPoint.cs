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
        [CommandMethod("CreateAlignmetByPolylineCmd")]
        public void CreateAlignmetByPolylineCmd()
        {
            // get currunt document 
            var acDoc = aApp.Application.DocumentManager.MdiActiveDocument;
            var database = acDoc.Database;
            var editor = acDoc.Editor;
            var civilDoc = cApp.CivilApplication.ActiveDocument;

            // Ask the user to select a polyline to convert to an alignment
            var opt = new PromptEntityOptions("\nSelect a polyline to convert to an Alignment");
            opt.SetRejectMessage("\nObject must be a polyline.");
            opt.AddAllowedClass(typeof(aDB.Polyline), false);
            var psr = editor.GetEntity(opt);
            if (psr.Status != PromptStatus.OK)
            {
                editor.WriteMessage("\nNo Selection found");
                return;
            }

            // create some polyline options for creating the new alignment
            var polylineOptions = new cDB.PolylineOptions()
            {
                AddCurvesBetweenTangents = true,
                EraseExistingEntities = false,
                PlineId = psr.ObjectId,
            };

            // create a new alignment
            var testAlignmentID = cDB.Alignment.Create(civilDoc, polylineOptions, "New Alignment 1", "", "0", "Basic", "_No Labels");


            //  the next line is identical to the previouse one, we just write it in declarative way
            //var testAlignmentID = cDB.Alignment.Create(civilDoc,
            //        plineOptions: polylineOptions,
            //        alignmentName: "New Alignment 1",
            //        siteName: "",
            //        layerName: "0",
            //        styleName: "Basic",
            //        labelSetName: "_No Labels");
        }


        [CommandMethod("CreateAlignmetByPolylineCmd2")]
        public void CreateAlignmetByPolylineCmd2()
        {
            // get currunt document 
            var acDoc = aApp.Application.DocumentManager.MdiActiveDocument;
            var database = acDoc.Database;
            var editor = acDoc.Editor;
            var civilDoc = cApp.CivilApplication.ActiveDocument;

            // Ask the user to select a polyline to convert to an alignment
            var opt = new PromptEntityOptions("\nSelect a polyline to convert to an Alignment");
            opt.SetRejectMessage("\nObject must be a polyline.");
            opt.AddAllowedClass(typeof(aDB.Polyline), false);
            var psr = editor.GetEntity(opt);
            if (psr.Status != PromptStatus.OK)
            {
                editor.WriteMessage("\nNo Selection found");
                return;
            }

            // create some polyline options for creating the new alignment
            var polylineOptions = new cDB.PolylineOptions()
            {
                AddCurvesBetweenTangents = true,
                EraseExistingEntities = false,
                PlineId = psr.ObjectId,
            };

            // get layer "0" id
            var layerId = database.LayerZero;

            // get an AlignmentStyle id and check if the drawings has no AlignmentStyles.
            aDB.ObjectId alignmentStyleId;
            if (civilDoc.Styles.AlignmentStyles.Count > 0)
            {
                alignmentStyleId = civilDoc.Styles.AlignmentStyles[0];
            }
            else
            {
                editor.WriteMessage("\n No Alignments styles found");
                return;
            }

            // get an AlignmentLabelSetStyle and check if the drawings has no AlignmentLabelSetStyles.
            aDB.ObjectId alignmentLablesSetId;
            if (civilDoc.Styles.LabelSetStyles.AlignmentLabelSetStyles.Count > 0)
            {
                alignmentLablesSetId = civilDoc.Styles.LabelSetStyles.AlignmentLabelSetStyles[0];
            }
            else
            {
                editor.WriteMessage("\n No Alignments label set found");
                return;
            }

            // create a new alignment
            var testAlignmentID = cDB.Alignment.Create(civilDoc, polylineOptions, "New Alignment 11", aDB.ObjectId.Null, layerId, alignmentStyleId, alignmentLablesSetId);
        }

    }
}