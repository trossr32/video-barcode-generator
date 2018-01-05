SET "sln=C:\Users\Rob\Source\Repos\film-barcodes\FilmBarcodes.sln"
SET "projDir=C:\Users\Rob\Source\Repos\film-barcodes\BarcodeManager\"
SET "sysDir=C:\windows\system32\"
SET "exeDir=Q:\executables\BarcodeManager\"

if not exist "C:\nuget\" mkdir C:\nuget
if not exist C:\nuget\nuget.exe (
	%sysDir%bitsadmin.exe /transfer "Getting nuget.exe..." https://dist.nuget.org/win-x86-commandline/latest/nuget.exe C:\nuget\nuget.exe
)
C:\nuget\nuget.exe update -self
C:\nuget\nuget.exe restore %sln%

"%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin\MSBuild.exe" %projDir%BarcodeManager.csproj /t:rebuild /property:Configuration=Release

::%sysDir%timeout /t 1
%sysDir%robocopy %projDir%bin\Release %exeDir% /MIR
::%sysDir%timeout /t 1

%exeDir%BarcodeManager.exe