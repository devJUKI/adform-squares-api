namespace API.Domain.DTOs;

public record PointDto(int X, int Y)
{
    public int DistanceSquared(PointDto other)
    {
        return (X - other.X) * (X - other.X) + (Y - other.Y) * (Y - other.Y);
    }
}
