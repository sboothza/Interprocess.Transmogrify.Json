namespace Interprocess.Transmogrify.Json.Tests.DAL;

public class Delta
{
	public Match? Match { get; set; }
	public Scoreboard? Scoreboard { get; set; }
	public List<Score>? Score { get; set; }
	public List<Market>? Markets { get; set; }
}