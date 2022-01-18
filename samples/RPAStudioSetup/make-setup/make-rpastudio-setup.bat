:: RPAStudio教学版安装程序制作
@setlocal enableextensions
@cd /d "%~dp0"

@echo off

set product_name=RPAStudio教学版设计器
set robot_name=RPARobot教学版执行器
set setup_exe_name=RPAStudioLearnSetup
set product_copyright_year=2022
set product_version=1.0.0.1

setlocal EnableDelayedExpansion
set setup_name=%~n0
title %setup_name%

rmdir /s /q _rpastudio_
xcopy /e /y RPAStudio _rpastudio_\
xcopy /e /y RPAStudio-dependencies\Python _rpastudio_\Python\
xcopy /e /y RPAStudio-dependencies\OfflinePackages _rpastudio_\OfflinePackages\
xcopy /e /y RPAStudio-dependencies\dotnet _rpastudio_\dotnet\

"..\nsis\NSIS-Unicode\makensis.exe" /RAW /NOCD /DPACKAGE_DIR="_rpastudio_" /DSETUP_EXE_NAME="%setup_exe_name%" /DPRODUCT_NAME="%product_name%" /DROBOT_NAME="%robot_name%" /DPRODUCT_COPYRIGHT_YEAR="%product_copyright_year%" /DPRODUCT_VERSION="%product_version%" "nsis-scripts\script.nsi" 

echo.
echo %Date% %Time%
if "%errorlevel%" == "0" (echo Package "%setup_name%" success & pause>nul ) else (echo Package "%setup_name%" failed & pause>nul )

rmdir /s /q _rpastudio_