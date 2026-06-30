using System.Drawing;

namespace Server.Pantheon;

public class AlignmentZone
{
    private Deity.Alignment _alignment;
    private int _distance;
    private Point3D _location;

    public AlignmentZone(Deity.Alignment alignment, int distance, Point3D location)
    {
        _alignment = alignment;
        _distance = distance;
        _location = location;
    }

    public Deity.Alignment Alignment { get => _alignment; set =>  _alignment = value; }
    public int Distance { get => _distance; set => _distance = value; }
    public Point3D Location { get => _location; set => _location = value; }
}
