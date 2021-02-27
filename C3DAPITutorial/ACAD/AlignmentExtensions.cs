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
    public static class AlignmentExtensions
    {

        public static aDB.ObjectId AddSampleLineGroup(this cDB.Alignment alignment, string sampleGroupName, out string validName)
        {
            if (string.IsNullOrEmpty(sampleGroupName))
            {
                sampleGroupName = "SL - Group";
            }

            //get existing names
            var existedNames = alignment.GetAllSampleLineGroupNames();
            // validate SampleLineGroup name to avoid an exception.
            validName = ACAD.General.GetNonDuplicateName(existedNames, sampleGroupName);
            // create new sampleline group
            var slGroupId = cDB.SampleLineGroup.Create(validName, alignment.Id);
            alignment.GetSampleLineGroupIds().Add(slGroupId);

            return slGroupId;
        }

        public static void AddSampleLines(this cDB.Alignment alignment, 
            aDB.ObjectId sampleGroupNameId, string sampleGroupName,
            IEnumerable<double> stations, double leftWidth, double rightWidth)
        {
            double stationX = 0, stationY = 0;

            foreach (var station in stations)
            {
                // calculate left point
                alignment.PointLocation(station, rightWidth, ref stationX, ref stationY);
                var rightPoint = new aGeom.Point2d(stationX, stationY);
                // calculate right point
                alignment.PointLocation(station, -1 * leftWidth, ref stationX, ref stationY);
                var leftPoint = new aGeom.Point2d(stationX, stationY);

                var slPoints = new aGeom.Point2dCollection() { rightPoint, leftPoint };
                // create sample line

                var sampleLineId = cDB.SampleLine.Create($"{sampleGroupName}-{station:f2}", sampleGroupNameId, slPoints);
            }
        }

        public static string[] GetAllSampleLineGroupNames(this cDB.Alignment alignment)
        {
            if (alignment is null)
            {
                return new string[0];
            }

            return
                 alignment
                .GetSampleLineGroupIds()
                .Cast<aDB.ObjectId>()
                .Select(id => id.GetCivilNameOrNull())
                .ToArray();
        }

    }
}


//public static IEnumerable<string> GetAllSampleLineGroupNames(this cDB.Alignment alignment)
//{
//    foreach (aDB.ObjectId id in alignment.GetSampleLineGroupIds())
//    {
//        if (C3DEntityExtensions.GetNameOrNull(id, out string name))
//        {
//            yield return name;
//        }
//    }
//}