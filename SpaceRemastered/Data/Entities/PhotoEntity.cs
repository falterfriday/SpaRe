using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpaceRemastered.Data.Entities
{
	public class PhotoEntity
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public int Height { get; set; }
		public int Width { get; set; }
		public string Url { get; set; }
		public string HdUrl { get; set; }
		public int Favorites { get; set; }
		public string Photographer { get; set; }
		public int Views { get; set; }
		public string Thumbnail { get; set; }
		public string RetrievedFrom { get; set; }
		public string DownloadUrl { get; set; }
		public DateTime DatePosted { get; set; }
		public DateTime DateCreated { get; set; }
		public DateTime DateUpdated { get; set; }
	}
}
