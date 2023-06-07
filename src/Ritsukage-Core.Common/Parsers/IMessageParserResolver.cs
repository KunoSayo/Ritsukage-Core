namespace RUCore.Common.Parsers
{
    /// <summary>
    /// Message parser resolver
    /// </summary>
    /// <typeparam name="TRawdata"></typeparam>
    /// <typeparam name="TParserService"></typeparam>
    public interface IMessageParserResolver<TRawdata, out TParserService> where TParserService : IMessageParser
    {
        /// <summary>
        /// Resolve parser
        /// </summary>
        /// <param name="rawdata"></param>
        /// <returns></returns>
        TParserService? ResolveParser(in TRawdata rawdata);

        /// <summary>
        /// Resolve parsers
        /// </summary>
        /// <param name="rawdata"></param>
        /// <returns></returns>
        IEnumerable<TParserService> ResolveParsers(in TRawdata rawdata);
    }
}
