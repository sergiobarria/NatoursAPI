using Mapster;
using NatoursApi.Domain.Entities;

namespace NatoursApi.Features.Tours.v1;

public class TourMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<TourStartDate, DateTime>()
            .MapWith(sd => sd.Date);
    }
}