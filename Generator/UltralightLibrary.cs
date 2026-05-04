using CppSharp;
using CppSharp.AST;
using CppSharp.Generators;

namespace Generator;

internal class UltralightLibrary(string basePath) : ILibrary
{
	private readonly string _basePath = basePath;
	public void Preprocess(Driver driver, ASTContext ctx)
	{

	}

	public void Postprocess(Driver driver, ASTContext ctx)
	{
	}

	public void Setup(Driver driver)
	{
		var options = driver.Options;
		options.GeneratorKind = GeneratorKind.CSharp;
		options.OutputDir = Path.Combine(AppContext.BaseDirectory, "Generated");
		
		var module = options.AddModule("Ultralight");
		module.IncludeDirs.Add(Path.Combine(_basePath, "include"));
		module.Headers.Add(Path.Combine(_basePath, "include/Ultralight/CAPI/CAPI_Ultralight.h"));
		module.LibraryDirs.Add(Path.Combine(_basePath, "lib"));
		module.Libraries.Add("Ultralight.lib");
	}

	public void SetupPasses(Driver driver)
	{
	}
}