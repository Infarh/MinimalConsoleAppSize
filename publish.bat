@echo off

if exist "release" (
  rd /S /Q release > nul
)

if exist "Hasher.exe" (
  del Hasher.exe
)

dotnet clean --nologo -v m

dotnet publish --nologo -c release -o release

copy release\Hasher.exe .\Hasher.exe

rd /S /Q release