using System.Text.Json;
using System.Text.Json.Serialization;
using NGame.Assets.Serialization;
using Singulink.IO;

namespace NGame.Assets.UsageFinder.AssetOverviews;



internal class JsonAsset : Asset;



internal interface IAssetOverviewCreator
{
	AssetOverview Create(IEnumerable<IAbsoluteFilePath> assetListPaths);
}



internal class AssetOverviewCreator(
	IEnumerable<JsonConverter> jsonConverters
) : IAssetOverviewCreator
{
	private record AssetFileInfo(
		IAbsoluteDirectoryPath ProjectDirectory,
		PathInfo MainPathInfo,
		PathInfo? SatellitePathInfo
	);



	public AssetOverview Create(IEnumerable<IAbsoluteFilePath> assetListPaths)
	{
		var options = new JsonSerializerOptions();

		foreach (var jsonConverter in jsonConverters)
		{
			options.Converters.Add(jsonConverter);
		}


		var assetEntries = assetListPaths
			.SelectMany(ReadAssetListFile)
			.Select(x => CreateAssetEntry(x, options));


		return new AssetOverview(assetEntries);
	}


	private static IEnumerable<AssetFileInfo> ReadAssetListFile(
		IAbsoluteFilePath assetListFile
	)
	{
		var fileLines = File.ReadAllLines(assetListFile.PathDisplay);

		var projectDirectory = DirectoryPath.ParseAbsolute(fileLines[0]);

		return fileLines
			.Skip(1)
			.Select(x => CreateAssetFileInfo(x, projectDirectory));
	}


	private static AssetFileInfo CreateAssetFileInfo(
		string assetListLine,
		IAbsoluteDirectoryPath projectDirectory
	)
	{
		var lineParts = assetListLine.Split(
			AssetConventions.RelativePathSeparator,
			2,
			StringSplitOptions.RemoveEmptyEntries
		);

		var mainPathInfo = CreatePathInfo(lineParts[0], projectDirectory);

		var satellitePathInfo =
			lineParts.Length == 2
				? CreatePathInfo(lineParts[1], projectDirectory)
				: null;

		return new AssetFileInfo(projectDirectory, mainPathInfo, satellitePathInfo);
	}


	private static PathInfo CreatePathInfo(
		ReadOnlySpan<char> fileLine,
		IAbsoluteDirectoryPath projectDirectory
	)
	{
		var sourcePath = projectDirectory.CombineFile(fileLine);
		var targetPath = FilePath.ParseRelative(fileLine);
		return new PathInfo(projectDirectory, sourcePath, targetPath);
	}


	private static AssetEntry CreateAssetEntry(
		AssetFileInfo assetFileInfo,
		JsonSerializerOptions options
	)
	{
		var mainPathInfo = assetFileInfo.MainPathInfo;
		var allText = File.ReadAllText(mainPathInfo.SourcePath.PathDisplay);
		var jsonAsset = JsonSerializer.Deserialize<JsonAsset>(allText, options)!;

		return new AssetEntry(
			jsonAsset.Id,
			assetFileInfo.ProjectDirectory,
			mainPathInfo,
			jsonAsset.PackageName,
			assetFileInfo.SatellitePathInfo
		);
	}
}
