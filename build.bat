@echo off
title ClearGOG Build Script

set CWD=%~dp0

if "%1" NEQ "" (
  if "%1" EQU "-c" goto clean
  if "%1" EQU "-d" goto restore
  if "%1" EQU "-h" goto help
  if "%1" EQU "-r" goto run
)

:build
dotnet publish -f net7.0-windows -c Release -v d
echo build complete

:restore
dotnet restore
goto end

:run
dotnet run -f netcoreapp3.1 -v d
goto end

:clean
if exist bin rmdir /S /Q bin
if exist obj rmdir /S /Q obj
echo Cleaning finished
goto end

:help
echo ClearGOG Build Script 1.0
echo Usage: build.bat [option]
echo Supplying no option will cause the build tool to build the app
echo.
echo Options
echo    -c              - Runs the binary cleanup
echo    -d              - Runs the dotnet restore command
echo    -h              - Shows this help text
echo    -r              - Runs a debug build of the app
goto end

:end
