namespace API.Domain.Models;

public class Point
{
    public int Id { get; }
    public int X { get; }
    public int Y { get; }

    public Point(int Id, int X, int Y)
    {
        this.Id = Id;
        this.X = X;
        this.Y = Y;
    }
}
