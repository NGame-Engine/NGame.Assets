using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NGame.Assets.Serialization;

namespace NGame.Setup;



public static class NGameAssetsInstaller
{
	public static IHostApplicationBuilder AddNGameAssets(this IHostApplicationBuilder builder)
	{
		builder.Services.AddTransient<IAssetDeserializerOptionsFactory, AssetDeserializerOptionsFactory>();
		builder.Services.AddTransient<IAssetSerializer, AssetSerializer>();
		builder.Services.AddSingleton<IAssetRegistry, AssetRegistry>();
		builder.Services.AddTransient<IStoredAssetReader, SimpleStoredAssetReader>();
		builder.Services.AddTransient<IAssetAccessor, AssetAccessor>();

		return builder;
	}
}
