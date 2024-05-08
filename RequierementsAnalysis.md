# 1. Introduction

Small game
https://stormboard.com/storm/1886510/The_Magic_Apprentice_game

## 1.1. Purpose of the system

The purpose of the game is for the player to have fun. 

## 1.2. Scope of the system

The system is a video game developed with Godot 4 that allows the user to control a player character in order to kill monsters in dungeons and to improve their player characters strength through augments and unlocking skills. 

## 1.3. Objectives and success criteria of the project

The goal of this project is the creation of a single player RPG web game.

## 1.4. Definitions, acronyms, and abbreviations

**Player Character (PC)**: This term refers to the character controlled by the player within the game. The player's decisions and actions dictate the character's behavior and progression.<br/>
**Hit Points (HP)**: Represent the health or vitality of entities, such as player characters and enemies. Hit Points decrease when the entity takes damage. An entity is defeated or dies when its Hit Points reach zero.<br/>
**Augment**: A type of item that can be equipped by the player to gain additional statistics or effects. Augments are typically obtained as drops from defeated enemies.<br/>
**Load-out**: The combination of augments and magic skills a player currently equips. A player can equip up to five augments and three magic skills simultaneously. <br/>
**Area of effect (AOE)**: Area in which a damaging skill effects enemies or the PC. 

## 1.5. References


## 1.6. Overview


# 2. Proposed system


## 2.1. Overview

Player is a magician that explores dungeons. In these they fight slimes with 3 magic spells to get to the boss room, where they fight a boss.  
There are 2 types of dungeons. Automatically generated ones and story dungeons, that are hand crafted. Furthermore, the player can choose to curse a dungeon, which increases the difficulty by strengthening the enemies or weakening the player. Cursed dungeons give better rewards.
By defeating the boss, the player can unlock new magic skills of the 3 different magic types.

Players also get “augments”, which are used instead of armor. They have between 1 and 3 effects. These effects can be simple stat increases or additional effects (like casting one Skill when another is cast or increasing damaging area).
The player can wear a total of 5 augments at a time. Additional augments can be stored in an inventory. Slots are unlocked by playing story dungeons.

Players can only change their load-out (active augments and spells) outside of dungeons in the home town. This is done since the difficulty of generated dungeons is determined by the amount of augments the player is currently using.
The game is supposed to be finishable in under 2 hours. The idea is hence not to create a long game, but instead allow great replayability by having many different and interesting choices of playstyles and builds.

## 2.2. Functional requirements

### 2.2.1 Entities 

There are two types of entities, the PC and enemies. 

#### 2.2.1.1 Entity properties 

All entities have the following properties: 

| **ID: 2**| **Entity property: Max HP** |
| --- | --- |
| Description | Every entity has a max HP value that shows how much damage the entity can take before dying. |
| Acceptance Criterion | Has to be implemented |
| Notes | |

| **ID: 2**| **Entity property: Current HP** |
| --- | --- |
| Description | Every entity has a current HP value that shows how much damage the entity has already taken and can still take before dying. If the current HP value is smaller or equal to zero, the entity dies. |
| Acceptance Criterion | Has to be implemented |
| Notes | The relation of current HP to max HP should be visible for the player for all entities on screen. |

| **ID: 2**| **Entity property: Armor values** |
| --- | --- |
| Description | Every entity has three armor values for each of the three magic types in game. The armor type for a magic type determines how much the damage to the entity is reduced by an attack of that element (30 armor means 30% damage reduction). <br> If an entity has an armor value above 100, part of the damage is reflected back to the attacker. Example: Entity has 120 armor, attack makes 100 damage, then 20 damage is reflected back to the attacker, if the attacker has an armor value of 50, the attacker takes 10 damage. |
| Acceptance Criterion | Damage of all types is applied correctly to entities depending on their armor values. |
| Notes | The damage calculation works like <br> if armor <= 100: health = health - damage*(100-armor)/100 <br> Only PC should be able to reach over 100 armor through additional stats, enemies need at least one armor value below 30 unless they are buffed. <br> Case where both entities have over 100 armor of the same type has to be covered nonetheless. In that case both entites take no damage of this magic type. |

