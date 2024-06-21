namespace Interprocess.Transmogrify.Json.Tests.DAL;

public class Scoreboard
{
	public string? Away { get; set; }

	[JsonProperty("bet_delay")]
	public string? BetDelay { get; set; }

	public int Clock { get; set; }
	public string? Home { get; set; }
	public string? Latency { get; set; }
}