namespace AlertsInUaCSharp;

public static class UIDResolver {
    private static readonly Dictionary<string,int> UIDs = new() {
        {"Хмельницька область",3},
        {"Вінницька область",4},
        {"Рівненська область",5},
        {"Волинська область",8},
        {"Дніпропетровська область",9},
        {"Житомирська область",10},
        {"Закарпатська область",11},
        {"Запорізька область",12},
        {"Івано-Франківська область",13},
        {"Київська область",14},
        {"Кіровоградська область",15},
        {"Луганська область",16},
        {"Миколаївська область",17},
        {"Одеська область",18},
        {"Полтавська область",19},
        {"Сумська область",20},
        {"Тернопільська область",21},
        {"Харківська область",22},
        {"Херсонська область",23},
        {"Черкаська область",24},
        {"Чернігівська область",25},
        {"Чернівецька область",26},
        {"Львівська область",27},
        {"Донецька область",28},
        {"Автономна Республіка Крим",29},
        {"м. Севастополь",30},
        {"Севастополь",30},
        {"м. Київ",31},
        {"Київ",31},
    };
    /// <summary>
    /// Gets the unique identifier for an oblast. Throws an ArgumentException if no oblast with a given name is found
    /// </summary>
    /// <param name="name">Name of the oblast</param>
    /// <exception cref="ArgumentException"></exception>
    public static int GetUIDFromOblast(string name) {
        bool result = UIDs.TryGetValue(name,out int uid);
        return result ? uid : throw new ArgumentException("Invalid argument, no oblast matches given name",nameof(name));
    }
    /// <summary>
    /// Gets the name of the oblast with a given unique identifier, or null if no name was found
    /// </summary>
    /// <param name="uid">Oblast identifier</param>
    public static string? GetOblastFromUID(int uid) {
        string? result = null;
        foreach (KeyValuePair<string,int> i in UIDs) {
            if (i.Value == uid) {
                result = i.Key;
                break;
            }
        }
        return result;
    }
    /// <summary>
    /// Checks if there's a UID for a given oblast
    /// </summary>
    /// <param name="name">Oblast name to check</param>
    public static bool UIDExists(string name) => UIDs.ContainsKey(name);
}