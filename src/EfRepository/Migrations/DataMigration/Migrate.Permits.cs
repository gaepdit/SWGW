﻿namespace SWGW.EfRepository.Migrations.DataMigration;

public static partial class Migrate
{
    // language=sql
    public const string Complaints =
        """
        SET IDENTITY_INSERT dbo.Complaints ON;
        insert into dbo.Complaints
            (Id,
             Status,
             EnteredDate,
             EnteredById,
             ReceivedDate,
             ReceivedById,
             CallerName,
             CallerRepresents,
             CallerAddress_Street,
             CallerAddress_Street2,
             CallerAddress_City,
             CallerAddress_State,
             CallerAddress_PostalCode,
             CallerPhoneNumber_Number,
             CallerPhoneNumber_Type,
             CallerSecondaryPhoneNumber_Number,
             CallerSecondaryPhoneNumber_Type,
             CallerTertiaryPhoneNumber_Number,
             CallerTertiaryPhoneNumber_Type,
             CallerEmail,
             ComplaintNature,
             ComplaintLocation,
             ComplaintDirections,
             ComplaintCity,
             ComplaintCounty,
             PrimaryConcernId,
             SecondaryConcernId,
             SourceFacilityIdNumber,
             SourceFacilityName,
             SourceContactName,
             SourceAddress_Street,
             SourceAddress_Street2,
             SourceAddress_City,
             SourceAddress_State,
             SourceAddress_PostalCode,
             SourcePhoneNumber_Number,
             SourcePhoneNumber_Type,
             SourceSecondaryPhoneNumber_Number,
             SourceSecondaryPhoneNumber_Type,
             SourceTertiaryPhoneNumber_Number,
             SourceTertiaryPhoneNumber_Type,
             SourceEmail,
             CurrentOfficeId,
             CurrentOwnerId,
             CurrentOwnerAssignedDate,
             CurrentOwnerAcceptedDate,
             ReviewedById,
             ReviewComments,
             ComplaintClosed,
             ComplaintClosedDate,
             DeleteComments,
             CreatedAt,
             CreatedById,
             UpdatedAt,
             UpdatedById,
             IsDeleted,
             DeletedAt,
             DeletedById)
        select c.Id                                                                 as Id,
               case
                   when c.Status = 0 then 'New'
                   when c.Status = 1 then 'UnderInvestigation'
                   when c.Status = 2 then 'ReviewPending'
                   when c.Status = 3 then 'Closed'
                   when c.Status = 4 then 'AdministrativelyClosed'
               end                                                                  as Status,
               c.DateEntered at time zone 'Eastern Standard Time'                   as DateEntered,
               dbo.FixUserId(c.EnteredById)                                         as EnteredById,
               c.DateReceived at time zone 'Eastern Standard Time'                  as DateReceived,
               dbo.FixUserId(c.ReceivedById)                                        as ReceivedById,
               trim(c.CallerName)                                                   as CallerName,
               trim(c.CallerRepresents)                                             as CallerRepresents,
               trim(c.CallerStreet)                                                 as CallerStreet,
               trim(c.CallerStreet2)                                                as CallerStreet2,
               trim(c.CallerCity)                                                   as CallerCity,
               s1.Name                                                              as CallerState,
               trim(c.CallerPostalCode)                                             as CallerPostalCode,
               trim(c.CallerPhoneNumber)                                            as CallerPhoneNumber,
               case
                   when c.CallerPhoneType = 0 then 'Cell'
                   when c.CallerPhoneType = 1 then 'Fax'
                   when c.CallerPhoneType = 2 then 'Home'
                   when c.CallerPhoneType = 3 then 'Office'
               end                                                                  as CallerPhoneType,
               trim(c.CallerSecondaryPhoneNumber)                                   as CallerSecondaryPhoneNumber,
               case
                   when c.CallerSecondaryPhoneType = 0 then 'Cell'
                   when c.CallerSecondaryPhoneType = 1 then 'Fax'
                   when c.CallerSecondaryPhoneType = 2 then 'Home'
                   when c.CallerSecondaryPhoneType = 3 then 'Office'
               end                                                                  as CallerSecondaryPhoneType,
               trim(c.CallerTertiaryPhoneNumber)                                    as CallerTertiaryPhoneNumber,
               case
                   when c.CallerTertiaryPhoneType = 0 then 'Cell'
                   when c.CallerTertiaryPhoneType = 1 then 'Fax'
                   when c.CallerTertiaryPhoneType = 2 then 'Home'
                   when c.CallerTertiaryPhoneType = 3 then 'Office'
               end                                                                  as CallerTertiaryPhoneType,
               trim(c.CallerEmail)                                                  as CallerEmail,
               trim(CHAR(13) + CHAR(10) + CHAR(9) + ' ' from c.ComplaintNature)     as ComplaintNature,
               trim(CHAR(13) + CHAR(10) + CHAR(9) + ' ' from c.ComplaintLocation)   as ComplaintLocation,
               trim(CHAR(13) + CHAR(10) + CHAR(9) + ' ' from c.ComplaintDirections) as ComplaintDirections,
               trim(c.ComplaintCity)                                                as ComplaintCity,
               u.Name                                                               as ComplaintCounty,
               lower(c.PrimaryConcernId)                                            as PrimaryConcernId,
               lower(c.SecondaryConcernId)                                          as SecondaryConcernId,
               trim(c.SourceFacilityId)                                             as SourceFacilityId,
               trim(c.SourceFacilityName)                                           as SourceFacilityName,
               trim(c.SourceContactName)                                            as SourceContactName,
               trim(c.SourceStreet)                                                 as SourceStreet,
               trim(c.SourceStreet2)                                                as SourceStreet2,
               trim(c.SourceCity)                                                   as SourceCity,
               s2.Name                                                              as SourceState,
               trim(c.SourcePostalCode)                                             as SourcePostalCode,
               trim(c.SourcePhoneNumber)                                            as SourcePhoneNumber,
               case
                   when c.SourcePhoneType = 0 then 'Cell'
                   when c.SourcePhoneType = 1 then 'Fax'
                   when c.SourcePhoneType = 2 then 'Home'
                   when c.SourcePhoneType = 3 then 'Office'
               end                                                                  as SourcePhoneType,
               trim(c.SourceSecondaryPhoneNumber)                                   as SourceSecondaryPhoneNumber,
               case
                   when c.SourceSecondaryPhoneType = 0 then 'Cell'
                   when c.SourceSecondaryPhoneType = 1 then 'Fax'
                   when c.SourceSecondaryPhoneType = 2 then 'Home'
                   when c.SourceSecondaryPhoneType = 3 then 'Office'
               end                                                                  as SourceSecondaryPhoneType,
               trim(c.SourceTertiaryPhoneNumber)                                    as SourceTertiaryPhoneNumber,
               case
                   when c.SourceTertiaryPhoneType = 0 then 'Cell'
                   when c.SourceTertiaryPhoneType = 1 then 'Fax'
                   when c.SourceTertiaryPhoneType = 2 then 'Home'
                   when c.SourceTertiaryPhoneType = 3 then 'Office'
               end                                                                  as SourceTertiaryPhoneType,
               trim(c.SourceEmail)                                                  as SourceEmail,
               lower(c.CurrentOfficeId)                                             as CurrentOfficeId,
               dbo.FixUserId(c.CurrentOwnerId)                                      as CurrentOwnerId,
               c.DateCurrentOwnerAssigned at time zone 'Eastern Standard Time'      as DateCurrentOwnerAssigned,
               c.DateCurrentOwnerAccepted at time zone 'Eastern Standard Time'      as DateCurrentOwnerAccepted,
               dbo.FixUserId(c.ReviewById)                                          as ReviewById,
               trim(CHAR(13) + CHAR(10) + CHAR(9) + ' ' from c.ReviewComments)      as ReviewComments,
               c.ComplaintClosed                                                    as ComplaintClosed,
               c.DateComplaintClosed at time zone 'Eastern Standard Time'           as DateComplaintClosed,
               trim(CHAR(13) + CHAR(10) + CHAR(9) + ' ' from c.DeleteComments)      as DeleteComments,
               c.CreatedDate at time zone 'Eastern Standard Time'                   as CreatedDate,
               dbo.FixUserId(c.CreatedById)                                         as CreatedById,
               c.UpdatedDate at time zone 'Eastern Standard Time'                   as UpdatedDate,
               dbo.FixUserId(c.UpdatedById)                                         as UpdatedById,
               c.Deleted                                                            as Deleted,
               c.DateDeleted at time zone 'Eastern Standard Time'                   as DateDeleted,
               dbo.FixUserId(c.DeletedById)                                         as DeletedById
        from dbo._archive_Complaints c
            left join dbo._archive_LookupStates s1
            on c.CallerStateId = s1.Id
            left join dbo._archive_LookupStates s2
            on c.SourceStateId = s2.Id
            left join dbo._archive_LookupCounties u
            on c.ComplaintCountyId = u.Id;
        SET IDENTITY_INSERT dbo.Complaints OFF;
        """;
}
