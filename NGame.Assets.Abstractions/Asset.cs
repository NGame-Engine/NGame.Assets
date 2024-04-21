namespace NGame.Assets;



public abstract class Asset
{
	public Guid Id { get; init; }
	public string PackageName { get; init; } = "Default";
}
