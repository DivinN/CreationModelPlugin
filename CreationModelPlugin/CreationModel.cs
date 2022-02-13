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

            Transaction transaction = new Transaction(doc, "Построение стен");
            transaction.Start();

            // Создание стен по прямоугольнику
            List<Wall> newWalls = WallsHelper.CreateWallsAlongRectangle(doc, "Уровень 1", "Уровень 2", 10000, 5000);

            FamilyInstance door = DoorHelper.CreateDoorInMiddle(doc, "Уровень 1", newWalls[0]);
            FamilyInstance window_0 = WindowHelper.CreateWindowInMiddle(doc, "Уровень 1", newWalls[1], 900);
            FamilyInstance window_1 = WindowHelper.CreateWindowInMiddle(doc, "Уровень 1", newWalls[2], 900);
            FamilyInstance window_2 = WindowHelper.CreateWindowInMiddle(doc, "Уровень 1", newWalls[3], 900);

            ExtrusionRoof roof = RoofHelper.CreateRoof(doc, "Уровень 2", newWalls[0], newWalls[1], 1000);

            transaction.Commit();


            return Result.Succeeded;
        }
    }
}
