namespace Interprocess.Transmogrify.Json.Tests.DAL;

public class Match
{
	public string? Away { get; set; }
	public string? Caption { get; set; }
	public string? Category { get; set; }
	public string? Comp { get; set; }

	[JsonProperty("direct_data")]
	public string? DirectData { get; set; }

	[JsonProperty("event_status")]
	public string? EventStatus { get; set; }

	public string? Format { get; set; }

	[JsonProperty("global_betting_status")]
	public string? GlobalBettingStatus { get; set; }

	public string? Guidance { get; set; }
	public string? Home { get; set; }
	public string? Identifier { get; set; }
	public string? Scoreline { get; set; }

	[JsonProperty("sport_id")]
	public int SportId { get; set; }

	[JsonProperty("stake_factor")]
	public double StakeFactor { get; set; }

	[JsonProperty("start_date")]
	public string? StartDate { get; set; }

	[JsonProperty("tier")]
	public int? Tier { get; set; }
	public string? Title { get; set; }
	public string? Venue { get; set; }
}