# NullSignal
Regarding my C# experience, I believe this project would be the best example, as due to it being my most recent project, involves a lot of my modern coding practices, which would best showcase my skill level today.

# Star Systems
Null Signal is a game I've been developing that takes heavy inspriation from [Signal Simulator](https://store.steampowered.com/app/839310/Signal_Simulator/) and [Voices of the Void](https://mrdrnose.itch.io/votv). The gameplay resolves around scanning stellar bodies within space such as Stars and Planets to perform research on them, which you can send off for in-game credits to purchase upgrades and various items. When scanning bodies, events can sometimes occur which can range from something mild such as strange noises coming from the Planet below, or the entire sun blowing up.

As my game is going to include a lot of different Stars and Planets, I wanted to create a system that would easily allow me to define various bodies, and attach unique events to them. You can find the majority of this work within [Astro](Assets/Scripts/Astro). The bodies within Space are discovered by the Player through Echos, which is defined as a ScriptableObject ([Echo](Assets/Scripts/Astro/Echo.cs)). This defines the type of Echo it is (Star, Planet, Asteroid, etc) and its difficulty to discover. It also contains a reference to the source of the Echo ([EchoSource](Assets/Scripts/Astro/EchoSource.cs)), which is another Scriptable Object mainly used to define the visual properties of the Echo on the galaxy map in-game. 

The way I've set it up currently is that I have a Star System class that inherit from EchoSource ([StarSystem](Assets/Scripts/Astro/StarSystem.cs)). This is used to define the various bodies within the system that the Echo points to, containing an array of System Bodies, which are also used for defining the visual properties for the galaxy map, as well as containing a reference to a Stellar Body ([StellarBody](Assets/Scripts/Astro/Bodies/StellarBody.cs)), which is another Scriptable Object used for defining the various bodies the player can find in space. 

Once these Echos and their Stellar Bodies have been defined within the Unity Editor, I then assign them to a component called the Echo Spawner ([EchoSpawner](Assets/Scripts/GameObjects/Objects/Space/EchoSpawner.cs)). It contains a unique Serialised Dictionary, which allows me to specify the rarity of each Echo, ranging from "Very Common" to "Exotic". The dictionary contains an array of Echos for each rarity, meaning I can assign multiple Echos to the same rarity. When the Spawner is called to spawn a Echo, it will generate a number between 0 and 100, which acts as the percentage. If a rarity falls within that percentage, it will then random pick an Echo from the array. 

### Custom Editor
I decided to also create my own Custom Editor for the Star System Scriptable Objects, as it made it a lot clearer when working with it, and was a great chance to improve my skills with developing my own custom editor. You'll find this editor within here: ([StellarSystemEditor](Assets/Scripts/Core/Editor/StellarSystemEditor.cs))

![image](https://github.com/user-attachments/assets/1bd0c422-d39b-4e6a-a20b-402c96ba57d0)


**I want to note that some classes within the Astro folder are not used. I'm sorry for the confusion.**

# UI
This project also contains quite a lot of UI work, including screen and world space UI. All of this is defined within the [UI Folder](Assets/Scripts/UI).

Like with most of my project, but espeically with UI, I try to decouple my code as much as possible through the use of C# Actions. The UI works by listening to events triggered by the game logic, meaning that the game logic has no reference to the UI at all, resulting in far less dependency issues. It means if the Ui breaks in anyway, the game won't completely break with it. It's rather difficult to pinpoint an example from the UI that stands out among the rest, and some of it is unused currently. For my work on World Space UI, you'll find that within the [World Folder](Assets/Scripts/UI/World)



