using AutoMapper;
using DemoEventi.Application.DTOs;
using DemoEventi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoEventi.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.InterestIds, opt => opt.MapFrom(src => src.Interests.Select(i => i.Id)));
        CreateMap<CreateUserDto, User>()
            .ForMember(dest => dest.Interests, opt => opt.Ignore());

        CreateMap<Event, EventDto>()
            .ForMember(dest => dest.ParticipantIds, opt => opt.MapFrom(src => src.Participants.Select(u => u.Id)));
        CreateMap<CreateEventDto, Event>()
            .ForMember(dest => dest.Participants, opt => opt.Ignore());
    }
}