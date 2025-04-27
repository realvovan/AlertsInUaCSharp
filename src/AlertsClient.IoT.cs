namespace AlertsInUaCSharp;

public partial class AlertsClient {
    /// <summary>
    /// Returns alert statuses in every oblast as a string in order <see cref="OblastOrder"/>, where A means active alert,
    /// P - partly active in some raions or hromadas, N - no active alert.
    /// <br/>
    /// Example output: "AAANNNNNNNNNANNNNNNNNPPPNNN"
    /// </summary>
    /// <exception cref="HttpRequestException"></exception>
    public async Task<string> GetRawAlertStatusesAsync() {
        string result = await this.request(ApiEndpoints.GET_STATUSES);
        return result.Substring(1,result.Length - 2);
    }
    /// <summary>
    /// Returns alert status in an oblast with a given uid as a char,
    /// where A means active alert, P - partly active in some raions or hromadas, N - no active alert.
    /// <br/>
    /// Example output: 'N'
    /// </summary>
    /// <param name="oblastUid">Oblast unique identifier</param>
    /// <exception cref="HttpRequestException"></exception>
    public async Task<char> GetRawAlertStatusAsync(int oblastUid)
        => ( await this.request(string.Format(ApiEndpoints.GET_STATUS_IN_OBLAST,oblastUid)) )[1];
    /// <summary>
    /// Returns alert status in an oblast with a given name as a char,
    /// where A means active alert, P - partly active in some raions or hromadas, N - no active alert.
    /// <br/>
    /// Example output: 'N'
    /// </summary>
    /// <param name="oblast">Oblast name</param>
    /// <exception cref="HttpRequestException"></exception>
    public async Task<char> GetRawAlertStatusAsync(string oblast)
        => await this.GetRawAlertStatusAsync(UIDResolver.GetUIDFromOblast(oblast));
}