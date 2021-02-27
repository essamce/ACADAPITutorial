using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using aDB = Autodesk.AutoCAD.DatabaseServices;
using aApp = Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using aEditor = Autodesk.AutoCAD.EditorInput;
using aGeom = Autodesk.AutoCAD.Geometry;
using cDB = Autodesk.Civil.DatabaseServices;
using cApp = Autodesk.Civil.ApplicationServices;

namespace HellooC3D.ACAD
{
    public static class Selection
    {
        /// <summary>
        /// returns true if succeed.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectId"></param>
        /// <returns></returns>
        public static bool TrySelectObject<T>(out aDB.ObjectId objectId)
        {
            var options = new PromptEntityOptions($"\nSelect an {typeof(T).Name}");
            options.SetRejectMessage($"\nObject must be a {typeof(T).Name}");
            options.AddAllowedClass(typeof(T), false);
            var result = aApp.Application.DocumentManager.MdiActiveDocument.Editor.GetEntity(options);
            if (result.Status == PromptStatus.OK)
            {
                objectId = result.ObjectId;
                return true;
            }
            else
            {
                aApp.Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage("\nNo Selection found");
                objectId = aDB.ObjectId.Null ;
                return false;
            }
        }
    }
}
