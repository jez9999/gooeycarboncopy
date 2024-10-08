;Carbon Copy install script (Inno Setup)

#dim Version[4]
#expr GetVersionComponents("..\CarbonCopy\bin\Release\CarbCopy.exe", Version[0], Version[1], Version[2], Version[3])
#define VersionString Str("Carbon Copy v") + Str(Version[0]) + Str(".") + Str(Version[1])

[Setup]
AppName=Carbon Copy
AppVerName={#VersionString}
DefaultDirName={commonpf}\Carbon Copy
MinVersion=6.1sp1
PrivilegesRequired=admin
UninstallDisplayIcon={app}\CarbCopy.exe
;Default group under Start Menu Programs
DefaultGroupName=Carbon Copy
;zip compression is less efficient than lzma, but MUCH quicker
Compression=zip
;Random 50 character GUID
AppMutex=L0XM3u83MJWN9gGMFVSt350qy7qpJYxW3eZt29Ylda4EO3KY50
ChangesAssociations=no

[Code]
// We have a .NET dependency.  Make sure it's installed before setup starts.
function InitializeSetup(): Boolean;
var
  ErrorCode: Integer;
  NetFrameWorkInstalled : Boolean;
  QueryResult : Integer;
begin
  NetFrameWorkInstalled := IsDotNetInstalled(net48, 0); // NOTE: we should require 4.8 and NOT 4.8.1, which is not compatible with some older OSes...
  if NetFrameWorkInstalled then
  begin
    // .NET 4.8 installed; OK.
    Result := true;
  end
  else begin
    // .NET 4.8 not installed; prompt to install.
    // (note: MB_DEFBUTTON1 flag defaults the selected button to the first button in the dialog)
    QueryResult := MsgBox(
      'This application requires the .NET Framework 4.8.  Please download and install the .NET Framework 4.8 and run this setup again.  Do you want to download the framework now (clicking on No will continue the setup anyway without installing the framework, clicking on Cancel will abort the setup completely)?',
      mbConfirmation,
      MB_YESNOCANCEL or MB_DEFBUTTON1
    );
    if QueryResult = IDNO then
    begin
      // No clicked; we want to go ahead and install the application anyway.
      Result := true;
    end
    else begin
      // Yes or Cancel clicked; we definitely want to abort the setup.
      Result := false;

      if QueryResult = IDYES then
      begin
        // Yes clicked; go to .NET Framework 4.8 download page.
        ShellExec(
          'open',
          'https://dotnet.microsoft.com/en-us/download/dotnet-framework/net48',
          '',
          '',
          SW_SHOWNORMAL,
          ewNoWait,
          ErrorCode
        );
      end;

      // If Cancel was clicked, we do nothing more here.  Setup is merely aborted.
    end;
  end;
end;

[Files]
;===Dependencies===
;=== .net??? ===  https://www.microsoft.com/net/download/windows
; ...
;===Program files===
Source: "..\CarbonCopy\bin\Release\CarbCopy.exe"; DestDir: "{app}"; Flags: replacesameversion promptifolder
Source: "..\CarbonCopy\bin\Release\CarbCopy.exe.config"; DestDir: "{app}"; Flags: replacesameversion promptifolder
Source: "..\_Libs\GooeyUtilities.dll"; DestDir: "{app}"; Flags: replacesameversion promptifolder
Source: "..\CarbonCopy\bin\Release\JunctionPoint.dll"; DestDir: "{app}"; Flags: replacesameversion promptifolder

[Icons]
;Create link on Start Menu Programs for program file
Name: "{group}\Carbon Copy"; Filename: "{app}\CarbCopy.exe"
