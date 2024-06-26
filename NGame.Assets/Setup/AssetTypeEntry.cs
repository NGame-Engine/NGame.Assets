using NGame.Assets;

namespace NGame.Setup;

public class AssetTypeEntry
{
	public AssetTypeEntry(Type subType)
	{
		if (subType.IsAssignableTo(typeof(Asset)) == false)
		{
			throw new InvalidOperationException($"{subType} is not a sub type of {nameof(Asset)}");
		}

		SubType = subType;
	}


	public Type SubType { get; private init; }
}
