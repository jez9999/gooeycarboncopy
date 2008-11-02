;Carbon Copy install script (Inno Setup)

[Setup]
AppName=Carbon Copy
AppVerName=Carbon Copy v2.1.1.0
DefaultDirName={pf}\Carbon Copy
MinVersion=4.0,4.0
PrivilegesRequired=none
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
  NetFrameWorkInstalled := RegKeyExists(HKLM, 'SOFTWARE\Microsoft\.NETFramework\policy\v2.0');
  if NetFrameWorkInstalled then
  begin
    // .NET 2.0 installed; OK.
    Result := true;
  end
  else begin
    // .NET 2.0 not installed; prompt to install.
    // (note: MB_DEFBUTTON1 flag defaults the selected button to the first button in the dialog)
    QueryResult := MsgBox(
      'This application requires the .NET Framework 2.0.  Please download and install the .NET Framework 2.0 and run this setup again.  Do you want to download the framework now (clicking on No will continue the setup anyway without installing the framework, clicking on Cancel will abort the setup completely)?',
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
        // Yes clicked; go to .NET Framework 2.0 download page.
        ShellExec(
          'open',
          'http://www.microsoft.com/downloads/details.aspx?FamilyID=0856EACB-4362-4B0D-8EDD-AAB15C5E04F5',
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
;=== .net??? ===  http://www.microsoft.com/downloads/details.aspx?FamilyID=0856EACB-4362-4B0D-8EDD-AAB15C5E04F5
; ...
;===Program files===
Source: "..\Carbon Copy\bin\Release\CarbCopy.exe"; DestDir: "{app}"; Flags: replacesameversion promptifolder
Source: "..\Carbon Copy\bin\Release\JezUtilities.dll"; DestDir: "{app}"; Flags: replacesameversion promptifolder

[Icons]
;Create link on Start Menu Programs for program file
Name: "{group}\Carbon Copy"; Filename: "{app}\CarbCopy.exe"

