using Microsoft.Extensions.DependencyInjection;
using RUCore.Common.Attributes;

namespace RUCore.Common.Parsers.Attributes
{
    /// <summary>
    /// Register a <see cref="IMessageParserResolver{TRawdata, TParserService}"/> to <see cref="IServiceCollection"/>
    /// </summary>
    public class RegisterParserResolverAttribute : RegisterBaseAttribute
    {
        /// <inheritdoc cref="RegisterParserResolverAttribute(Type, ServiceLifetime?)"/>
        public RegisterParserResolverAttribute(Type implementationType) : this(implementationType, null)
        {
        }

        /// <summary>
        /// Register a <see cref="IMessageParserResolver{TRawdata, TParserService}"/> to <see cref="IServiceCollection"/>
        /// </summary>
        /// <param name="implementationType"></param>
        /// <param name="lifetime"></param>
        public RegisterParserResolverAttribute(Type implementationType, ServiceLifetime? lifetime) : base(implementationType, lifetime)
        {
        }

        /// <summary>
        /// Gets the service type of given <paramref name="implementationType"/>
        /// </summary>
        /// <param name="implementationType"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        protected override Type GetServiceType(Type implementationType)
        {
            Type openGeneric = typeof(IMessageParserResolver<,>);
            foreach (Type interfaceType in implementationType.GetInterfaces())
                if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == openGeneric)
                    return interfaceType;
            throw new ArgumentException($"Given {implementationType.FullName} does not implement {openGeneric.FullName}", nameof(implementationType));
        }
    }
}
