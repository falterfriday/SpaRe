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
	public class PhotoController : Controller
	{
		private readonly ISpaceRepository _repository;
		private readonly IConfiguration _config;
		private string results;

		public PhotoController(ISpaceRepository repository, IConfiguration config)
		{
			_repository = repository;
			_config = config;
		}

		[HttpGet]
		public async Task<IActionResult> GetPhotos()
		{
			await GetNasaPhotos();
			await GetRedditPhotos();
			await GetUnsplashPhotos();

			return Ok();
		}

		/// <summary>
		/// retrieves photo info from Nasa's APOD API
		/// </summary>
		/// <returns></returns>
		public async Task<IActionResult> GetNasaPhotos()
		{
			List<NasaModel> photoList = new List<NasaModel>();
			using (HttpClient client = new HttpClient())
			{
				try
				{
					client.BaseAddress = new Uri("https://api.nasa.gov/planetary/");
					DateTime d = DateTime.Now;
					string date;

					for (int i = 0; i < 100; i++)
					{
						d = d.AddDays(-i);
						date = d.ToString("yyyy-MM-dd");
						var response = await client.GetAsync($"apod?hd=True&date={date}&api_key={_config["NasaApiKey"]}");
						//response.EnsureSuccessStatusCode();
						results = await response.Content.ReadAsStringAsync();
						photoList.Add(JsonUtilities.DeserializeJson<NasaModel>(results));
					}
				}
				catch (Exception ex)
				{
					return BadRequest($"Error getting Nasa photos: {ex}");
				}
				client.Dispose();
			}

			for (int i = 0; i < photoList.Count; i++)
			{
				var photo = photoList[i];

				if (string.IsNullOrEmpty(photo.url)) continue;

				PhotoEntity model = new PhotoEntity()
				{
					Title = photo.title,
					Description = photo.explanation,
					HdUrl = photo.hdurl,
					Url = photo.url,
					Height = 0,
					Width = 0,
					Favorites = 0,
					Photographer = "",
					DatePosted = photo.date,
					Views = 0,
					Thumbnail = "",
					RetrievedFrom = "NasaApod",
					DownloadUrl = photo.hdurl,
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

		/// <summary>
		/// retrieves photo info from Reddit's r/spaceporn API
		/// </summary>
		/// <returns></returns>
		public async Task<IActionResult> GetRedditPhotos()
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

				if (string.IsNullOrEmpty(photo.url)) continue;

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

		/// <summary>
		/// retrieves photo info from Unsplash API
		/// </summary>
		/// <returns></returns>
		public async Task<IActionResult> GetUnsplashPhotos()
		{
			using (HttpClient client = new HttpClient())
			{

				client.BaseAddress = new Uri("https://api.unsplash.com/");
				try
				{
					var response = await client.GetAsync($"collections/365/photos?client_id={_config["UnsplashKey"]}&per_page=50");
					response.EnsureSuccessStatusCode();
					results = await response.Content.ReadAsStringAsync();
				}
				catch (Exception ex)
				{
					return BadRequest($"Error getting unsplash photos: {ex}");
				}
				client.Dispose();
			}

			List<UnsplashModel> photoList = JsonUtilities.DeserializeJson<List<UnsplashModel>>(results);

			for (int i = 0; i < photoList.Count; i++)
			{
				var photo = photoList[i];

				if (string.IsNullOrEmpty(photo.urls.raw)) continue;

				PhotoEntity model = new PhotoEntity()
				{
					Title = photo.description,
					Description = photo.description,
					Height = photo.height,
					Width = photo.width,
					Url = photo.urls.raw,
					HdUrl = photo.urls.raw,
					Favorites = 0,
					Photographer = photo.user.name,
					DatePosted = photo.created_at,
					Views = 0,
					Thumbnail = photo.urls.thumb,
					RetrievedFrom = "Unsplash",
					DownloadUrl = photo.links.download,
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