| **ID: 2**| **Entity property: Damaging skills** |
| --- | --- |
| Description | Every entity has one or more damaging skills. Every skill has a magic type and a base damage value. The base damage value can be modified under certain circumstances (curses for enemies, augments for the PC). A damaging skill only damages the opponent, that means the PC takes no damage from their own spells and enemy attacks only damage the PC. |
| Acceptance Criterion | Damage from different skills of all magic types is applied correctly to PC and enemies for armor values of 0. |
| Notes | The area in which a damaging skill applies damage to the opponent and the duration in which the opponent takes damage depends on the specific skill. |

| **ID: 2**| **Entity property: Speed** |
| --- | --- |
| Description | Every entity has a speed value that determines how quickly it can move across the game environment. This affects both the player character (PC) and enemies. |
| Acceptance Criterion | Speed should be accurately reflected in the movement rate of entities in the game. |
| Notes | Speed may be modified by certain augments or dungeon curses. |

| **ID: 2**| **Entity property: Invincibility Time** |
| --- | --- |
| Description | After taking damage, an entity becomes invincible for a short duration during which it cannot take further damage. This is to prevent rapid successive damage from multiple sources. |
| Acceptance Criterion | Invincibility time is correctly applied after each instance of damage. |
| Notes | The duration of invincibility should be short and consistent across all entities unless modified by specific augments or dungeon curses. |



#### 2.2.1.2 Magic types 

There are three magic types in the game: Sun, Cosmic and Dark. Skills and enemies belonging to the different magic types are colour coded to allow differentiation by the player. The colours are Sun: yellow - red, Cosmic: blue - white, Dark: black - purple. The magic types work like in Rock, paper, scissors such that they have one other magic type that they are strong against and one against which they are weak. Their effectivity against themselves is in between. <br>
The magic types are:
- Sun, strong against Cosmic, weak against Dark
- Cosmic, strong against Dark, weak against Sun
- Dark, strong against Sun, weak against Cosmic


#### 2.2.1.3 Player character (PC)

The PC is the figure the player controls while playing the game. The PC is a wizard and should wear therefore wizard-like clothing, e.g., a pointy hat, a long robe and a staff.

##### 2.2.1.3.1 PC control

The PC is controled by the player via keyboard and mouse movements. The PC has four states which are described in the following. 

| **ID: 2**| **PC state: Idle** |
| --- | --- |
| Description | If the player does not enter any input for the PC, the PC remains idle and does not move. An idle animation is shown. |
| Acceptance Criterion | PC is in idle state if no input is given. |
| Notes | None |

| **ID: 2**| **PC state: Walking** |
| --- | --- |
| Description | If the player uses the WASD keys, the PC moves as long as the keys are pressed. The PC can walk in 8 directions: left, right, top and bottom and the diagonal directions inbetween. A specific walking animation for each direction is shown. |
| Acceptance Criterion | PC walks correctly if WASD is given as input. |
| Notes | Diagonal walking is achieved by pressing two keys at once and should not be faster than straight walking. |

| **ID: 2**| **PC state: Dashing** |
| --- | --- |
| Description | If the player presses the space bar, the PC dashes in the direction of the current mouse position. TODO: elaborate here. How does dashing work exactly? |
| Acceptance Criterion | PC dashes in the correct direction when the space bar is pressed. |
| Notes | None |

| **ID: 2**| **PC state: SpellCasting** |
| --- | --- |
| Description | If the player presses one of the keys 1,2 or 3, the PC casts a spell/uses a PC skill and a spell casting animation is shown. All PC skills that require a position or direction to be cast take the current mouse position for the position or direction.  |
| Acceptance Criterion | PC is in SpellCasting state if inout 1,2 and 3 is given. |
| Notes | Only one spell can be cast at a time. Input that is given while the player casts (spell cast animation plays) a spell is (TODO!) buffered and executed after the animation is done./disregarded by the system. <br> All skills use the same spell cast animation. Cast times should generally be short (less than 1 second). Slower cast times are achieved by playing the spell cast animation slower. |


##### 2.2.1.3.2 PC progression

While playing the game, the player can unlock new skills. The player also unlocks additional augment slots to equip up to 5 augments for additional effects when casting PC skills or increased stats. 

