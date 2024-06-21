namespace Interprocess.Transmogrify.Json.Tests.DAL;

public class Score 
{
	public string? Ball { get; set; }
	public int Modified { get; set; }
	public int Priority { get; set; }
	public string? Result { get; set; }
	public int Runs { get; set; }
	public string? Timestamp { get; set; }

	public string Key => Priority.ToString();
}