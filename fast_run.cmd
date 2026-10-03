@echo off

@if exist ".\bin\AquaMate.exe" goto start

call clean.cmd
dotnet build AquaMate.sln /p:Configuration="Debug" /t:Rebuild /verbosity:quiet

:start
start .\bin\AquaMate.exe
