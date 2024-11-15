using dotbim.Tekla.Engine.Entities;
using System.Linq;
using TSG = Tekla.Structures.Geometry3d;
using TSMUI = Tekla.Structures.Model.UI;

namespace dotbim.Tekla.Engine.TestHelpers;

public class PolylineDrawer
{
    private readonly TSMUI.Color _red = new TSMUI.Color(1, 0, 0);
    private readonly TSMUI.Color _green = new TSMUI.Color(0, 1, 0);

    private readonly TSMUI.GraphicsDrawer _drawer = new TSMUI.GraphicsDrawer();

    public void Draw(Solid solid)
    {
        foreach (var face in solid.Faces)
        {
            _drawer.DrawPolyLine(Transform(face.Contour, _green));
            foreach (var hole in face.Holes)
            {
                _drawer.DrawPolyLine(Transform(hole, _red));
            }
        }
    }

    public void Draw(Triangle triangle)
    {
        _drawer.DrawPolyLine(Transform(triangle, _green));
    }

    private TSMUI.GraphicPolyLine Transform(Polygon polygon, TSMUI.Color color)
    {
        var polyline = new TSG.PolyLine(polygon.Points);
        polyline.Points.Add(polygon.Points.First());
        return new TSMUI.GraphicPolyLine(color, 1, TSMUI.GraphicPolyLine.LineType.Solid)
        {
            PolyLine = polyline
        };
    }

    private TSMUI.GraphicPolyLine Transform(Triangle triangle, TSMUI.Color color)
    {
        var polyline = new TSG.PolyLine(new TSG.Point[]
        {
            triangle.Point1,
            triangle.Point2,
            triangle.Point3,
            triangle.Point1
        });
        return new TSMUI.GraphicPolyLine(color, 1, TSMUI.GraphicPolyLine.LineType.Solid)
        {
            PolyLine = polyline
        };
    }
}
