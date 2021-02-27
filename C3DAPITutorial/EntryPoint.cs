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
        [CommandMethod("PrintPointGroupNumberOfPoint")]
        public void PrintPointGroupNumberOfPoint()
        {

            var ptGroupsIDs = ACAD.CurrentDrawing.CivilDoc.PointGroups;
            if (ptGroupsIDs.Count < 1)
            {
                ACAD.CurrentDrawing.Editor.WriteMessage($"\n-----No Point groups found");
                return;
            }

            var ptGroupsNames = new List<string>();
            var ptGroupCounts = new List<uint>();

            using (var ts = ACAD.CurrentDrawing.TransM.StartTransaction())
            {
                foreach (var groupId in ptGroupsIDs)
                {
                    var ptGroup = groupId.GetObject(aDB.OpenMode.ForRead) as cDB.PointGroup;
                    ptGroupsNames.Add(ptGroup.Name);
                    ptGroupCounts.Add(ptGroup.PointsCount);
                    //var name = C3DEntityExtensions.GetCivilNameOrNull(groupId);
                }

            }

            var pKeyOpts = new PromptKeywordOptions("");
            pKeyOpts.Message = "\nEnter Point group name";
            ptGroupsNames.ForEach(name => pKeyOpts.Keywords.Add(name));
            pKeyOpts.Keywords.Default = ptGroupsNames.First();
            pKeyOpts.AllowNone = true;
            var keyWordRes = ACAD.CurrentDrawing.Editor.GetKeywords(pKeyOpts);
            if (keyWordRes.Status != PromptStatus.OK)
            {
                ACAD.CurrentDrawing.Editor.WriteMessage($"\n-----No names found");
                return;
            }

            var selectedName = keyWordRes.StringResult;
            //var index = ptGroupsNames.IndexOf(selectedName);
            //var index = ptGroupsNames.FindIndex((name) => name.Equals(selectedName));
            //var groupPtCount = ptGroupCounts[index];

            //int i = 0;
            //for (; i < ptGroupsNames.Count; i++)
            //{
            //    if (ptGroupsNames[i] == selectedName)
            //    {
            //        break;
            //    }
            //}
            //var groupPtCount = ptGroupCounts[i];

            //ACAD.CurrentDrawing.Editor.WriteMessage($"\n-----no of points:  <{groupPtCount}>");





        }




    }
}



//var pKeyOpts = new PromptKeywordOptions("");
//pKeyOpts.Message = "\nEnter an option ";
//pKeyOpts.Keywords.Add("Line");
//pKeyOpts.Keywords.Add("Circle");
//pKeyOpts.Keywords.Add("Arc");
//pKeyOpts.Keywords.Default = "Arc";
//pKeyOpts.AllowNone = true;

//PromptResult pKeyRes = acDoc.Editor.GetKeywords(pKeyOpts);

//Application.ShowAlertDialog("Entered keyword: " +
//                            pKeyRes.StringResult);