###### 2.2.1.3.2.1 PC skills

Every PC skill belongs to one magic type and does damage of that magic type. 
Each magic type has three different skills: a base skill, a supportive skill and an offensive skill.
All nine skills have upgraded versions which are automiatically unlocked if the player fulfills a predefined criteria. 
The PC can have up to three different skills equipped.  


| **ID: 1**| **Skill: Base Skills** |
| --- | --- |
| Description | Each element has a base skill that consist of a colored circular projectile shot from PC in the direction of the mouse. |
| Acceptance Criterion | Has to be implemented |
| Notes | None |

| **ID: 1**| **Skill: Sun Beam** |
| --- | --- |
| Description | The supportive skill of the sun element. The PC emits a ray of ligth from the PC in the direction of the mouse. <br> Enemies hit deal reduced damage and have reduced armor. |
| Acceptance Criterion | Has to be implemented |
| Notes | None |

| **ID: 1**| **Skill: Summon Sun** |
| --- | --- |
| Description | PC spawns a sun at mouse location for a few seconds. Enemies close to it take damage depending on how close they are to the sun. The center of the sun deals the most damage. Enemies take damage at predefined intervals as long as they are inside the AOE.|
| Acceptance Criterion | Has to be implemented |
| Notes | None |

| **ID: 1**| **Skill: Moon Light** |
| --- | --- |
| Description | A Ray of moonlight shines down on the player increasing the attack value of all their equipped skills and the armor values for all magic types. |
| Acceptance Criterion | Has to be implemented |
| Notes | None |

| **ID: 1**| **Skill: Star Rain** |
| --- | --- |
| Description | Multiple single projectiles spawn around the PC with random offset and start homing to mouse position. <br> On collision with enemy they do damage and despawn. |
| Acceptance Criterion | Has to be implemented |
| Notes | None |

| **ID: 1**| **Skill: Dark Energy Wave** |
| --- | --- |
| Description | PC creates a black wave that pushes all enemies away from the PC. The wave pushes away all enemies in the same room as the PC, independently of the distance to the PC. |
| Acceptance Criterion | Has to be implemented |
| Notes | None |

| **ID: 1**| **Skill: Black Hole** |
| --- | --- |
| Description | PC creates a round black void at mouse position that pulls all enemies towards it, if they hit the black void they take massive damage. |
| Acceptance Criterion | Has to be implemented |
| Notes | None |


###### 2.2.1.3.2.2 Augments 

Instead of a traditional armor and weapon system, the game uses augments to enhance the PC by giving additional effects and stats. 
There are a  total of 5 augment slots which can be unlocked. Every augment can be equipped to every unlocked augment slot. 
This will allow easy build crafting for the player.  <br>

| **ID: 1**| **Augments: Equipping augments** |
| --- | --- |
| Description | The player can equip augments to their unlocked augment slots. Every augment can only be equipped to one slot at a time. The augment effects are then applied to the PC. |
| Acceptance Criterion | Augments can be equipped and the effects are applied to the PC correctly. |
| Notes | None |

| **ID: 1**| **Augments: Unlocking augment slots** |
| --- | --- |
| Description | When the player completes the intro dungeon, the first augment slot is unlocked. <br> When the player clears each further story dungeon, one additional augment slot is unlocked. There are a total of 5 augment slots maximally. |
| Acceptance Criterion | Augment slots are unlocked correctly. |
| Notes | None |

| **ID: 1**| **Augments: Obtaining augments** |
| --- | --- |
| Description | When the PC kills an enemy there is a chance to obtain an augment. Slimes have a low chance of dropping augments and are more likely to drop low quality augments. Bosses are guaranteed to drop augments and can drop several augments according to a distribution. Augments dropped by bosses also have a higher chance to be high quality augments. If an enemy drops an augment, the PC obtains the augment automatically. |
| Acceptance Criterion | Enemies drop augments with the correct chances and the PC obtains the augments when they are dropped. |
| Notes | Every enemy has a chance bigger than zero to drop every possible existing augment. |


