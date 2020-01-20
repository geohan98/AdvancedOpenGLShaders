workspace "OpenGL"
	architecture "x64"
	startproject "OpenGL"
	systemversion "latest"
	cppdialect "C++17"

	configurations
	{
		"Debug"
	}

outputdir = "%{cfg.buildcfg}-%{cfg.system}"

project "OpenGL"
	location "OpenGL"
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
		"vendor/STB/stb_image.cpp"
	}

	includedirs
	{
		"vendor",
		"vendor/GLFW/include",
		"vendor/GLEW/include",
		"vendor/GLM",
		"%{prj.name}/include",
		"vendor/ASSIMP/include"
	}
	
	libdirs 
	{ 
	"vendor/GLFW/lib", 
	"vendor/GLEW/lib",
	"vendor/ASSIMP/lib"
	}
	
	links 
	{
		"opengl32",
		"GLFW3",
		"glew32s",
		"assimp"
	}
	
	