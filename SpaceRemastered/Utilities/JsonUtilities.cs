using Newtonsoft.Json;

namespace SpaceRemastered.Utilities
{
	public static class JsonUtilities
	{
		public static T DeserializeJson<T>(string s) => JsonConvert.DeserializeObject<T>(s);

	}
}
