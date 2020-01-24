workspace "AdvancedShaders"
	architecture "x86"
	startproject "AdvancedShaders"
	systemversion "latest"
	cppdialect "C++17"

	configurations
	{
		"Debug"
	}

outputdir = "%{cfg.buildcfg}-%{cfg.system}"

project "AdvancedShaders"
	location "AdvancedShaders"
	kind "ConsoleApp"
	language "C++"
	staticruntime "off"
	
	targetdir ("bin/" .. outputdir .. "/%{prj.name}")
	objdir ("build/" .. outputdir .. "/%{prj.name}")
	
	defines
	{
		"GLEW_STATIC"
	}

	files
	{
		"%{prj.name}/include/**.h",
		"%{prj.name}/src/**.cpp",
		"%{prj.name}/src/**.c"
	}

	includedirs
	{
		"%{prj.name}/include",
		"Dependancies/include/"
	}
	
	libdirs 
	{ 
		"Dependancies/lib/"
	}
	
	links 
	{
		"opengl32",
		"GLFW3",
		"assimp"
	}
	
	