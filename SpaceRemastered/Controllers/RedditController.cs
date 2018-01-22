using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SpaceRemastered.Data;
using SpaceRemastered.Data.Entities;
using SpaceRemastered.Models;
using SpaceRemastered.Utilities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SpaceRemastered.Controllers
{

	[Route("api/[Controller]")]
	public class RedditController : Controller
	{
		private readonly ISpaceRepository _repository;
		private readonly IConfiguration _config;
		private string results;

		public RedditController(ISpaceRepository repository, IConfiguration config)
		{
			_repository = repository;
			_config = config;
		}

		[HttpGet]
		public async Task<IActionResult> Get()
		{
			using (HttpClient client = new HttpClient())
			{
				try
				{
					var response = await client.GetAsync("https://www.reddit.com/r/spaceporn.json?");
					response.EnsureSuccessStatusCode();
					results = await response.Content.ReadAsStringAsync();
				}
				catch (HttpRequestException ex)
				{
					return BadRequest($"Error getting reddit photos: {ex}");
				}
				client.Dispose();
			}

			List<Child> photoList = (JsonUtilities.DeserializeJson<RedditModel>(results)).data.children;

			for (int i = 1; i < photoList.Count; i++)
			{
				var photo = photoList[i].data;

				PhotoEntity model = new PhotoEntity
				{
					Description = photo.title,
					Title = photo.title,
					Height = photo.preview.images[0].source.height,
					Width = photo.preview.images[0].source.width,
					Thumbnail = photo.thumbnail,
					Url = photo.url,
					Photographer = photo.author,
					Views = 0,
					Favorites = 0,
					DatePosted = FromSecondsUnixEpoch((long)photo.created_utc),
					RetrievedFrom = "Reddit",
					DownloadUrl = photo.url,
					DateCreated = DateTime.Now,
					DateUpdated = DateTime.Now
				};

				if (ModelState.IsValid)
				{
					await _repository.AddPhoto(model);
				}
			}
			return Ok();
		}


		private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		private static DateTime FromSecondsUnixEpoch(long seconds) => UnixEpoch.AddSeconds(seconds);
	}
}
