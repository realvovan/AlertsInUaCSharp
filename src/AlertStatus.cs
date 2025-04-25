using AlertsInUaCSharp.Enums;

namespace AlertsInUaCSharp;

public readonly struct AlertStatus {
    public string Oblast { get; }
    public AlertActiveStatus Status { get; }

    public bool IsActive() => this.Status == AlertActiveStatus.Active;
    public bool IsPartlyActive() => this.Status == AlertActiveStatus.Partial;
    public bool IsNoAlert() => this.Status == AlertActiveStatus.None;
    public override string ToString() {
        string m = this.Status switch {
            AlertActiveStatus.Active => "🔴 Active alert in ",
            AlertActiveStatus.Partial => "🟡 Partly active alert in ",
            AlertActiveStatus.None => "🟢 No alert in ",
            _ => "❔No info for "
        };
        return m + this.Oblast;
    }

    public AlertStatus(string oblastName,AlertActiveStatus status) {
        this.Oblast = oblastName;
        this.Status = status;
    }
    public AlertStatus(string oblastName,char status) : this(
        oblastName,
        status switch {
            'A' => AlertActiveStatus.Active,
            'P' => AlertActiveStatus.Partial,
            'N' => AlertActiveStatus.None,
            _ => throw new ArgumentException("Invalid status",nameof(status))
        }
    ) {}
}

public readonly struct AlertStatuses {
    private readonly AlertStatus[] statuses;

    public int Length { get => this.statuses.Length; }
    public AlertStatus this[int key] { get => this.statuses[key]; }

    public AlertStatuses GetActiveStatuses() => this.filter(AlertActiveStatus.Active);
    public AlertStatuses GetPartlyActiveStatuses() => this.filter(AlertActiveStatus.Partial);
    public AlertStatuses GetInactiveStatuses() => this.filter(AlertActiveStatus.None);
    public IEnumerator<AlertStatus> GetEnumerator() {
        foreach (var i in this.statuses) yield return i;
    }
    public AlertStatus GetStatusFromName(string name) {
        foreach (var i in this.statuses) {
            if (i.Oblast == name) return i;
        }
        throw new ArgumentException("No status found for given name",nameof(name));
    }
    public override string ToString() => string.Join('\n',this.statuses);

    private AlertStatuses filter(AlertActiveStatus filterBy) {
        List<AlertStatus> newStatuses = new();
        foreach (var i in this.statuses) {
            if (i.Status == filterBy) newStatuses.Add(i);
        }
        return new AlertStatuses(newStatuses.ToArray());
    }

    public AlertStatuses(AlertStatus[] statuses) => this.statuses = statuses;
}