Each augment will have 1, 2 or 3 effects. Augments can the same effect several times in which case the effect will stack. The amount of effects determines the quality of the augment. The effects are decided randomly from the list of possible effects by the game when the augment is dropped. The effects will have percentage values. This means that effects of the same type will stack multiplicative.   

To allow build crafting for the player, it is possible to destroy one augment and to move one of its effects onto another augment, overwriting one of its previous effects. This is described in more detail in chapter 2.2.2.1.3 Fusing augments.


##### List of augment effects

The effects of the augments are the following: 

- 10 additional armor of a type
- 5 additional armor of all types
- 10% more hp
- 10% extra damage for one skill (one for each damaging ability)
- 5% extra damage for one magic type
- 1% life steal
- 10% bigger radius for one skill (exists for “summon sun”, “black hole”)
- 10% more stars for “star rain”
- Upon casting spell x also cast spell y (specific spells will be determined during balancing)
- Spell x explodes on impact with enemy (for directional skills only, means damage is applied to all enemies in an AOE)
- Plus 10 attack for all spells of one element (this way supportive spells can also deal damage)
- 50% longer duration for skills that remain on field (“summon sun” and/or “black hole”) 
- Plus 20 attack for skill in slot 1/2/3 

Values and effects might have to be changed, added or removed for good balancing later.


#### 2.2.1.4 Enemies

Each Enemy only deals damage of one magic type. It will have a high armor against the damage type that it is strong against and a low armor against the type that it should be weak against. Its armor value against its own magic type is in between. 
Use color coding to signal the magic type of the enemy to the player.

There are two types of enemies:
1. Slimes (small and big, melee and ranged)
2. Unicorn bosses

Enemies are controlled via their state machine.
Most will deal only melee damage. Hence they will track the player and once they are close attack him.
Slimes will come in large groups. Use Group behaviors to simulate better movement (not all of them stand on top of each other, but instead keep distance).


##### 2.2.1.4.1 Slimes

Every slimes belongs to one magic type. They are colored according to this magic type, only deal damage aligned to this magic type, have high armor against the magic type their magic type is strong against and low armor against the magic type which their magic type is weak against.  

Slimes can have three possible states.

| **ID: 1**| **Slimes states: Idle** |
| --- | --- |
| Description | If the PC is outside of the detection range of the slime, the slime is idle. It moves around randomly. |
| Acceptance Criterion | Has to be implemented |
| Notes | None |

| **ID: 1**| **Slimes states: Moving** |
| --- | --- |
| Description | If the PC is inside of the detection range of the slime but outside of the attack range of the slime, the slime moves towards the PC until it is in attack range. |
| Acceptance Criterion | Has to be implemented |
| Notes | None |

| **ID: 1**| **Slimes states: Attacking** |
| --- | --- |
| Description | If the PC is inside of the attack range of the slime, the slime attacks the PC. |
| Acceptance Criterion | Has to be implemented |
| Notes | None |


Slimes can differ in two attributes. There are large and small slimes and there are melee and ranged slimes, making a total of 4 different slime types. 
Large slimes have more HP and higher attack values than small slimes. Large slimes are rarer than small slimes and only appear in small group of up to 3 large slimes. They are often accompanied by several small slimes. 
Ranged slimes have a larger attack range and have a different color brightness than melee slimes. 
The armor values of all slimes of one magic type are the same.

| **ID: 1**| **Slime types: Melee Slime** |
| --- | --- |
| Description | Melee slimes move towards the PC and once they are very close to the PC, they jump against the PC for their attack. <br> Melee slimes can be large or small. |
| Acceptance Criterion | Has to be implemented |
| Notes | None |

| **ID: 1**| **Slime types: Ranged Slime** |
| --- | --- |
| Description | Ranged slimes have a larger attack radius. When attacking, they shoot a small projectile in the direction of the PC. <br> Ranged slimes can be large or small. |
| Acceptance Criterion | Has to be implemented |
| Notes | None |


##### 2.2.1.4.2 Bosses

The unicorn is the boss monster of the dungeons. It looks like a unicorn but is colored according to its magic type. <br>
Unicorns have no attack and detection range since they can detect and attack the player from every position in the room. Unicorns have three different attacks. Unicorns have a melee attack radius. If the PC is inside of the melee attack radius, the unicorn uses the melee attack, otherwise it uses one of the ranged attacks at random.  <br>
In between two attacks the unicorn remains idle for a short while to allow the player to attack the unicorn with their skills.

