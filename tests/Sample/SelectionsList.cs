namespace Interprocess.Transmogrify.Json.Tests.DAL;

public class SelectionsList
{
	public int Id { get; set; }
	public int OriginalId
	{
		get => Id;
		set => Id = value;
	}
	public string? Name { get; set; }

	[JsonProperty("lay_price")]
	public int LayPrice { get; set; }

	[JsonProperty("back_price")]
	public int BackPrice { get; set; }

	public double Probability { get; set; }
	public int Active { get; set; }

	[JsonProperty("recommended_price")]
	public double RecommendedPrice { get; set; }

	public int Do { get; set; }
	public string? Result { get; set; }

	public string Key => Id.ToString();
}