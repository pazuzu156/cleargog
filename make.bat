@echo off
set MS=C:\msys64\mingw64\bin
set PATH=%PATH%;%MS%

if exist cleargog.exe (
    del cleargog.exe
    del resource.syso
) else (
    windres -i resource.rc -O coff -o resource.syso
    go build
)
