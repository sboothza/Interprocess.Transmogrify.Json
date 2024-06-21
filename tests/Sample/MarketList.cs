
namespace Interprocess.Transmogrify.Json.Tests.DAL;

public class MarketList
{
    public string _id { get; set; }

    [JsonProperty("feed_id")]
	public string? FeedId { get; set; }

	[JsonProperty("server_time")]
	public DateTime ServerTime { get; set; }

	[JsonProperty("last_update")]
	public string? LastUpdate { get; set; }

	[JsonProperty("delta")]
	public Delta? Delta { get; set; }

	[JsonProperty("msg_type")]
	public string? MsgType { get; set; }

	public string? State { get; set; }

	public string MessageType => "Delta";

	public string? FixtureId => FeedId;


	public DateTime? Timestamp = DateTime.UtcNow;

	public string Key => $"DecimalPoint|{FeedId}|MarketList|{ServerTime}";
}