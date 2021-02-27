using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using aDB = Autodesk.AutoCAD.DatabaseServices;
using aApp = Autodesk.AutoCAD.ApplicationServices;
using cApp = Autodesk.Civil.ApplicationServices;
using aEditor = Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using aGeom = Autodesk.AutoCAD.Geometry;

namespace ACAD
{
    public class CurrentDrawing
    {
        /// <summary>
        /// <i>Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument</i>
        /// </summary>
        public static aApp.Document Doc;
        /// <summary>
        /// <i>Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument</i>
        /// </summary>
        public static cApp.CivilDocument CivilDoc;
        /// <summary>
        /// <i>Autodesk.Civil.ApplicationServices.CivilApplication.ActiveDocument</i>
        /// </summary>
        public static aDB.Database Database;
        /// <summary>
        /// <i>Autodesk.AutoCAD.DatabaseServices.HostApplicationServices.WorkingDatabase.TransactionManager</i>
        /// </summary>
        public static aDB.TransactionManager TransM;
        /// <summary>
        /// <i>Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor</i>
        /// </summary>
        public static aEditor.Editor Editor => Doc.Editor;
        public static aDB.ObjectId ModelSpaceId => Database.CurrentSpaceId;

        //public static aDB.BlockTableRecord ModelSpace;
        //static bool modelSpaceOpened = false;
        static CurrentDrawing()
        {
            Doc = aApp.Application.DocumentManager.MdiActiveDocument;
            CivilDoc = cApp.CivilApplication.ActiveDocument;
            Database = Doc.Database;
            TransM = aDB.HostApplicationServices.WorkingDatabase.TransactionManager;

            //if (modelSpaceOpened == false)
            //{
            //    modelSpaceOpened = true;
            //}


        }


    }
}
