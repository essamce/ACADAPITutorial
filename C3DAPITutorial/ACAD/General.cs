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
    public static class General
    {
        public static string GetNonDuplicateName(string[] names, string name)
        {
            if ((names is null) || (names.Length < 1))
            {
                return name;
            }

            while (names.Contains(name))
            {
                name += "(2)";
            }

            return name;
        }

        /// <summary>
        /// it works only for step > zero.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="step"></param>
        /// <param name="epsilon"></param>
        /// <returns></returns>
        public static IEnumerable<double> GetRange(double start, double end, double step, double epsilon)
        {
            //if (start > (end + epsilon))
            //{
            //    // invalid edges
            //    yield break; // == return
            //}

            double i = start;
            for (; i < (end + epsilon); i += step)
            {
                yield return i;
            }

            // add end if not added
            if (Math.Abs(i - step) < (end - epsilon))
            {
                yield return end;
            }

        }


    }
}
