public static class UnianioConfig
{
    public const string FactoryType = "CustomAssembly.CustomFactory, CustomAssembly";

    public static readonly string[] Assemblies =
    {
        "Unianio", "Assembly-CSharp"/*, "CustomAssembly"*/
    };
}
