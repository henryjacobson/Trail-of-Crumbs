# Trail of Crumbs

A stealth game developed by me and three other students.

## Play the Game

The third level was too large to upload to WebGL so here is the link to levels 1 and 2 and the final boss fight.

https://jacobsonh.itch.io/trail-of-crumbs

And here is the full game windows download.

https://drive.google.com/file/d/1nD_HutgJGW2WR-TEeCh8_SDIU6LWbY8h/view?usp=sharing

## My Contributions

I implemented all enemy AI with multiple FSMs and navigation mechanics, models from various sources, animations from Mixamo, and behavior including a complex final boss fight with three stages. I also wrote the code for and designed the intro sequences for each level. I also integrated exisiting menu asset to control settings and pause.

### Some of the scripts I wrote

* [EnemyAI](Assets/Scripts/Enemies/EnemyAI.cs)
* [IntroBehavior](Assets/Scripts/Intro/IntroBehavior.cs)
* [ConductorAI](Assets/Scripts/ConductorBehavior.cs) (second level boss)
* [CrumbsBanksAI](Assets/Scripts/CrumbsBanksAI.cs) (final boss)

I also assissted in level design, the level manager, and many smaller details along the way.

## Game Design Document
* __CHARACTERS__
  * __Player:__ The player is a secret agent and has a hand that acts like a grappling hook.
    * They are unnamed.
  * __Super Private Assassination Conglomerate Enterprise (S. P. A. C. E):__ The organization of secret agents the player is a part of. They are tracking intel on Crumbs Banks.
  * __Cell Prime:__ A high-ranking agent with intel on Crumbs Banks. They are locked in a remote prison by Crumbs Banks, and need to be freed so that they can give their information.
  * __Crumbs Banks:__ The pseudonym of the leader of an intergalactic smuggling operation. They are smuggling goo and need to be stopped.
  * __Robots:__ Crumbs Banks has an army of robots that protect him and his operation.
* __STORY/NARRATIVE__
  * For training, the player is assigned some missions with regards to the renowned space criminal Crumbs Banks. 
  * The player’s first training mission is to free Cell Prime from the prison in which they are held. Their second training mission is to destroy the very illegal cargo that is being transported on one of Crumbs Banks’s big trains. 
  * The player passes their training after completing these missions and is then tasked with the final mission of assassinating Crumbs Banks themself.
* __GAME WORLD__
  * A futuristic world with big sci-fi cities and space stations.
  * Settings for the levels:
    * __First level:__ a large prison complex.
    * __Second level:__ A very large & moving train.
    * __Third Level:__ A huge space station where Crumbs Banks is supposedly in hiding.
  * Each level has many spotlights and guards with flashlights to avoid, as well as many hooks, ledges, and other objects to grab onto. Level 3 includes gravity fields.
* __GAMEPLAY__
  * There are various objectives for each level:
    * Save the target and escape
    * Destroy goo and defeat the target
    * Defeat the target
  * Long term objective: Take out Crumbs Banks
  * Avoid detection by lights + enemies
  * Genre: Puzzle/Stealth
* __GAME MECHANICS__
  * __Player mechanics: __
    * Using a grapple-hand to reach far places or grab objects that are otherwise out of reach.
    * Player can walk, crouch, and jump
    * Their perspective is in first-person, and they can aim and launch the grapple hand to grab things.
    * You can only grab things that are designated as “grabbable.” This includes small objects and specific points in the environment.
      * Things that can be grabbed will be distinguished in several ways, which may include:
        * Giving the object a colored glow
        * Showing text that says it can be grabbed
    * If the grapple hand grabs an object, the object is pulled towards the player for them to hold.
    * If the grapple hand grabs part of the environment, the player is pulled towards that point and hangs stationary from it. They must let go or jump from the point to use the grapple hand again.
    * The grapple hand can only reach a certain distance.
    * Sometimes, the grapplehand can be used to destroy or interact with objects without grabbing them.
  * __Enemy mechanics:__
    * Enemies patrol a designated area.
    * The enemy can catch the player by shining their light on them.
    * Enemies have two states: patrol and alerted.
      * In patrol, the enemy will just patrol their area on a cycle.
      * When the enemy sees the player’s grapple hand, they investigate the source for a few seconds
    * The enemy can be defeated if the player has the attack powerup
* __ITEMS/LOOT/POWERUPS__
  * __Gravity mechanics:__
    * In certain parts of the space station level, designated by glowing pink columns, gravity will be inverted, allowing the player to walk on the ceiling.
  * __Keys:__
    * The player can grab keys that can be used to unlock doors.
  * __Goo Pods:__
    * Pods can be destroyed in the second and third levels. In the second level, they unlock the door to the next area.
  * __Other powerups:__ There are several spots within the levels that can provide temporary system upgrades to your grapple hand. Once they gain the upgrade, they have a limited amount of time to use it before they lose it.
    * An upgrade that removes the distance limit on the grapple hand, allowing the player to reach any location.
    * An attack upgrade that allows the player to kill enemy robots.
  * __Colored Lights:__
    * If the player enters the light the level is over
    * They can be toggled by hitting a button with the grapplehand so only one color is showing at a time.
  * __Boss Fight:__
    * Crumbs Banks flies around the room shooting fireballs at the player.
    * When the player hits him with their grapple hand, he makes a shield using shield generators that the player must destroy.
    * After three hits Crumbs Banks dies and the game is completed
* __GAME RULES__
  * Avoid being caught in the light of an enemy’s flashlight or a spotlight
  * Don’t get seen by enemies
  * Exit the scene after the assassination without being caught 
  * If the player is caught by a light or by an enemy, they must restart the level from the beginning.
