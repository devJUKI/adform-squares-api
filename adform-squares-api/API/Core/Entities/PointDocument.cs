namespace API.Core.Entities;

public class PointDocument
{
    public int Id { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public Guid UserId { get; set; }
}
