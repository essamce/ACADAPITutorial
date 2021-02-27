using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using aDB = Autodesk.AutoCAD.DatabaseServices;
using aApp = Autodesk.AutoCAD.ApplicationServices;
using aEditor = Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using aGeom = Autodesk.AutoCAD.Geometry;

namespace ACAD
{
    public class Interaction
    {
        #region select single obj
        /// <summary>
        /// Asks the user to select an object of type <see cref="aDB.Entity"/>
        /// </summary>
        /// <typeparam name="TACAD">ACAD entity type</typeparam>
        /// <param name="objRef">a ref to selected object</param>
        /// <returns></returns>
        public static bool TrySelectObject<TACAD>(out aDB.ObjectId objId) 
        {
            // 
            var type = typeof(TACAD);
            var promptOptions = new aEditor.PromptEntityOptions($"\nSelect a {type.Name}");
            promptOptions.SetRejectMessage($"\nObject must be a {type.Name}.");
            promptOptions.AddAllowedClass(type, false);

            var promptResult = CurrentDrawing.Doc.Editor.GetEntity(promptOptions);
            if (promptResult.Status == aEditor.PromptStatus.OK)
            {
                objId = promptResult.ObjectId;
                return true;
            }
            else
            {
                ACAD.CurrentDrawing.Editor.WriteMessage($"\n ----- No {type.Name} found -----");
                objId = aDB.ObjectId.Null;
                return false;
            }
        }
        #endregion

        #region user input
        /// <summary>
        /// asking ACAD user for a distance.
        /// </summary>
        /// <param name="distance"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool AskForDisatance(out double distance, string message)
        {
            // 
            var distanceAskingResult = CurrentDrawing.Editor.GetDistance($"\n{message}:");
            if (distanceAskingResult.Status == aEditor.PromptStatus.OK) // prompt succeed
            {
                distance = distanceAskingResult.Value;
                return true;
            }
            else // prompt failed
            {
                ACAD.CurrentDrawing.Editor.WriteMessage($"\n ----- No Distance found -----");
                distance = 0;
                return false;
            }
        }
        /// <summary>
        /// asking ACAD user for a string.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool AskForString(out string text, string message)
        {
            // 
            var distanceAskingResult = CurrentDrawing.Editor.GetString($"\n{message}:");
            if (distanceAskingResult.Status == aEditor.PromptStatus.OK) // prompt succeed
            {
                text = distanceAskingResult.StringResult;
                return true;
            }
            else // prompt failed
            {
                ACAD.CurrentDrawing.Editor.WriteMessage($"\n ----- No text found -----");
                text = string.Empty;
                return false;
            }
        }
        #endregion
    }
}