| **ID: 1**| **Unicorn states: Charge attack** |
| --- | --- |
| Description | If the PC is outside of the melee radius of the unicorn, the unicorn can use a charged attack. The unicorn charges at the PC and deals a large amount of damage if the player is hit. |
| Acceptance Criterion | Has to be implemented |
| Notes | None |

| **ID: 1**| **Unicorn states: Shooting attack** |
| --- | --- |
| Description | If the PC is outside of the melee radius of the unicorn, the unicorn can shoot a set of projectiles at the PC. |
| Acceptance Criterion | Has to be implemented |
| Notes | None |

| **ID: 1**| **Unicorn states: Stomping attack** |
| --- | --- |
| Description | If the PC is inside of the melee radius of the unicorn, the unicorn uses a stomping attack. The unicorn stomps on the ground before it and deals damage in an AOE. |
| Acceptance Criterion | Has to be implemented |
| Notes | None |

| **ID: 1**| **Unicorn states: Wait** |
| --- | --- |
| Description | After every attack, the unicorn remains idle for a short while in which it only moves around slowly. |
| Acceptance Criterion | Has to be implemented |
| Notes | None |


### 2.2.2 Areas

#### 2.2.2.1 Main Hub 

The first area type is the main hub which is a menu that allows the player to modify their load-out. In this area no PC exists that can be moved. Instead there is a point-and-click visualisation of the magic school. Clicking on specific objects in the image opens the different menus needed to modify the load-out. 

##### 2.2.2.1.1 Skill tree

The skill tree is a menu where the player can unlock new skills and read the effects of the different skills. 
Each magic type has its own small skill tree.  
The base skill of each magic type is the basis of the skill tree and has to be unlocked before other skills of that magic type can be unlocked. 
Skill points for each tree are earned by clearing dungeons of that type.  
The player chooses an element at the beginning of a playthrough and unlocks with it the basic spell of that element for the intro dungeon. After completing the intro dungeon, the player automatically unlocks the basic spell of the element that his chosen element is strong against (e.g., player chooses sun and receives the basic cosmic spell after the intro dungeon).  
Each Element has 3 spells. (Damage values shall be determined during development to allow good balance)

- One basic spell that just shoots a colored projectile (this is the first skill of every tree).
- One supportive spell
    - Sun: sunbeam (attack and defense debuff in a straight line with small diameter)
    - Cosmic: Moon light (attack and defense buff)
    - Dark: dark energy wave (pushes enemies away from player)
- One offensive spell
    - Sun: summon sun (aoe damage on location)
    - Cosmic: Star rain (multiple stars erupt from staff and move towards target location. damaging the first enemy they collide with)
    - Dark: black hole (aoe pulls enemies in)

All skills have hidden upgrades that improve the skill and are only shown in the skill tree after the player unlocks them by fulfilling a certain condition, e.g., clearing a dungeon with only one spell unlocks an improved version of that spell.
Further passive Skills (like damage increase or range increase) can be added as well, if there is time.

##### 2.2.2.1.2 Equipping

##### 2.2.2.1.3 Fusing augments

##### 2.2.2.1.4 Saving the game 

##### 2.2.2.1.5 Entering a dungeon

#### 2.2.2.2 Dungeons 

Camera follows the player TODO

We want 5 handcrafted story dungeons plus a handcrafted intro dungeon. Additional dungeons will be procedurally generated.  
Dungeons are composed of a set of rooms which are connected through doors. Each room is an instance. In the final room of each dungeon is a boss.  
Dungeons are not linear. Instead the player is forced to find the boss room. However, only the boss has to be killed in order to clear a dungeon. Not every single room.  

The the rooms out of which the dungeon is created are handcrafted and not randomly generated. There should be at least 5 different rooms.  
The spawn locations for the enemies are also determined by hand. However not all possible spawn locations must also spawn enemies.  
Since there are curses that spawn additional monsters each room, not all spawn locations must spawn an enemy.
Instead whenever the room is first initialised the game will determine how many enemies should be spawned and then randomly pick the locations.

