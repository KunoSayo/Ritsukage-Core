namespace RUCore.Common.Parsers.Attributes
{
    /// <summary>
    /// Suppresses the automatic mapping of a class to a message.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class SuppressAutoMappingAttribute : Attribute
    { }
}
