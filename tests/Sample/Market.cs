namespace Interprocess.Transmogrify.Json.Tests.DAL;

public class Market
{
	public int Active { get; set; }
	public int Display { get; set; }

	[JsonProperty("is_spread")]
	public int IsSpread { get; set; }

	public int Lay { get; set; }
	public double Line { get; set; }

	[JsonProperty("market_do")]
	public int MarketDo { get; set; }

	[JsonProperty("market_group")]
	public string? MarketGroup { get; set; }

	public string? Result { get; set; }

	[JsonProperty("market_id")]
	public int MarketId { get; set; }

	[JsonProperty("market_line_id")]
	public long MarketLineId { get; set; }

	[JsonProperty("market_name")]
	public string? MarketName { get; set; }

	[JsonProperty("market_type")]
	public string? MarketType { get; set; }

	[JsonProperty("market_type_id")]
	public int? MarketTypeId { get; set; }

	public double Midpoint { get; set; }
	public int Over { get; set; }
	public string? Player { get; set; }

	[JsonProperty("player_id")]
	public int PlayerId { get; set; }

	[JsonProperty("pre_match")]
	public string? PreMatch { get; set; }

	public int Priority { get; set; }
	public int Resettled { get; set; }
	public int Selections { get; set; }
	public int Session { get; set; }

	[JsonProperty("so_far")]
	public int SoFar { get; set; }

	[JsonProperty("so_far_res")]
	public int SoFarRes { get; set; }

	public int Spread { get; set; }

	[JsonProperty("stake_factor")]
	public double StakeFactor { get; set; }

	public string? Status { get; set; }
	public string? Team { get; set; }

	[JsonProperty("team_id")]
	public int TeamId { get; set; }

	public int Wkt { get; set; }
	public string? Class { get; set; }

	[JsonProperty("in_play")]
	public string? InPlay { get; set; }

	public int Inns { get; set; }
	public int Delivery { get; set; }
	public double Margin { get; set; }
	public List<SelectionsList>? Selectionslist { get; set; }

	public string Key => $"DecimalPoint|{MarketId}";
}