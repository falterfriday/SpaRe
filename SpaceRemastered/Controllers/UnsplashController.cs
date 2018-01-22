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
	public class UnsplashController : Controller
	{
		private readonly ISpaceRepository _repository;
		private readonly IConfiguration _config;
		private string results;

		public UnsplashController(ISpaceRepository repository, IConfiguration config)
		{
			_repository = repository;
			_config = config;
		}

		public async Task<IActionResult> Get()
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
	}
}
