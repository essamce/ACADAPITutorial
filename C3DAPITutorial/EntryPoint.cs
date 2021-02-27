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
        [CommandMethod("HelloC3D")]
        public void SendMessageToAcad()
        {
            // get currunt document 
            var aDocument = aApp.Application.DocumentManager.MdiActiveDocument;
            var database = aDocument.Database;
            var editor = aDocument.Editor;
            var cDocument = cApp.CivilApplication.ActiveDocument;

            // send message to ACAD
            editor.WriteMessage("Hello from c3d api");

        }

    }
}