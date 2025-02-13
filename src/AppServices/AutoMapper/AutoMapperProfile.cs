using AutoMapper;
using SWGW.AppServices.ActionTypes;
using SWGW.AppServices.Offices;
using SWGW.AppServices.Staff.Dto;
using SWGW.AppServices.Permits.CommandDto;
using SWGW.AppServices.Permits.QueryDto;
using SWGW.Domain.Entities.PermitActions;
using SWGW.AppServices.PermitActions.Dto;
using SWGW.Domain.Entities.ActionTypes;
using SWGW.Domain.Entities.Offices;
using SWGW.Domain.Entities.Permits;
using SWGW.Domain.Identity;

namespace SWGW.AppServices.AutoMapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<ApplicationUser, StaffSearchResultDto>();
        CreateMap<ApplicationUser, StaffViewDto>();

        CreateMap<PermitAction, ActionUpdateDto>();
        CreateMap<PermitAction, ActionViewDto>();
        CreateMap<PermitAction, ActionSearchResultDto>();

        CreateMap<ActionType, ActionTypeUpdateDto>();
        CreateMap<ActionType, ActionTypeViewDto>();

        CreateMap<Office, OfficeUpdateDto>();
        CreateMap<Office, OfficeViewDto>();
        CreateMap<Office, OfficeWithAssignorDto>();

        CreateMap<Permit, PermitSearchResultDto>();
        CreateMap<Permit, PermitCreateDto>();   
        CreateMap<Permit, PermitUpdateDto>()
            .ForMember(dto => dto.ReceivedDate, expression =>
                expression.MapFrom(permit => DateOnly.FromDateTime(permit.ReceivedDate.Date)))
            .ForMember(dto => dto.ReceivedTime, expression =>
                expression.MapFrom(permit => TimeOnly.FromTimeSpan(permit.ReceivedDate.TimeOfDay)));
        CreateMap<Permit, PermitViewDto>();
    }
}
