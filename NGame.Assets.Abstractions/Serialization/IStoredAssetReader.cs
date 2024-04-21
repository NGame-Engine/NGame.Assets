namespace NGame.Assets.Serialization;



public interface IStoredAssetReader
{
	string GetAssetJson(Guid assetId);
	byte[]? GetAssetData(Guid assetId);
}
