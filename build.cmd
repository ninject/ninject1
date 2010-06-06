@echo off
set nantfile=Ninject.build
set nantexe=tools\nant\nant.exe

%nantexe% -buildfile:%nantfile% clean %1 %2 %3 %4 %5 %6 %7 %8
IF ERRORLEVEL 1 GOTO Failed
%nantexe% -buildfile:%nantfile% "-D:package=true" package-source %1 %2 %3 %4 %5 %6 %7 %8
IF ERRORLEVEL 1 GOTO Failed
%nantexe% -buildfile:%nantfile% "-D:build.config=release" "-D:package=true" %1 %2 %3 %4 %5 %6 %7 %8
IF ERRORLEVEL 1 GOTO Failed

%nantexe% -buildfile:%nantfile% -q -nologo revert
IF ERRORLEVEL 1 GOTO Failed

echo "Release build completed."
GOTO End

:Failed
%nantexe% -buildfile:%nantfile% -q -nologo revert
echo "============================================================"
echo "BUILD FAILED"
echo "============================================================"

:End
pause