using SpaceRemastered.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpaceRemastered.Data
{
	public interface ISpaceRepository
	{
		IEnumerable<PhotoEntity> GetAllPhotos();

		Task AddPhoto(PhotoEntity photo);

		void SaveAll();

		//void AddEntity(Photo model);

		//bool IsUrlPresent(string url);

		//IEnumerable<string> GetAllUrls();
	}
}