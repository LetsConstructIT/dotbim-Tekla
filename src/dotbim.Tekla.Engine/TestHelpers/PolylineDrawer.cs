using dotbim.Tekla.Engine.Entities;
using System.Linq;
using TSG = Tekla.Structures.Geometry3d;
using TSMUI = Tekla.Structures.Model.UI;

namespace dotbim.Tekla.Engine.TestHelpers
{
    public class PolylineDrawer
    {
        private TSMUI.Color _red = new TSMUI.Color(1, 0, 0);
        private TSMUI.Color _green = new TSMUI.Color(0, 1, 0);

        public void Draw(Solid solid)
        {
            var drawer = new TSMUI.GraphicsDrawer();

            foreach (var face in solid.Faces)
            {
                drawer.DrawPolyLine(Transform(face.Contour, _green));
                foreach (var hole in face.Holes)
                {
                    drawer.DrawPolyLine(Transform(hole, _red));
                }
            }
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
    }
}
