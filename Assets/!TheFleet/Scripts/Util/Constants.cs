[System.Serializable]
public enum EPageName
{
	NONE = -1,
	Splash,
	Main,
	Credits,
	COUNT
}
public enum EAlien
{
	NONE = -1,
	Green,
	Blue,
	Red,
	COUNT
}

public enum EPowerUp
{
	NONE = -1,
	Damage,
	Piercing,
	Spread,
	Seeker,
	Nuke,
	COUNT
}
public static class EnumExtension
{
	public static int Int(this System.Enum i)
	{
		return (int)(object)i;
	}
}