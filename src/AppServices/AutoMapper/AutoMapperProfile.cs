using AutoMapper;
using SWGW.AppServices.EntryActions.Dto;
using SWGW.AppServices.EntryTypes;
using SWGW.AppServices.Offices;
using SWGW.AppServices.Staff.Dto;
using SWGW.AppServices.WorkEntries.CommandDto;
using SWGW.AppServices.WorkEntries.QueryDto;
using SWGW.Domain.Entities.EntryActions;
using SWGW.Domain.Entities.EntryTypes;
using SWGW.Domain.Entities.Offices;
using SWGW.Domain.Entities.WorkEntries;
using SWGW.Domain.Identity;

namespace SWGW.AppServices.AutoMapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<ApplicationUser, StaffSearchResultDto>();
        CreateMap<ApplicationUser, StaffViewDto>();

        CreateMap<EntryAction, EntryActionUpdateDto>();
        CreateMap<EntryAction, EntryActionViewDto>();

        CreateMap<EntryType, EntryTypeUpdateDto>();
        CreateMap<EntryType, EntryTypeViewDto>();

        CreateMap<Office, OfficeUpdateDto>();
        CreateMap<Office, OfficeViewDto>();

        CreateMap<WorkEntry, WorkEntrySearchResultDto>();
        CreateMap<WorkEntry, WorkEntryCreateDto>();
        CreateMap<WorkEntry, WorkEntryUpdateDto>();
        CreateMap<WorkEntry, WorkEntryViewDto>();
    }
}
