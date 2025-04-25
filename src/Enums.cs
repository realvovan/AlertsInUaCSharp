namespace AlertsInUaCSharp.Enums;

public enum LocationTypes {
	Oblast,
	Raion,
	City,
	Hromada,
	Unknown
}

public enum AlertTypes {
	AirRaid,
	ArtilleryShelling,
	UrbanFighting,
	Chemical,
	Nuclear
}

public enum AlertActiveStatus {
    Active,
    Partial,
    None
}