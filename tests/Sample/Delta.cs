namespace Interprocess.Transmogrify.Json.Tests.DAL;

public class Delta
{
	public MarketMatch? Match { get; set; }
	public Scoreboard? Scoreboard { get; set; }
	public List<Score>? Score { get; set; }
	public List<Market>? Markets { get; set; }
}