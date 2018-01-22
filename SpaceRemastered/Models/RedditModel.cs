using System.Collections.Generic;

namespace SpaceRemastered.Models
{
	public class RedditModel
	{
		public string kind { get; set; }
		public Data data { get; set; }
	}

	public class Data
	{
		public List<Child> children { get; set; }
	}

	public class Child
	{
		public Data2 data { get; set; }
	}

	public class Data2
	{
		public string domain { get; set; }
		public string author { get; set; }

		public Preview preview { get; set; }
		public string thumbnail { get; set; }
		public string name { get; set; }
		public string url { get; set; }
		public string title { get; set; }
		public float created_utc { get; set; }
	}

	public class Preview
	{
		public List<Image> images { get; set; }
	}

	public class Image
	{
		public Source source { get; set; }
		public List<Resolution> resolutions { get; set; }
	}

	public class Source
	{
		public string url { get; set; }
		public int width { get; set; }
		public int height { get; set; }
	}

	public class Resolution
	{
		public string url { get; set; }
		public int width { get; set; }
		public int height { get; set; }
	}
}
