namespace AlertsInUaCSharp;

internal static class ApiEndpoints {
    public const string BASE_URL = "https://api.alerts.in.ua/v1/";
    public const string GET_ACTIVE_ALERTS = "alerts/active.json?token=";
    public const string GET_STATUSES = "iot/active_air_raid_alerts_by_oblast.json?token=";
    public const string GET_STATUS_IN_OBLAST = "iot/active_air_raid_alerts/{0}.json?token=";
    public const string GET_HISTORY = "regions/{0}/alerts/month_ago.json?token=";
}