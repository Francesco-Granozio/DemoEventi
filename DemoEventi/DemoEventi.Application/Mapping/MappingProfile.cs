using AutoMapper;
using DemoEventi.Application.DTOs;
using DemoEventi.Domain.Entities;

namespace DemoEventi.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // User mappings
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.InterestIds, opt => opt.MapFrom(src => src.Interests.Select(i => i.Id)));
        
        CreateMap<CreateUserDto, User>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Interests, opt => opt.Ignore())
            .ForMember(dest => dest.Events, opt => opt.Ignore())
            .ForMember(dest => dest.DataOraCreazione, opt => opt.Ignore())
            .ForMember(dest => dest.DataOraModifica, opt => opt.Ignore());

        // Event mappings
        CreateMap<Event, EventDto>()
            .ForMember(dest => dest.ParticipantIds, opt => opt.MapFrom(src => src.Participants.Select(u => u.Id)));
        
        CreateMap<CreateEventDto, Event>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Participants, opt => opt.Ignore())
            .ForMember(dest => dest.DataOraCreazione, opt => opt.Ignore())
            .ForMember(dest => dest.DataOraModifica, opt => opt.Ignore());

        // Interest mappings
        CreateMap<Interest, InterestDto>().ReverseMap();
    }
}