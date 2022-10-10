using System.Text.Json;
using System.Text.Json.Serialization;
public class TwitterSpaceService
{
    private const string TwitterSpaceUrl = "https://raw.githubusercontent.com/425show/newWebsite/main/spaces.json";
    private HttpClient? _httpClient;
    private IWebHostEnvironment _hostEnvironment;

    public TwitterSpaceService(HttpClient httpClient, IWebHostEnvironment hostEnvironment)
    {
        _httpClient = httpClient;
        _hostEnvironment = hostEnvironment;
    }

    public async Task<IEnumerable<TwitterSpace>> GetData()
    {   
        var rawJson = await File.ReadAllTextAsync(Path.Combine(_hostEnvironment.ContentRootPath, "spaces.json"));
        var twitterSpaces = JsonSerializer.Deserialize<IEnumerable<TwitterSpace>>(rawJson);
        return twitterSpaces;
    }
}
public class Link
{
    [JsonPropertyName("linktext")]
    public string? LinkText { get; set; }
    [JsonPropertyName("linkurl")]
    public string? LinkUrl { get; set; }
}

public class Participant
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonPropertyName("link")]
    public string? Link { get; set; }
}

public class TwitterSpace
{
    [JsonPropertyName("topic")]
    public string? Topic { get; set; }
    [JsonPropertyName("recordingdate")]
    public string? RecordingDate { get; set; }
    [JsonPropertyName("expirydate")]
    public string? ExpiryDate { get; set; }
    [JsonPropertyName("link")]
    public string? Link { get; set; }
    [JsonPropertyName("summary")]
    public string? Summary { get; set; }
    [JsonPropertyName("links")]
    public List<Link>? Links { get; set; }

    [JsonPropertyName("participants")]
    public List<Participant>? Participants { get; set; }

    public DateTimeOffset StartDate{
        get {
            return DateTimeOffset.Parse(RecordingDate);
        }
    }
    public bool IsUpcoming
    {
        get
        {
            return StartDate.Date > DateTime.Today.Date;
        }
    }
    public DateTimeOffset EndDate{
        get {
            return DateTimeOffset.Parse(ExpiryDate);
        }
    }

    public bool IsLatest { get; internal set; }
    public bool IsPrevious { get; internal set; }
    public bool IsUpcomingHeader { get; internal set; }
}