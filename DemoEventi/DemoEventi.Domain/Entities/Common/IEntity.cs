namespace DemoEventi.Domain.Common;

/// <summary>
/// Base interface for all entities, exposing primary key.
/// </summary>
public interface IEntity<T>
{
    T Id { get; set; }
}