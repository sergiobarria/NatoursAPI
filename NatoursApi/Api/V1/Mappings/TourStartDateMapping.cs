using Mapster;
using NatoursApi.Api.V1.Dtos;
using NatoursApi.Domain.Entities;

namespace NatoursApi.Api.V1.Mappings;

public class TourStartDateMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<TourStartDate, TourStartDateDto>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Date, src => src.Date)
            .Map(dest => dest.TourId, src => src.TourId);
    }
}