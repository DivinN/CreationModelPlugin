using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreationModelPlugin
{
    [Transaction(TransactionMode.Manual)]
    public class CreationModel : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;

            // Создание стен по прямоугольнику
            List<Wall> newWalls = WallsHelper.CreateWallsAlongRectangle(doc, "Уровень 1", "Уровень 2", 10000, 5000);

            return Result.Succeeded;
        }
    }
}
