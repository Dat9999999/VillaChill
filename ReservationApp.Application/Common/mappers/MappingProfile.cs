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
    }
}