using SWGW.AppServices.DataExport;
using SWGW.AppServices.Permissions;
using SWGW.AppServices.Perimits.QueryDto;

namespace SWGW.WebApp.Pages.Staff.Perimits;

[Authorize(Policy = nameof(Policies.ActiveUser))]
public class DownloadSearchModel(ISearchResultsExportService searchResultsExportService) : PageModel
{
    public PermitSearchDto Spec { get; private set; } = null!;
    public int ResultsCount { get; private set; }
    private const string ExcelContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

    public async Task<IActionResult> OnGetAsync(PermitSearchDto? spec, CancellationToken token)
    {
        if (spec is null) return BadRequest();
        ResultsCount = await searchResultsExportService.CountAsync(spec, token);
        Spec = spec;
        return Page();
    }

    public async Task<IActionResult> OnGetDownloadAsync(PermitSearchDto? spec, CancellationToken token)
    {
        if (spec is null) return BadRequest();
        var excel = (await searchResultsExportService.ExportSearchResultsAsync(spec, token))
            .ToExcel(sheetName: "Search Results", deleteLastColumn: spec.DeletedStatus == null);
        var fileDownloadName = $"search_{DateTime.Now:yyyy-MM-dd--HH-mm-ss}.xlsx";
        return File(excel, ExcelContentType, fileDownloadName: fileDownloadName);
    }
}
