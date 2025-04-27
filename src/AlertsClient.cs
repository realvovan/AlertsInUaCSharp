using System.Text.Json;
using AlertsInUaCSharp.Enums;

namespace AlertsInUaCSharp;

/// <summary>
/// Class for working with the alerts.in.ua API
/// </summary>
public partial class AlertsClient {
	private readonly HttpClient httpClient = new();
	public static string[] OblastOrder { get; } = [
		"Автономна Республіка Крим",
		"Волинська область",
		"Вінницька область",
		"Дніпропетровська область",
		"Донецька область",
		"Житомирська область",
		"Закарпатська область",
		"Запорізька область",
		"Івано-Франківська область",
		"м. Київ",
		"Київська область",
		"Кіровоградська область",
		"Луганська область",
		"Львівська область",
		"Миколаївська область",
		"Одеська область",
		"Полтавська область",
		"Рівненська область",
		"м. Севастополь",
		"Сумська область",
		"Тернопільська область",
		"Харківська область",
		"Херсонська область",
		"Хмельницька область",
		"Черкаська область",
		"Чернівецька область",
		"Чернігівська область"
	];
	public string Token { get; }
	/// <summary>
	/// Returns a list of all active alerts in oblast, raions, cities and hromadas
	/// </summary>
	/// <exception cref="HttpRequestException"></exception>
	/// <exception cref="JsonException"></exception>
	public async Task<Alert[]> GetActiveAlertsAsync() {
		return this.createAlertsFromDictionary(
			JsonSerializer.Deserialize<Dictionary<string,JsonElement>>(await this.request(ApiEndpoints.GET_ACTIVE_ALERTS))
		);
	}
	/// <summary>
	/// Returns a list of alert statuses in every oblast in order (see <see cref="OblastOrder"/>)
	/// </summary>
	/// <exception cref="HttpRequestException"></exception>
	public async Task<AlertStatuses> GetAlertStatusesAsync() {
		var result = await this.request(ApiEndpoints.GET_STATUSES);
		var statuses = new AlertStatus[result.Length - 2]; // -2 for the two " symbols in a string
		for (int i = 1; i < result.Length - 1; i++) {
			statuses[i-1] = new AlertStatus(OblastOrder[i-1],result[i]);
		}
		return new AlertStatuses(statuses);
	}
	/// <summary>
	/// Gets the alert status for an oblast with a given UID
	/// </summary>
	/// <param name="uid">UID of the oblast</param>
	/// <exception cref="HttpRequestException"></exception>
	public async Task<AlertStatus> GetAlertStatusInOblastAsync(int uid) {
		var result = await this.request(string.Format(ApiEndpoints.GET_STATUS_IN_OBLAST,uid));
		return new AlertStatus(UIDResolver.GetOblastFromUID(uid) ?? $"Location with UID {uid}",result[1]);
	}
	/// <summary>
	/// Gets the alert status for an oblast with a given name
	/// </summary>
	/// <param name="oblast">Name of the oblast</param>
	/// <exception cref="HttpRequestException"></exception>
	public async Task<AlertStatus> GetAlertStatusInOblastAsync(string oblast) {
		var result = await this.request(string.Format(ApiEndpoints.GET_STATUS_IN_OBLAST,UIDResolver.GetUIDFromOblast(oblast)));
		return new AlertStatus(oblast,result[1]);
	}
	/// <summary>
	/// Returns a list of alerts in a given oblast in the last month
	/// </summary>
	/// <param name="uid">UID of the oblast</param>
	/// <exception cref="HttpRequestException"></exception>
	/// <exception cref="JsonException"></exception>
	public async Task<Alert[]> GetAlertHistoryAsync(int uid) {
		return this.createAlertsFromDictionary(
			JsonSerializer.Deserialize<Dictionary<string,JsonElement>>(await this.request(string.Format(ApiEndpoints.GET_HISTORY,uid)))
		);
	}
	/// <summary>
	/// Returns a list of alerts in a given oblast in the last month
	/// </summary>
	/// <param name="oblast">Name of the oblast</param>
	/// <exception cref="HttpRequestException"></exception>
	/// <exception cref="JsonException"></exception>
	public async Task<Alert[]> GetAlertHistoryAsync(string oblast) => await this.GetAlertHistoryAsync(UIDResolver.GetUIDFromOblast(oblast));

	private async Task<string> request(string endPoint) {
		var response = await this.httpClient.GetAsync(ApiEndpoints.BASE_URL+endPoint+this.Token);
		return (int)response.StatusCode switch {
			200 => await response.Content.ReadAsStringAsync(),
			401 => throw new HttpRequestException("Invalid API token"),
			403 => throw new HttpRequestException("Your IP address is blocked or the API is not available in your region"),
			429 => throw new HttpRequestException("Too many requests"),
			_ => throw new HttpRequestException($"An error occured trying to make a request, error code: {(int)response.StatusCode}, reason: {response.ReasonPhrase}"),
		};
	}

	private Alert[] createAlertsFromDictionary(Dictionary<string,JsonElement>? dict) {
        ArgumentNullException.ThrowIfNull(dict,nameof(dict));
		if (!dict.TryGetValue("alerts",out JsonElement list)) throw new ArgumentException(
			"Invalid argument. Dictionary must contain key 'alerts'",
			nameof(dict)
		);
		var alerts = new Alert[list.GetArrayLength()];
		for (int i = 0; i < alerts.Length; i++) {
			var alertInfo = list[i].Deserialize<Dictionary<string,JsonElement>>()
				?? throw new JsonException("Error occured trying to deserialize the response");
			alerts[i] = new Alert() {
				Id = alertInfo["id"].GetInt32(),
				LocationTitle = alertInfo["location_title"].GetString()!,
				LocationType = alertInfo["location_type"].GetString() switch {
					"oblast" => LocationTypes.Oblast,
					"raion" => LocationTypes.Raion,
					"city" => LocationTypes.City,
					"hromada" => LocationTypes.Hromada,
					_ => LocationTypes.Unknown
				},
				StartedAt = alertInfo["started_at"].GetDateTime(),
				FinishedAt = alertInfo["finished_at"].Deserialize<DateTime?>(),
				UpdatedAt = alertInfo["updated_at"].GetDateTime(),
				AlertType = alertInfo["alert_type"].GetString() switch {
					"air_raid" => AlertTypes.AirRaid,
					"artillery_shelling" => AlertTypes.ArtilleryShelling,
					"urban_fighting" => AlertTypes.UrbanFighting,
					"chemical" => AlertTypes.Chemical,
					"nuclear" => AlertTypes.Nuclear,
					_ => AlertTypes.AirRaid
				},
				LocationUID = alertInfo["location_uid"].GetString()!,
				LocationOblast = alertInfo["location_oblast"].GetString()!,
				LocationOblastUID = alertInfo["location_oblast_uid"].GetInt32(),
				Notes = alertInfo["notes"].GetString() ?? "",
				IsCalculated = alertInfo["calculated"].Deserialize<bool?>() ?? false
			};
		}
		return alerts;
    }

	public AlertsClient(string token) => this.Token = token;
}