Each room is its own instance.
The player can go into another room by walking through a door of the room. 
This will then load the next room instance. Direction is preserved, meaning if the player goes through the door on the left, the player will come out the door on the right in the next room, and vice versa for the other four directions.
The door leading to the boss room should be marked so that the player knows that by going through the door they will have to defeat the boss.

The first time the player enters a room, a bunch of slimes are spawned. The player can only exit the room after killing all enemies within it.  
The player can also return to a room they have already been to and cleared.
In that case no enemies are spawned and the player can immideately leave the room through any door.
Dungeons can be left early. When dying or leaving a dungeon voluntarily all previous collected items remain in the player’s inventory. The dungeon then has to be started again from the beginning.

##### 2.2.2.2.1 Intro dungeon

A shorter dungeon that serves as a tutorial for the game. Before entering the dungeon, the player chooses the element with which they want to start. The slimes inside the dungeon will change their element according to the players choice to teach the player about the strengths and weaknesses of their element.  
At the end of the dungeon the boss is a large slime. All slimes before are small.

##### 2.2.2.2.2 Story dungeon

There are 5 story dungeons.  
The bosses of the story dungeons 1 to 4 are unicorns. The last dungeon has a specially designed boss.

##### 2.2.2.2.3 Generated dungeons

At the start of the generated dungeon, the player can choose which element the dungeon should have. This choice determines the element of the final boss and guarantees that at least 50% of all slimes in the dungeon are of this element.  

##### 2.2.2.2.3.1 Generation

##### 2.2.2.2.3.2 Rewards

##### 2.2.2.2.3.3 Curses 

Dungeons can be cursed before starting the dungeon. The dungeon will then be harder by strengthening enemies or weakening the player.  
Upon dying the player has the choice to either retry the previously generated dungeon or to create a new one.  
If the player decides to leave the dungeon, they cannot try the same dungeon again. Instead, a new one will be generated.  
The number of rooms in the dungeon is decided randomly from a distribution.  

The curses of a cursed dungeon are randomly picked from a list and shown to the player before entering.  
The player can reroll the curses up to 2 times.  
If the player clears any generated dungeon, they get all rerolls back.

The dungeon layout is created using the possible rooms when the player first enteres the dungeon.  
The layout is generated in a grid like pattern, where each room is one cell.  
There should be at least 2 room between the entrance of the dungeon and the boss room.  
Furthermore there should not be more then 10 rooms in total.


##### Possible Curses

- player cannot use skill 3
- player can only use skill 1
- player takes x % more damage
- monsters have 20/40/60/80/100 % more hp/damage
- x % more monsters
- dungeon contains 2 bosses in the final room

### 2.2.3 Tile-Based System and Collision Detection

The game utilizes a tile-based system for both the macro-scale dungeon layouts connecting the different rooms, as well as for the in-room environment. Each room in the dungeon corresponds to a tile in a grid, ensuring structured navigation and interaction within the environment. This system facilitates the procedural generation of dungeons and supports a variety of room configurations.

### 2.2.3.1 Macro-scale dungeon layouts

- 

### 2.2.3.2 In-room environment



- set of predefined rooms, consisting of
  - tiles
  - enemy spawn points
  - entry points
  - exit points

### 2.2.3.3 Collision Detection

Collision detection is integral to the gameplay, ensuring that players and enemies interact with the environment and each other in a predictable manner. The game engine checks for collisions between entities (player, enemies) and environmental obstacles (walls, doors) to determine valid movements and interactions. This system is crucial for implementing gameplay mechanics such as combat, movement restrictions, and accessing different areas within the dungeons.

### 2.2.3.3.1 Player-Enemy Collisions

### 2.2.3.3.2 Player-Wall Collisions



### 2.2.3.3.3 Enemy-Enemy Collisions


## 2.3. Nonfunctional requirements

Use component based architecture if possible.

Desired FPS: 60 TODO Delete this

Please include the coding style here, as well as the performance as the frame rate that is to be achieved % the requirements are written in such a way that they can also be checked - later this should also be checked explicitly in the results

