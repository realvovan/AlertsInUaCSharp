using AlertsInUaCSharp.Enums;

namespace AlertsInUaCSharp;
/// <summary>
/// Represents info about a air alert (bombing threat) in a location (oblast/raion/hromada/city)
/// </summary>
public readonly struct Alert {
	public int Id { get; init; }
	public string LocationTitle { get; init; }
	public LocationTypes LocationType { get; init; }
	public DateTime StartedAt { get; init; }
	public DateTime? FinishedAt { get; init; }
	public DateTime UpdatedAt { get; init; }
	public AlertTypes AlertType { get; init; }
	public string LocationUID { get; init; }
	public string LocationOblast { get; init; }
	public int LocationOblastUID { get; init; }
	public string Notes { get; init; }
	/// <summary>
	/// Whether the FinishedAt time is estimated or actual time is used
	/// </summary>
	public bool IsCalculated { get; init; }

	public override string ToString() {
		string m = "--------------------\n";
		foreach (var i in typeof(Alert).GetProperties()) m += $"{i.Name}: {i.GetValue(this)}\n";
		m += "--------------------";
		return m;
	}
}