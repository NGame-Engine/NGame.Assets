using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NGame.Assets;

namespace NGame.Setup;



public static class AssetTypeInstaller
{
	public static IHostApplicationBuilder AddAssetTypesFromAssembly(
		this IHostApplicationBuilder builder,
		Assembly assembly
	)
	{
		var assetTypes =
			assembly
				.ExportedTypes
				.Where(x =>
					x.IsAbstract == false &&
					x.IsAssignableTo(typeof(Asset)) &&
					x != typeof(Asset)
				);

		foreach (var assetType in assetTypes)
		{
			builder.Services.AddSingleton(new AssetTypeEntry(assetType));
		}

		return builder;
	}
}
