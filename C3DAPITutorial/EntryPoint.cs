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
    //accoremgd.dll
    //acdbmgd.dll
    //acmgd.dll
    //AecBaseMgd.dll
    //AeccDbMgd.dll
     
    public class EntryPoint
    {
        [CommandMethod("GetAlignments")]
        public void GetAlignmetns()
        {
            // get currunt document 
            var aDocument = aApp.Application.DocumentManager.MdiActiveDocument;
            var database = aDocument.Database;
            var editor = aDocument.Editor;
            var cDocument = cApp.CivilApplication.ActiveDocument;

            // get alignmets from civil document
            var allAlignmentsIds = cDocument.GetAlignmentIds();

            // start transaction
            using (var ts = database.TransactionManager.StartTransaction())
            {
                foreach (aDB.ObjectId id in allAlignmentsIds)
                {
                    var alignmet = id.GetObject(aDB.OpenMode.ForRead) as cDB.Alignment;
                    editor.WriteMessage($"\nalignmet.Name: <{alignmet.Name,-15}>, alignmet.Length: <{alignmet.Length,-10}>");
                }

                ts.Commit();
            }

        }


        [CommandMethod("GetAlignmetnBySelection")]
        public void GetAlignmetnBySelection()
        {
            // get currunt document 
            var aDocument = aApp.Application.DocumentManager.MdiActiveDocument;
            var database = aDocument.Database;
            var editor = aDocument.Editor;
            var cDocument = cApp.CivilApplication.ActiveDocument;

            // get alignmet by selection
            var opt = new PromptEntityOptions("\nSelect an alignment");
            opt.SetRejectMessage("\nObject must be an alignmet.");
            opt.AddAllowedClass(typeof(cDB.Alignment), false);
            var psr = editor.GetEntity(opt);

            if (psr.Status != PromptStatus.OK)
            {
                editor.WriteMessage("\nNo Selection found");
                return;
            }

            // start transaction
            using (var ts = database.TransactionManager.StartTransaction())
            {
                var alignmet = psr.ObjectId.GetObject(aDB.OpenMode.ForRead) as cDB.Alignment;
                editor.WriteMessage($"\nalignmet.Name: <{alignmet.Name,-15}>, alignmet.Length: <{alignmet.Length,-10}>");

                ts.Commit();
            }

        }

    }
}