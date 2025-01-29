using SWGW.AppServices.Perimits.QueryDto;
using SWGW.Domain.Entities.Permits;
using SWGW.TestData;
using SWGW.AppServices.Perimits;

namespace AppServicesTests.Search;

public class PermitFilterTests
{
    [Test]
    public void DefaultSpec_ReturnsAllNonDeleted()
    {
        // Arrange
        var spec = new PermitSearchDto();
        var expression = PermitFilters.SearchPredicate(spec);
        var expected = PermitData.GetPermits.Where(entry => !entry.IsDeleted);

        // Act
        var result = PermitData.GetPermits.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void ClosedStatusSpec_ReturnsFilteredList()
    {
        // Arrange
        var spec = new PermitSearchDto { Status = PermitStatus.Closed };
        var expression = PermitFilters.SearchPredicate(spec);
        var expected = PermitData.GetPermits.Where(entry => entry is { IsDeleted: false, Closed: true });

        // Act
        var result = PermitData.GetPermits.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void DeletedSpec_ReturnsFilteredList()
    {
        // Arrange
        var spec = new PermitSearchDto { DeletedStatus = SearchDeleteStatus.Deleted };
        var expression = PermitFilters.SearchPredicate(spec);
        var expected = PermitData.GetPermits.Where(entry => entry.IsDeleted);

        // Act
        var result = PermitData.GetPermits.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void NeutralDeletedSpec_ReturnsAll()
    {
        // Arrange
        var spec = new PermitSearchDto { DeletedStatus = SearchDeleteStatus.All };
        var expression = PermitFilters.SearchPredicate(spec);

        // Act
        var result = PermitData.GetPermits.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(PermitData.GetPermits);
    }

    [Test]
    public void ReceivedDateSpec_ReturnsFilteredList()
    {
        // Arrange
        var referenceItem = PermitData.GetPermits.First();

        var spec = new PermitSearchDto
        {
            ReceivedFrom = DateOnly.FromDateTime(referenceItem.ReceivedDate.Date),
            ReceivedTo = DateOnly.FromDateTime(referenceItem.ReceivedDate.Date),
        };

        var expression = PermitFilters.SearchPredicate(spec);

        var expected = PermitData.GetPermits
            .Where(entry =>
                entry is { IsDeleted: false } && entry.ReceivedDate.Date == referenceItem.ReceivedDate.Date);

        // Act
        var result = PermitData.GetPermits.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void ReceivedBySpec_ReturnsFilteredList()
    {
        // Arrange
        var referenceItem = PermitData.GetPermits.First(entry => entry.ReceivedBy != null);
        var spec = new PermitSearchDto { ReceivedBy = referenceItem.ReceivedBy!.Id };
        var expression = PermitFilters.SearchPredicate(spec);

        var expected = PermitData.GetPermits
            .Where(entry => entry is { IsDeleted: false } && entry.ReceivedBy == referenceItem.ReceivedBy);

        // Act
        var result = PermitData.GetPermits.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void ActionTypeSpec_ReturnsFilteredList()
    {
        // Arrange
        var referenceItem = PermitData.GetPermits.First(entry => entry.ActionType != null);
        var spec = new PermitSearchDto {    ActionType = referenceItem.ActionType!.Id };
        var expression = PermitFilters.SearchPredicate(spec);

        var expected = PermitData.GetPermits
            .Where(entry => entry is { IsDeleted: false, ActionType: not null } &&
                            entry.ActionType.Id == referenceItem.ActionType.Id);

        // Act
        var result = PermitData.GetPermits.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }
}
