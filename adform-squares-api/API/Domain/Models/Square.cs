using API.Domain.Exceptions;
using API.Domain.DTOs;

namespace API.Domain.Models;

public class Square
{
    public PointDto[] Points { get; }

    public Square(PointDto[] points)
    {
        if (points.Length != 4)
            throw new ValidationException("A square must have exactly 4 points");

        Points = points;
    }
}
