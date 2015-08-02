<h1>PlayMeidaWav</h1>
<p>Original by Robert Parker 2015 released under the <a href="http://opensource.org/licenses/MIT">MIT Licence</a> from his GitHub <a href="https://github.com/pyblendnet-js/playMediaWav">repository</a>.</p>
<h2>Introduction</h2>
<p>This program shows a list of available wave files as buttons and allows simultaneous playback.  It was created to access the system load in comparison with other playback methods.</p>
<h2>Setup</h2>
<p>All wave files must be located either in the executable directory or the path must specified in runtime argument.  To do this I have used a batch file - a sample of which is normally found in the "playMediaWav/bin/debug" directory but I have included a copy in the base directory so that github finds it.</p>
<h2>Key control</h2>
<ul>
<li>Press L to create an instance of media player for each wave file.</li>
<li>Press A to play all instances of media player - wave files are only loaded at this stage.</li>
</ul>
<h2>System Load</h2>
<p>By comparison, playMedia wave (26kB on disk) opens a seperate instance of WMPLib.WindowsMediaPlayer through the Windows Media Player wmp.dll
Memory min 18.4Mb, rises from 28mB to 30mB if ambiance is played. Rises to 32Mb if wave files are loaded as seperate media player instances.  Peaks at 225Mb if all 137 (29mB) wave files are played. Then stays at 215mB.  Seems that wav files are not loaded until played.
Some process called Windows Audio Device Graph Isolation used 22mB also seems to be involved.This drops to 17Mb when the playMedia process is closed.</p>
