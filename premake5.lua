workspace "Ferret"
	configurations
	{
		"Debug x64", "Debug x86",
		"Release x64", "Release x86",
		"Dist x64",  "Dist x86",
	}

    filter "configurations:*x86"
	   architecture "x86"

    filter "configurations:*x64"
	   architecture "x64"


outputdir = "%{cfg.buildcfg}-%{cfg.system}-%{cfg.architecture}"


project "FerretEngine.Sandbox"
	location "FerretEngine.Sandbox"
	kind "ConsoleApp"
	language "C#"

	targetdir ("bin/" .. outputdir .. "/%{prj.name}")
	objdir ("obj/" .. outputdir .. "/%{prj.name}")

	files
	{
		"%{prj.name}/src/**.cs",
		"%{prj.name}/Content/**/*"
	}

	includedirs
	{
		"%{prj.name}/src",
		"%{prj.name}/Content",
	}

	filter "configurations:Debug*"
		defines "DEBUG"
		runtime "Debug"
		symbols "on"

	filter "configurations:Release*"
		defines "RELEASE"
		runtime "Release"
		optimize "on"

	filter "configurations:Dist*"
		defines "DIST"
		runtime "Release"
		optimize "on"
