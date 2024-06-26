using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace NGame.Assets.Serialization;



public interface IAssetDeserializerOptionsFactory
{
	JsonSerializerOptions Create(IEnumerable<Type> assetTypes);
}



public class AssetDeserializerOptionsFactory(
	IEnumerable<JsonConverter> jsonConverters
)
	: IAssetDeserializerOptionsFactory
{
	public JsonSerializerOptions Create(IEnumerable<Type> assetTypes)
	{
		var options = new JsonSerializerOptions
		{
			TypeInfoResolver = CreateTypeInfoResolver(assetTypes)
		};

		foreach (var jsonConverter in jsonConverters)
		{
			options.Converters.Add(jsonConverter);
		}

		return options;
	}


	private static DefaultJsonTypeInfoResolver CreateTypeInfoResolver(IEnumerable<Type> assetTypes)
	{
		var assetPolymorphismOptions = CreatePolymorphismOptions(assetTypes);


		return new DefaultJsonTypeInfoResolver
		{
			Modifiers =
			{
				x =>
				{
					if (x.Type != typeof(Asset)) return;
					x.PolymorphismOptions = assetPolymorphismOptions;
				}
			}
		};
	}


	private static JsonPolymorphismOptions CreatePolymorphismOptions(IEnumerable<Type> assetTypes)
	{
		var jsonPolymorphismOptions = new JsonPolymorphismOptions
		{
			TypeDiscriminatorPropertyName = AssetConventions.TypeDiscriminatorPropertyName,
			IgnoreUnrecognizedTypeDiscriminators = true,
			UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization
		};

		foreach (var assetType in assetTypes)
		{
			if (assetType.IsAssignableTo(typeof(Asset)) == false)
			{
				var message = $"Type {assetType} is not a subtype of {nameof(Asset)}";
				throw new InvalidOperationException(message);
			}

			var typeDiscriminator = AssetAttribute.GetDiscriminator(assetType);
			var jsonDerivedType = new JsonDerivedType(assetType, typeDiscriminator);
			jsonPolymorphismOptions.DerivedTypes.Add(jsonDerivedType);
		}

		return jsonPolymorphismOptions;
	}
}
