spieluhr-demo
=============

What
----

It started with a simple question: 
*"What can you do with 100KB (self-contained, no streaming) and Unity (web) engine?"*

Spieluhr is a small psychedelic demo experience, written in 2011, using Unity3D 3.0. It was designed specifically for 
the web player.

How
----
_Trivia: This demo was programmed on a Sunday from ~3AM-9AM, powered by Hershey's chocolate bar and a bottle of pepsi._

At first it relied specifically on d3d9 and ps3.0 for smaller shader code path. I worked around that and at the end it supported gl code as well.
I wanted the unity3d web executable to be much smaller than 100KB. 
However, lack of low level SFX access and untouchable footprint (around 40KB at the time) made me go for 100KB (I really wanted to add some kind of sound on there).

Estimates by the time it was made states that further optimizations could be done to decrease final build size down to ~85KB but I didn't feel like doing a seconds pass on it.

Bullet points at the time:
* 1-2KB that could be easily removed from level data (scene overhead)
* maybe remove a couple more KB from shaderdata as well
* 20% of uncompressed data is SFX. SFX is one of the elements that benefits less from LZMA since it is already compressed down the ground way before cooking the final app. ~45KB of sfx equals to ~50% of the total app size btw
* hardest part would be removing unused code for a max of 5kb estimated size reduction.
* 42KB is part of the core (untouchable. you get it wheter you want it or not)
* 40-45KB for sfx due to lack of low level sound access. Gives a good range for stuff other than 8bit tunes.
* That roughly sums up to 87KB. Even micro optimizations favoring size instead of speed (i.e. watching closely for IL and final dll size) don't make me tick.


The main shader code was based on perlin noise from the Unity Community and the Standard Image Effects.

Downloads
--------

* Check the original web version: http://filippsen.github.io/spieluhr/spieluhr.html

The project has been converted to Unity 4. The following releases were built with Unity 4.2 Pro Trial:
* Download Windows version: https://github.com/filippsen/spieluhr-demo/releases/download/1.0/spieluhr-demo-win.zip
* Download Linux version: https://github.com/filippsen/spieluhr-demo/releases/download/1.0-linux/spieluhr-demo-linux.zip
* Download Mac version: https://github.com/filippsen/spieluhr-demo/releases/download/1.0-mac/spieluhr-demo-mac.zip

License
-------
Source code is released into the public domain. Read the LICENSE file.

Art assets and sounds are copyrighted and are not part of the public domain license statement.
