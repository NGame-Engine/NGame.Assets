namespace NGame.Assets.Serialization;



public interface IAssetProcessor
{
	Type Type { get; }
	void Load(Asset asset, byte[]? companionFileBytes);
	void Unload(Asset asset);
}



public abstract class AssetProcessor<TAsset>
	: IAssetProcessor where TAsset : Asset
{
	public Type Type => typeof(TAsset);

	protected abstract void Load(TAsset asset, byte[]? satelliteFileBytes);
	protected abstract void Unload(TAsset asset);


	void IAssetProcessor.Load(Asset asset, byte[]? satelliteFileBytes) =>
		Load((TAsset)asset, satelliteFileBytes);


	void IAssetProcessor.Unload(Asset asset) =>
		Unload((TAsset)asset);
}
