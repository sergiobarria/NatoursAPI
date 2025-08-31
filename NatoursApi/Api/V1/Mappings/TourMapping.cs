using Mapster;
using NatoursApi.Domain.Entities;

namespace NatoursApi.Api.V1.Mappings;

public class TourMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<TourStartDate, DateTime>()
            .MapWith(sd => sd.Date);
    }
}