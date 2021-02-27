using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using aDB = Autodesk.AutoCAD.DatabaseServices;
using cDB = Autodesk.Civil.DatabaseServices;
using aApp = Autodesk.AutoCAD.ApplicationServices;
using aEditor = Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using aGeom = Autodesk.AutoCAD.Geometry;

namespace ACAD
{
    public static class C3DEntityExtensions
    {
        #region private
        public static bool IsC3DObject(this aDB.ObjectId id)
        {
            if ((id.IsValid == false) || (id.IsErased == true))
            {
                return false;
            }
            else
            {
                return id.ObjectClass.IsDerivedFrom(RXClass.GetClass(typeof(cDB.Entity)));
            }
        }
        #endregion

        #region get property
        public static string GetCivilNameOrNull(this aDB.ObjectId id)
        {
            if (IsC3DObject(id) == false)
            {
                return null;
            }

            using (var ts = ACAD.CurrentDrawing.TransM.StartTransaction())
            {
                var civilObj = id.GetObject(aDB.OpenMode.ForRead) as cDB.Entity;
                if (civilObj is null)
                {
                    ts.Abort();
                    return null;
                }
                else
                {
                    return civilObj.Name;
                }
            }

        }


        #endregion

    }
}
