namespace NGame.Assets;



[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class AssetAttribute : Attribute
{
	public string? Name { get; set; }

	/// <summary>
	/// The key used to identify this asset type in a scene asset file.
	/// <para>
	/// If this is not set, the fully qualified filename will be used.
	/// That works fine, but if the namespace or class name is changed,
	/// the asset type will not be recognized in a previously saved scene.
	/// </para> 
	/// </summary>
	public string? Discriminator { get; set; }


	public static string GetName(Type type) =>
		GetAttribute(type)?.Name ?? type.Name;


	public static string GetDiscriminator(Type type) =>
		GetAttribute(type)?.Discriminator ?? type.FullName!;


	private static AssetAttribute? GetAttribute(Type type) =>
		type
			.GetCustomAttributes(false)
			.Where(x => x is AssetAttribute)
			.Cast<AssetAttribute>()
			.FirstOrDefault();
}
