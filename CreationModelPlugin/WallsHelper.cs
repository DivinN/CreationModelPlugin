using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CreationModelPlugin
{
    public static class WallsHelper
    {
        public static List<Wall> CreateWallsAlongRectangle(Document doc, string level1Name, string level2Name, int widthInMm, int depthInMm)
        {
            List<Wall> walls = new List<Wall>();

            if (widthInMm == 0 || depthInMm == 0)
            {
                TaskDialog.Show("Ошибка", "Размеры сишком малы");
                return walls;
            }

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
                return walls;
            }

            Level level2 = listLevel
                .Where(x => x.Name.Equals(level2Name))
                .FirstOrDefault();
            
            if (level2 == null)
            {
                TaskDialog.Show("Ошибка", "Верхнего уровня с таким именем не существует");
                return walls;
            }



            double width = UnitUtils.ConvertToInternalUnits(widthInMm, UnitTypeId.Millimeters);
            double depth = UnitUtils.ConvertToInternalUnits(depthInMm, UnitTypeId.Millimeters);
            double dx = width / 2;
            double dy = depth / 2;

            List<XYZ> points = new List<XYZ>();
            points.Add(new XYZ(-dx, -dy, 0));
            points.Add(new XYZ(dx, -dy, 0));
            points.Add(new XYZ(dx, dy, 0));
            points.Add(new XYZ(-dx, dy, 0));
            points.Add(new XYZ(-dx, -dy, 0));

            Transaction transaction = new Transaction(doc, "Построение стен");
            transaction.Start();
            for (int i = 0; i < 4; i++)
            {
                Line line = Line.CreateBound(points[i], points[i + 1]);
                Wall wall = Wall.Create(doc, line, level1.Id, false);
                walls.Add(wall);
                wall.get_Parameter(BuiltInParameter.WALL_HEIGHT_TYPE).Set(level2.Id);
            }

            transaction.Commit();

            return walls;
        }
    }
}