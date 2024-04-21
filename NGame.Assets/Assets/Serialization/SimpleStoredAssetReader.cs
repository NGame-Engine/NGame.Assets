using System.Text;
using Microsoft.Extensions.Hosting;

namespace NGame.Assets.Serialization;



public class SimpleStoredAssetReader(
	IHostEnvironment hostEnvironment
) : IStoredAssetReader
{
	public string GetAssetJson(Guid assetId)
	{
		var path = Path.Combine(
			AssetConventions.AssetPackSubFolder,
			$"{assetId}",
			"asset.json"
		);

		using var stream =
			hostEnvironment
				.ContentRootFileProvider
				.GetFileInfo(path)
				.CreateReadStream();

		using var reader = new StreamReader(stream, Encoding.UTF8);
		return reader.ReadToEnd();
	}


	public byte[]? GetAssetData(Guid assetId)
	{
		var path = Path.Combine(
			AssetConventions.AssetPackSubFolder,
			$"{assetId}",
			"data.bin"
		);

		var fileInfo = hostEnvironment
			.ContentRootFileProvider
			.GetFileInfo(path);

		if (fileInfo.Exists == false) return null;

		using var stream = fileInfo.CreateReadStream();
		using var memoryStream = new MemoryStream();
		stream.CopyTo(memoryStream);
		return memoryStream.ToArray();
	}
}
