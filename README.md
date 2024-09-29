# NullSignal
Regarding my C# experience, I believe this project would be the best example, as due to it being my most recent project, involves a lot of my modern coding practices, which would best showcase my skill level today.

Null Signal is a game I've been developing, heavily inspired by [Signal Simulator](https://store.steampowered.com/app/839310/Signal_Simulator/) and [Voices of the Void](https://mrdrnose.itch.io/votv). The gameplay revolves around scanning stellar bodies in space, such as stars and planets, to conduct research. Players can send this research off for in-game credits to purchase upgrades and items. During scans, various events can occur, ranging from minor events such as strange noises coming from a planet, to catastrophic events like a star exploding.

# Star Systems
Given the variety of stars and planets in the game, I developed a system to easily define different stellar bodies and attach unique events to them. Most of this functionality is found in the [Astro](Assets/Scripts/Astro) folder. Bodies in space are discovered through Echos, which are defined as ScriptableObjects ([Echo](Assets/Scripts/Astro/Echo.cs)). Each Echo specifies the type of body (E.g: star, planet, asteroid) and its difficulty to detect. It also references its source ([EchoSource](Assets/Scripts/Astro/EchoSource.cs)), another ScriptableObject that defines the visual properties of the Echo on the galaxy map.
![image](https://github.com/user-attachments/assets/7036b3a5-504f-4a6b-a8c4-8bd1ec09397e)

To manage star systems, I created a [StarSystem](Assets/Scripts/Astro/StarSystem.cs) class, which inherits from [EchoSource](Assets/Scripts/Astro/EchoSource.cs). This class defines the various bodies within a system that the Echo points to, using an array of SystemBody objects. These bodies not only define visual properties but also reference a [StellarBody](Assets/Scripts/Astro/Bodies/StellarBody.cs), a ScriptableObject used to define the specific stellar bodies players can find.

Once these Echos and Stellar Bodies are set up in the Unity Editor, they are assigned to an [EchoSpawner](Assets/Scripts/GameObjects/Objects/Space/EchoSpawner.cs) component. The spawner uses a serialized dictionary to assign Echos to different rarities, from Very Common to Exotic. Each rarity key contains an array of Echos, allowing for multiple Echos per rarity. When the spawner is triggered, it generates a number between 0 and 100, representing a percentage. Based on the result, the system selects a rarity and randomly picks an Echo from the corresponding array.

**Note: Some classes within the Astro folder are not used. Sorry for the confusion.**

### Custom Editor
I created a custom editor for the [StarSystem](Assets/Scripts/Astro/StarSystem.cs) ScriptableObjects to make them easier to work with in the Unity Editor. It makes things a lot clearer, and was a great chance to improve my skills in developing a custom editor. You can find the custom editor here: ([StellarSystemEditor](Assets/Scripts/Core/Editor/StellarSystemEditor.cs))

![image](https://github.com/user-attachments/assets/1bd0c422-d39b-4e6a-a20b-402c96ba57d0)

# UI
This project features quite a lot of UI work, including both screen-space and world-space UI, which is all organised within the [UI](Assets/Scripts/UI) folder. 

With most of my work, especially the UI, I aim to decouple the code as much as possible using C# Actions. The UI works by listening to events triggered by the game logic, ensuring that the game logic itself has no direct references to the UI. This reduces dependency issues, including if the UI encounters any problems, meaning the core gameplay won't be affected. It's difficult to highlight a single example from the UI that stands out among the rest. For my work on world space UI, you can find it in the [World](Assets/Scripts/UI/World) folder.



