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
	public class NasaController : Controller
	{
		private readonly ISpaceRepository _repository;
		private readonly IConfiguration _config;
		private string results;

		public NasaController(ISpaceRepository repository, IConfiguration config)
		{
			_repository = repository;
			_config = config;
		}

		[HttpGet]
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
						response.EnsureSuccessStatusCode();
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
	}
}
