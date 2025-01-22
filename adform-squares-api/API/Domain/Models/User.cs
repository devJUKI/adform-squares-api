using API.Domain.Exceptions;

namespace API.Domain.Models;

public class User
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Name { get; }

    public User(string name)
    {
        if (string.IsNullOrEmpty(name))
            throw new ValidationException("Name can't be null or empty");

        Name = name;
    }

    public User(Guid id, string name) : this(name)
    {
        Id = id;
    }
}
