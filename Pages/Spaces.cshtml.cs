using Microsoft.AspNetCore.Mvc.RazorPages;

namespace _425ShowWebsite.Pages;

public class SpacesModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly TwitterSpaceService _twitterSpaceService;
    public bool HasUpcoming { get; set; }
    public IEnumerable<TwitterSpace> TwitterSpaces { get; set; }

    public SpacesModel(ILogger<IndexModel> logger, TwitterSpaceService twitterSpaceService)
    {
        _logger = logger;
        _twitterSpaceService = twitterSpaceService;
    }

    public async Task OnGet()
    {
        TwitterSpaces = await _twitterSpaceService.GetData();

        SetHeaders(TwitterSpaces);
    }

    private void SetHeaders(IEnumerable<TwitterSpace> twitterSpaces)
    {
        bool upcomingFound = false, latestFound = false , previousFound = false;

        foreach(var twitterSpace in twitterSpaces)
        {
            if (!upcomingFound && twitterSpace.IsUpcoming) //First upcoming item, flag it
            {
                upcomingFound = true;
                twitterSpace.IsUpcomingHeader = true;
            }
            if (!latestFound && !twitterSpace.IsUpcoming) //first recorded space, flag as latest
            {
                latestFound = true;
                twitterSpace.IsLatest = true;
            }
            if (!previousFound && !twitterSpace.IsLatest && !twitterSpace.IsUpcoming)
            {
                previousFound = true;
                twitterSpace.IsPrevious = true;
            }
            if(upcomingFound && latestFound && previousFound) //stop searching for header items.
            {
                break;
            }
        }
        HasUpcoming = upcomingFound;
    }
}