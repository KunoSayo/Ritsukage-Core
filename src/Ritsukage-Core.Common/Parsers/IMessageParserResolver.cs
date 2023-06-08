namespace RUCore.Common.Parsers
{
    /// <summary>
    /// Message parser resolver
    /// </summary>
    /// <typeparam name="TRawData"></typeparam>
    /// <typeparam name="TParserService"></typeparam>
    public interface IMessageParserResolver<TRawData, out TParserService> where TParserService : IMessageParser
    {
        /// <summary>
        /// Resolve parser
        /// </summary>
        /// <param name="rawData"></param>
        /// <returns></returns>
        TParserService? ResolveParser(in TRawData rawData);

        /// <summary>
        /// Resolve parsers
        /// </summary>
        /// <param name="rawData"></param>
        /// <returns></returns>
        IEnumerable<TParserService> ResolveParsers(in TRawData rawData);
    }
}
