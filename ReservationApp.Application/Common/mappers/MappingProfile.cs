using AutoMapper;
using AutoMapper.Configuration.Annotations;
using ReservationApp.Domain.Entities;
using ReservationApp.ViewModels;

namespace ReservationApp.Application.Common.mappers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<RatingRequestDTO, Rating>().ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Name));
        CreateMap<CommissionRateRequestDTO, CommissionRate>();
        CreateMap<OwnerSettlementDTO, OwnerSettlement>();
        CreateMap<OwnerSettlement, OwnerSettlementDTO>()
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => (double)src.Amount))
            .ForMember(dest => dest.CommissionRate,
                opt => opt.MapFrom(src => src.CommissionRate.HasValue ? (double?)src.CommissionRate : null));
    }
}