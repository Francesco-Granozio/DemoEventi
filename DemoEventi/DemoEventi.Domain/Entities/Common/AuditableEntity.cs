namespace DemoEventi.Domain.Common;

/// <summary>
/// Base abstract class providing auditing fields.
/// </summary>
public abstract class AuditableEntity<T> : IEntity<T>
{
    public T Id { get; set; }
    public DateTime DataOraCreazione { get; set; }
    public DateTime? DataOraModifica { get; set; }
}