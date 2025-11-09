@echo off
set TargetFramework=%1
set ProjectName=%2

del "*.nupkg"
"..\..\oqtane.framework\oqtane.package\FixProps.exe"
"..\..\Oqtane.Framework\oqtane.package\nuget.exe" pack %ProjectName%.nuspec -Properties targetframework=%TargetFramework%;projectname=%ProjectName%
XCOPY "*.nupkg" "..\..\Oqtane.Framework\Oqtane.Server\Packages\" /Y