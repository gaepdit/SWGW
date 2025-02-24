﻿using SWGW.Domain.Entities.ActionTypes;
using SWGW.Domain.Entities.PermitActions;
using SWGW.Domain.Entities.Offices;
using SWGW.Domain.Identity;

namespace SWGW.Domain.Entities.Permits;

public class PermitManager(IPermitRepository repository) : IPermitManager
{
    public Permit Create(ApplicationUser? user)
    {
        var item = new Permit(repository.GetNextId()) { EnteredBy = user };
        item.SetCreator(user?.Id);
        return item;
    }

    public PermitAction CreateAction(Permit permit, ActionType actionType, ApplicationUser? user)
    {
        var action = new PermitAction(Guid.NewGuid(), permit);
        action.SetCreator(user?.Id);
        return action;
    }

    public PermitAction CreateAction(Permit permit,  ApplicationUser? user)
    {
        var action = new PermitAction(Guid.NewGuid(), permit);
        action.SetCreator(user?.Id);
        return action;
    }

    public void Close(Permit permit, string? comment, ApplicationUser? user)
    {
        permit.SetUpdater(user?.Id);
        permit.Status = PermitStatus.Void;
        permit.Closed = true;
        permit.PermitClosedDate = DateTime.Now;
        permit.ClosedBy = user;
        permit.ClosedComments = comment;
    }

    public void Reopen(Permit permit, ApplicationUser? user)
    {
        permit.SetUpdater(user?.Id);
        permit.Status = PermitStatus.Active;
        permit.Closed = false;
        permit.PermitClosedDate = null;
        permit.ClosedBy = null;
        permit.ClosedComments = null;
    }

    public void Delete(Permit permit, string? comment, ApplicationUser? user)
    {
        permit.SetDeleted(user?.Id);
        permit.DeletedBy = user;
        permit.DeleteComments = comment;
    }

    public void Restore(Permit permit, ApplicationUser? user)
    {
        permit.SetNotDeleted();
        permit.DeleteComments = null;
    }
}
