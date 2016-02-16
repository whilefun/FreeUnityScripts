:
: Unity Backup Archiver
: Richard Walsh 2014
: While Fun Games
: http://whilefun.com
:
: A simple batch file that creates a stripped down "backup" archive of a Unity project.
: Archive is based on the directory this batch file is in, named after projectName below.
: Excludes .psd, .blend/.blend1/.blend2, .rar, .bat, and the Library directory
: For best results, ensure Unity is closed.
:
: Note: Requires WinRAR (built and tested with WinRARx64 v3.90)
:
: 1. Place this batch file in Unity project folder
: 2. Change the value of projectName to your project or whatever (e.g. set projectName=MyProject)
: 3. Double click the batch file, and an archive should pop out.
:
@ECHO OFF

: Change this to your project name
set projectName=Buzzing

: --------------------------------------------
set timestamp=%date:~6,4%%date:~3,2%%date:~0,2%
set /a version=1
set filename=%projectName%%timestamp%-%version%.rar

:loop
set filename=%projectName%%timestamp%-%version%.rar
echo.%filename%

if exist %filename% (
	
	set /a version+=1
	goto loop
	
) else (

	echo.%filename% does not exist, creating archive...
	rar a -r -x*.bat -x*.bar -x*.rar -x*.pdn -x*.blend1 -x*.blend2 -x*.psd -x*.blend -x*.exe -x*\research\* -x*\research -x*\*_Data -x*\*_Data\* -xLibrary %filename%
	goto finished
	
)

:finished
echo.All done!
:pause
exit