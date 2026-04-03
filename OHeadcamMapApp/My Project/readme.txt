
INSTALLING

1. ffmpeg http://sourceforge.net/projects/mplayer-win32/files/FFmpeg/   
Tested with version git-N-30172-g3c20c0e  2011-05-24
Pleade use this first even if there is more recent versions availabe. 
There has been prblems with more recent versions, those just may not work.

2. Virtualdub v1.9.11  http://www.virtualdub.org/

3. Virtualdub deshaker plugin http://www.guthspot.se/video/deshaker.htm
Copy Deshaker.vdf to virtualdub's "plugins" subfolder.

4. Extract RGmapvideo.zip to a folder. I recommend using folders without
spaces, just in case.

5. Open RGmapvideo.ini. At the end of fiel there is paths to ffmpeg.exe 
and virtualdub.exe. Chance those to match your installation.

TESTING

1. Shoot about 5 min video with your camera and copy video file to your computer.
2. Open RGmapvideo.ini. In the beginnig there is VIDEO parameter. Type your 
video's name there (with correct path). Then run go.bat. Best if you open 
command promth window and run go command there, you'll see error messages if 
program exists immediately. If everything goes well you'll have to wait about 
4x video's duration and youll get mp4 video file with maps, graphs and all. 
Then everyithing should is set up for real use.

USING

- Open ini and take a look at parameters. Lots of them, tweak them as you 
wish.

- Take a look at the included gpx file. If you like to get HR graph you need to 
make a gpx like that, gpx with HR data.

- you need RouteGadget or quickroute (latest development version December 2011). Map and route is downloaded 
from there. More about how to install RG can be found here: http://www.routegadget.co.uk/

- if needed, crop beginnign of your gpx file to make it start when the actual 
start was. To make it match with video and splits times.
- this tool can crop yout video. Just look from your video when start is and type 
that in milliseconds to ini's CROPSTART parameter
- you can fine tune video & gpx and spilt times difference with SPLITSOFFSET 
paremeter. 

Take a look at go.bat file. Four steps of the process are there as separate 
commands. You can re-run later steps without having to run all steps from the 
beginning. For uing different parameters or if you like tweak intermediate files 
before writing out the final video file.
