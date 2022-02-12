using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CreationModelPlugin
{
    public static class DoorHelper
    {
        public static FamilyInstance CreateDoorInMiddle(Document doc, string level1Name, Wall wall)
        {
            List<Level> listLevel = new FilteredElementCollector(doc)
            .OfClass(typeof(Level))
            .OfType<Level>()
            .ToList();

            Level level1 = listLevel
                .Where(x => x.Name.Equals(level1Name))
                .FirstOrDefault();

            if (level1 == null)
            {
                TaskDialog.Show("Ошибка", "Базового уровня с таким именем не существует");
                return null;
            }

            FamilySymbol doorType = new FilteredElementCollector(doc)
                .OfClass(typeof(FamilySymbol))
                .OfCategory(BuiltInCategory.OST_Doors)
                .OfType<FamilySymbol>()
                .Where(x => x.Name.Equals("0915 x 2134 мм"))
                .Where(x => x.FamilyName.Equals("Одиночные-Щитовые"))
                .FirstOrDefault();

            LocationCurve locationCurve = wall.Location as LocationCurve;
            XYZ point1 = locationCurve.Curve.GetEndPoint(0);
            XYZ point2 = locationCurve.Curve.GetEndPoint(1);
            XYZ point = (point1 + point2) / 2;

            if (!doorType.IsActive)
            {
                doorType.Activate();
            }

            FamilyInstance door = doc.Create.NewFamilyInstance(point, doorType, wall, level1, StructuralType.NonStructural);

            return door;
        }
    }
}