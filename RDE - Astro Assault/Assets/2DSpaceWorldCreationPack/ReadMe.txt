Well hello there and thanks for purchasing this awesome 2D Space Pack!

First of all, I hope that the game you will make will be amazing as hell. I mean, you got unity, the amazing community of it and now even
a working space environment! If you are done, feel free to send me a notice of your game, I always love to find new things.

So, enough of the pep talk and greetings, let get down to the juicy parts!

0. How do I set this package up and why doesn't do it automatically?
So, to get allow all scripts to do their purpose, you need to do the following:
0.1. Add a tag called "PlayerShipTag" and apply that to the player. Alternatively, you can also use your own tag for the player. Just make sure
to change it in both parallax Scripts, where it looks for the player to have a reference point
0.2. Add 4 SpriteRenderer Sorting Layers by the names: "FurthestBack", "Back", "Middle" and "FrontBack". For more information on the layers, look
to point No. 5.
0.3 Go to "Objects" -> "5 Noise Nebula" -> "Prefabs" and give the prefabs the fitting Sorting Layer (61=FrontBack, 62=Middle, 63=Back).

1. What will this package do?
It will provide you with everything you need for a space scene, considering optical ambience. This encompasses the following:
- 40/52 Planet Prefabs, divided into 5 categories: Terran, Lava, Ice, Toxic and city (some of the cities are meshed with the other types!).
- 38 Asteroids, with 4 different formations in which they spawn.
- 5 Dust Clouds which form small to medium sized dust blobs.
- 4 diffuse Nebulas, which will add highlights to the scene.
- 1 Shipwreck. As I don't know which Sprite Style you are going for in the ships, this serves more as an example.
- 12 different background Fog basis, which create a randomly colored ambience for the entire scene (three are extra long for sidescrollers).
- 2 Starfield Background prefab, which are used as different layers of stars in the far back
- Diverse Scripts that make the space-creation magic work.
Aside from that, there are a few helper Scripts and Sprites, like a simple Movement Sprite or a simple Camera-Follows-Player Script.


2. How does this package work?
The Prefabs work in a rather tight, hierarchical order. I suggest you compare with the ScriptNet png I included for better understanding.
At the core is the WorldSpawner Script. This instantiates every object in sufficient quantity and tells the object on which layer it should be.
The Next step are (usually) the Spawners, which take their respective Input (e.g. Asteroids in the Asteroid Formations) and instantiate them.
Finally, the instantiated objects have their own script, which makes them rotate, increase or shrink in size, etc.
You will find that different prefabs work a bit differently. This was mainly done to show different ways to get objects in the scene.
If you want to add your own objects, look at the scripts and just pick the "style" you like most or have the easiest time understanding.


3. What are these Styles?
Sheesh, go into the Scripts and find out!
Just kidding, of course I will explain it here:
3.1 First, we have the Planets. They have their own Scripts tailored to them, to suit for parallax and to enable easier manipulation
within the game. This will be important if you have a randomly generated universe and want to make sure that ice world have ice planets.
Planets are automatically assigned to the FurthestBack Layer.

3.2 Second are the Asteroids, the basic example. The Asteroid field spawners work by instantiating the Asteroid Prefab (which takes in all
Asteroid Sprites) over and over again along a certain path to create an arc or a ring. Said Prefab randomly picks one of the Sprites, therefore
every Asteroid Field looks different, even if it has the same base formation.
Asteroids are on the layer that the World Spawner tells them to spawn on.

3.3 Third would be the Nebula Spawner, which instiates a random nebula Prefab. Compared to the Asteroid Script, we don't have one Asteroid
Prefab with different Sprites, but one Prefab for each, in case you want to give each nebula Prefab different base values, for example.
Nebulas are on the layer that the World Spawner tells them to spawn on.

3.4 Fourth, the Dust Spawner are a combination of Asteroid Fields and Nebulas. They will instantiate the Dust Prefabs (like at the nebulas), but
will do so multiple times like an Asteroid Formation. More work to put everything in, but has the advantages of both. 
Dust Clouds are on the layer that the World Spawner tells them to spawn on.

3.5 Fifth is the Wreck. Here, the Prefab has a hardcoded number of child-prefabs, all of which are a piece of a broken ship. The disadvantage
is, that you sacrifice some flexibility as you can't simply increase or decrease the number of broken parts in Script. However, like this you
can easily see how the Prefab looks in the game without having to look at it at runtime. Easier manipulation, but less flexibility.
The Wreck is on the layer that the World Spawner tells it to spawn on.

3.6 The Background Fog. The FogSpawner instantiates 2-3 Fogs (which should cover the entire gameworld), and that's about it.
The Fogs have their layer hardcoded into them-


4. What are these layers you are talking about?
And another good question!
The Layers are quite an easy way to tell how far away an object is from the "plane of movement", ergo where the player flies on. While objects in the front move relatively fast compared to the player and are of brighter color, the further in the back something is, the darker and relatively slower it is. I suggest reading up on the "parallax effect" to explain how this works. Basically, the layers are the way of using
said effect to get depth into the game.
I use the Sorting layer of the Sprite Renderer, as I want to make sure that objects in the back are indeed showing up behind objects in the front. This also serves as a flag for the parallax effect strength and the Sprite darkening.


5. Why are the Starfields not in the World Spawner and what are they?
The Starfields are little white dots on a transparent & black background. These dots move relative to the player (parallax effect) and 
provice with a feeling of being in space, as you can see the different distant suns. Sure, more colors would be realistic, but I abide to
the sci-fi clichées everyone expects.
The reason they are not in the World Spawner is simple: They dont need to be. They are so small and distant tiny atmospheric speckles that
in all the trials I had with them, there was never a reason to instantiate them. The Rest of the Random instantiation make this wohle unique
enough.

6. Anything else you want me to notice?
Oh yeah, sure. Two things, actually. Thing 1: The red borders are the limimts of the level. If you fly beyond them, the simulation will break as the
prefabs only get instantiated within and the fogs end after that. If you need bigger levels, I suggest downscaling everything or increasing the
spawnrate of objects, the borders of spawning (see WorldSpawnerScript) and scale of the background noises.
Second are the background star fields. They do not cover the entire camera, leaving small areas without stars to the left and the right. While
standing still, their borders are rather clear cut. However, if you move, the stars appear to be smoothly spawning and flying along (if you pay 
close attention, at least). The few testers that noticed a difference between a camera-encompassing starfield and the small borders is miniscule,
however, most of them did like it better like that, as it, I quote: "felt more dynamic". If you want it differently, change the camera size or the
starfields scale.

7. But...but... but... I still have a question!
Oh, sure! Feel free to shoot me an email whenever you get stuck on anything. I'll gladly help you to improve your project!
So, without further ado, my Email and my farewell:
max.al.braun@gmail.com

Looking forward to your creation (meaning: if you use this, please send me a demo or a link to the final game, I'd love to see whatever you brew up)!
We rock!
Max