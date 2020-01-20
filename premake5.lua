workspace "AdvancedShaders"
	architecture "x64"
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
		"%{prj.name}/src/**.cpp"
	}

	includedirs
	{
		"%{prj.name}/include",
		"Dependancies/include/"
	}
	
	libdirs 
	{ 
		"Dependancies/include/"
	}
	
	links 
	{
		"opengl32",
		"GLFW3"
	}
	
	