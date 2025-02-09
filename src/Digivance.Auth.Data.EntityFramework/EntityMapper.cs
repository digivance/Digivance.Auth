using AutoMapper;

namespace Digivance.Auth.Data.EntityFramework
{
    /// <summary>
    /// Each entity should implement a copy of this to ensure that we can convert
    /// to it's eqivalent DTO model
    /// </summary>
    public interface IEntityMapperConfiguration
    {
        /// <summary>
        /// Use this to configure automapper for your entity
        /// </summary>
        /// <param name="cfg">The mapper configuration builder to use</param>
        public void Configure(IMapperConfigurationExpression cfg);
    }

    /// <summary>
    /// EntityMapper is a helper that should be registered with DI and/or used as a 
    /// singleton. When creating this class we will reflect and invoke all the 
    /// IEntityMapperConfiguration.Configure implementations to configure our automapper
    /// object.  Afterwhich you can access the Automapper via the .Mapper property,
    /// or simply call the Map function directly
    /// </summary>
    public class EntityMapper
    {
        /// <summary>
        /// Reference to the AutoMapper that we have configured based on all implementations
        /// of IEntityMapperConfiguration
        /// </summary>
        public IMapper Mapper { get; set; }

        /// <summary>
        /// Constructor will reflect and instantiate / invoke all configure methods
        /// </summary>
        public EntityMapper()
        {
            var type = typeof(IEntityMapperConfiguration);
            var entityConfigs = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => type.IsAssignableFrom(x) && x.IsClass);

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                foreach (var entityConfig in entityConfigs)
                {
                    IEntityMapperConfiguration config = (IEntityMapperConfiguration)Activator.CreateInstance(entityConfig);
                    config?.Configure(cfg);
                }
            });

            Mapper = new Mapper(mapperConfig);
        }

        public TOUT Map<TOUT>(object entity)
            => Mapper.Map<TOUT>(entity);

        public TOUT Map<TIN, TOUT>(TIN entity)
            => Mapper.Map<TIN, TOUT>(entity);
    }
}