Coding Style: Adhere to the [GDScript style guide](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_styleguide.html) and use a component-based architecture for improved maintainability, code reuse, and easier future expansion.

### 2.3.1. User interface and human factors

Outside the dungeons, the user interface will be fully controllable via mouse and keyboard for menu navigation, spell/augment selection, skill point allocation, and dungeon selection.

Furthermore every menu has to be slectable with not more then 3 clicks.  
And each action, like selecting a dungeon or merging two augments, has to be doable with less then 10.

TODO delte rest here:
Outside the dungeons the user can use the ui to choose their spells and augments and spent skill points. Furthermore, they can select a dungeon they want to enter.
User interface is controlled via mouse and keyboard.

### 2.3.2. Documentation

Every non-trivial function within the codebase will have clear comments explaining its purpose, parameters, and return values.

A comprehensive user manual will be developed alongside the game to aid players.

TODO REMOVE
Every nontrivial function must have a comment above that explains the usage of this function.  
Plus external documentation.

### 2.3.3. Hardware considerations

The game is intended for personal computers that meet the recomended system requirements to run the Godot 4 game engine effectively.  

 - **CPU**: x86_64 CPU with SSE4.2 instructions, with 4 physical cores or more
 - **GPU**: Dedicated graphics with full OpenGL 4.6 support or full Vulkan 1.2 support
 - **RAM**: 8GB
 - **OS**: Latest version of Firefox, Chrome, Edge, Safari, Opera or Windows 10 for native export

### 2.3.4. Performance characteristics

The game should maintain an average frame rate of at least 55 FPS at 1080p resolution with no more than 10% dips below 60 FPS during gameplay.

### 2.3.5. Error handling and extreme conditions

All methods that can potentially encounter errors (e.g., missing file, invalid user input) will be explicitly marked with error handling code.

Errors will be communicated to the player through clear on-screen messages. Additionally, a log file will record all encountered errors for debugging purposes.

TODO REMOVE
In order to avoid problems, every method that can run into an error or return an error value has to be explicitly marked and then whenever such a function it’s called the error has to be handled immediately.

### 2.3.6. Quality issues

The development process will involve thorough quality checks, including functionality testing, bug fixing, and performance optimization to ensure a polished gaming experience.

### 2.3.7. System modifications

No additional software installations will be required beyond the Godot 4 game engine itself.

### 2.3.8. Physical environment

The game will run inside a modern browser supporting html5, javascript and webGL

### 2.3.9. Security issues

Due to its single-player nature, the game will not implement any online features or user accounts, eliminating security concerns related to player data.

The game will be restricted to reading and writing data within its designated folder, ensuring no security risks to the player's computer.


TODO REMVOVE:
Since the game will be a single player game hacking and accessing game data is irrelevant. If the player wants to cheat or change something in the game, they are free to do so as it does not affect anyone else.  
Furthermore, the game should also only be allowed to read and write in the designated folder for the game and does not pose a security risk to the player’s computer.

### 2.3.10. Resource issues

The game's memory usage and disk space requirements will be optimized to ensure smooth performance on a wide range of PC hardware. Specific limitations will be determined during development based on testing.

## 2.4. Pseudo requirements

The game should minimize visual clutter. 
There is no need unneccessary visual effects if they do not add much to the enjoiment of the player.

The game should also not contain large empty spaces devoid of any form of player interaction.

## 2.5. System models


### 2.5.1. Scenarios

1. **Scenario** The player starts a new game. They are then asked to choose an element with wich they want to start.
2. **Scenario** The PC enters a room in a dungeon. Close enemies then target the PC, forcing them to figth their way trough


### 2.5.2. Use case model

 - **Use case:** Player unlocks a new skill
    - **Precondition:** The PC has a skill point to spend in an element and an unlearned skill in that element
    - **Steps:** 
      1. The player selects the skils menu
      2. The player selects the skill they want to unlock
      3. They click unlock
    - **Postcondition:** The PC has now learned the skill and can select it

### 2.5.3. Object model


#### 2.5.3.1. Data dictionary


#### 2.5.3.2. Class diagrams


### 2.5.4. Dynamic models


### 2.5.5. User-interface -- navigational paths and screen mock-ups


# 3. Glossary
