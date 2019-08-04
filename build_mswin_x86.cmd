@echo off
set MSBDIR=@%WINDIR%\Microsoft.NET\Framework\v4.0.30319
%MSBDIR%\msbuild.exe AquaLog.sln /p:Configuration="Debug" /p:Platform="Any CPU" /t:Rebuild /p:TargetFrameworkVersion=v4.5

set BUILD_STATUS=%ERRORLEVEL% 
if %BUILD_STATUS%==0 goto test 
if not %BUILD_STATUS%==0 goto fail 
 
:fail 
pause 
exit /b 1 
 
:test 
set NUNIT="nunit_not_found"
@if exist "%PROGRAMFILES(X86)%\NUnit 2.6.4\bin\nunit-console-x86.exe" set NUNIT="%PROGRAMFILES(X86)%\NUnit 2.6.4\bin\nunit-console-x86.exe"
@if exist "%PROGRAMFILES(X86)%\NUnit.org\nunit-console\nunit3-console.exe" set NUNIT=@"%PROGRAMFILES(X86)%\NUnit.org\nunit-console\nunit3-console.exe" --x86
%NUNIT% AquaLog.Tests\bin\Debug\AquaLog.Tests.dll
pause 
exit /b 0