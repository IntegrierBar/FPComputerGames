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

PC: Player character <br>
HP: Hit Points, shows the current state of an entity (PC and enemies), are reduced when the entity is attacked. If the HP are reduced to 0, the entity dies. 

## 1.5. References


## 1.6. Overview


# 2. Proposed system


## 2.1. Overview

Player is a magician that explores dungeons. In these they fight slimes with 3 magic spells and maybe solve puzzles to get to the boss room, where they fight a boss.  
There are 2 types of dungeons. Automatically generated ones and story dungeons, that are hand crafted. Furthermore, the player can choose to curse a dungeon which increases the difficulty by strengthening the enemies or weakening the player. Cursed dungeons give better rewards.
By defeating the boss they can unlock new magic of the 3 different magic types.

Players also get “augments” these are used instead of armor. They have between 1 and 3 effects. These effects can be simple stat increases or additional effects (like casting one Skill when another is cast or increasing damaging area).
The player can wear a total of 5 augments at a time. Additional augments can be stored in an inventory. Slots are unlocked by playing story dungeons.

Players can only change their load-out (active augments and spells) outside of dungeons in the home town. This is done since the difficulty of generated dungeons is determined by the amount of augments the player is currently using.
The game is supposed to be finishable in under 2 hours. The idea is hence not to create a long game, but instead allow great replayability by having many different and interesting choices of playstyles and builds.

## 2.2. Functional requirements

| **ID: 1**| **Title: Damage Types** |
| --- | --- |
| Description | The game has 3 magic types. These are: Sun (red), Cosmic (purple) and Dark (black). <br> Sun magic deals more damage vs Cosmic enemies, Cosmic more vs Dark enemies and Dark more vs Sun enemies <br> (Rock, Paper, Scissors pattern) |
| Acceptance Criterion | Has to be implemented |
| Notes | None |


<!--- 
Sun, Cosmic, Dark. (Sun > Cosmic > Dark > Sun, like rock Paper scissors)
--->
Camera follows the player TODO

### 2.2.1 Components
TODO Delete Components Chapter?

| **ID: 2**| **Title: Health component** |
| --- | --- |
| Description | Healt component will handel HP and damage calculatations for a target. <br> Containes the current HP of the entitiy and its armor values. <br> Armor values are between 0 and 100 and determine the percentage that is absorbed by the armor when taking damage. <br> The health component also containes a "take_damage" function that is called, when the target takes damage. <br> This function takes damage type and value as input. <br> If the HP of a entity reach 0 it dies. |
| Acceptance Criterion | Has to be implemented |
| Notes | The damage calculation works like <br> health = health - damage*(100-armor)/100 |


#### Health component  
- Dictionary for armor values
   - armor is percent absorption. i.e. 40 armor means 40% of damage is absorbed
- current health (float)
- take damage function
    - takes damage type and value as input
    - health = health - damage*(100-armor)/100
    - if health is 0 or less, emit signal, that object dies



#### State machine  
TODO: DELETE?
A state is a subnode of the state machine. The state knows which state/node is currently active and redirects the process function to it.
Use a “changestate” function to change the state.
This state machine also handles animations.  
Possible states:
- Idle
- Walking
- Dashing
- SpellCasting

### 2.2.2 How damage is applied
TODO DELETE?
A Damaging ability creates an area2D with the damage type and the damage value.  
If the Area2D intersects something, it will check if it has a damage component and then call the “take damage” function.  
In order to prevent the entities to damage themselves, layers or groups can be used.

### 2.2.3 Player movement

| **ID: 1**| **Title: PC Movement** |
| --- | --- |
| Description | PC moves with wasd. PC can dash with spacebar. While dashing, the PC cannot get hit. <br> PC casts skills with 123. During casting, player cannot move or dash. |
| Acceptance Criterion | Has to be implemented |
| Notes | None |

Character moves with wasd.  
Character can dash by pressing spacebar. While dashing, the player has no hit box and thus cannot take damage.  
Player casts skills with 123. During casting, player cannot move, but cast times are small.  
All skills have the same player animation (player holds up staff), but if the cast time is slower, the animation plays slower. The spell comes out of the tip of the staff. Use color coding and different visuals to differentiate between the different spells.

### 2.2.4 Player Skills
The player can have up to 3 different skills selected.  
Each element has 3 different skills. 
A base skill, a supportive skill and an offensive skill.


| **ID: 1**| **Title: Base Skills** |
| --- | --- |
| Description | Each element has a base skill that consist of a colored circular projectile shot from PC in the direction of the mouse. |
| Acceptance Criterion | Has to be implemented |
| Notes | None |

| **ID: 1**| **Title: Sun Beam** |
| --- | --- |
| Description | The supportive skill of the sun element. The PC emits a ray of ligth from the PC in the direction of the mouse. <br> Enemies hit deal reduced damage and have reduced armor. |
| Acceptance Criterion | Has to be implemented |
| Notes | None |

| **ID: 1**| **Title: Summon Sun** |
| --- | --- |
| Description | PC spawns a sun at mouse location for a fiew seconds. Enemies close to it take damage depending on how close they are to the sun. |
| Acceptance Criterion | Has to be implemented |
| Notes | None |

| **ID: 1**| **Title: Moon Light** |
| --- | --- |
| Description | A Ray of moonlight shines down on the player increasing their attack and defenses. |
| Acceptance Criterion | Has to be implemented |
| Notes | None |

| **ID: 1**| **Title: Star Rain** |
| --- | --- |
| Description | Multiple single projectiles spawn around the PC with random offset and start homing to mouse position. <br> On collision with enemy they do damage and despawn. |
| Acceptance Criterion | Has to be implemented |
| Notes | None |

| **ID: 1**| **Title: Dark Energy Wave** |
| --- | --- |
| Description | PC creates a black wave that pushes all enemies away from the PC |
| Acceptance Criterion | Has to be implemented |
| Notes | None |

| **ID: 1**| **Title: Black Hole** |
| --- | --- |
| Description | PC creates a round black void at mouse position that pulls all enemies towards it, if they hit the black void they take massive damage. |
| Acceptance Criterion | Has to be implemented |
| Notes | None |



The player can choose up to three skills:
1. For all base skills, a color coded circular projectile is shot from the tip of the staff in the direction of the mouse.
2. Sun:
    1. Sunbeam: beam that debuffs (decrease attack and defence) all enemies in a straight line in one direction with a small diameter and long length, probably until the end of the screen.
    2. Summon sun: spawn a sun at mouse location for a few seconds, enemies close to it take damage depending on how close they are to the sun.
3. Cosmic:
    1. Moon light: ray of moonlight shines down on the player increasing their attack and defenses.
    2. Star rain: multiple single projectiles spawn around the player with random offset and start homing to mouse position. On collision with enemy they do damage and despawn.
4. Dark:
    1. Dark energy wave: pushes all enemies away from the player.
    2. Black hole: creates a round black void at mouse position that pulls all enemies towards it, if they hit the black void they take massive damage.

### 2.2.5 Enemies

Each Enemy only deals damage of one type. It will have high armor against this damage type and low armor against the type that it should be weak against.
Use color coding to signal the type to the player
Types of enemies:
1. Slimes (small and big, melee and ranged)
2. mini bosses like unicorns
3. Big bosses (probably no time for this) with special designs and attacks
Enemies are controlled via their state machine.
Most will deal only melee damage. Hence they will track the player and once they are close attack him.
Slimes will come in large groups. Use Group behaviors to simulate better movement (not all of them stand on top of each other, but instead keep distance).

#### 2.2.5.1 Slimes
Slimes are of one element. They are colored according to this element, only deal damage with this element, have high armor against this element and low armor against the element which is strong against their element.  
The difference between small and big slimes is the amount of HP they have and their Hitbox.

Slimes have a detection radius and an attack radius.  
If the player is inside the detection radius the slime moves towards them.  
If the player is inside the attack radius the slime attacks the player.

| **ID: 1**| **Title: Melee Slime** |
| --- | --- |
| Description | Melee slimes move towards the PC and once they are very close to the PC, they perform their attack. |
| Acceptance Criterion | Has to be implemented |
| Notes | None |

| **ID: 1**| **Title: Ranged Slime** |
| --- | --- |
| Description | Ranged slimes have a larger attack radius. When attacking, they shoot a small projectile towards the PC |
| Acceptance Criterion | Has to be implemented |
| Notes | None |


There are two different types of slimes, melee and ranged.
Melee slimes move towards the player and once they are close to the player, they start attacking with their element by blobbing onto them. 
Ranged slimes shoot small projectiles in the direction of the player. If the player is out of their range, they move closer to them. Different attack patterns can be implemented if there is time.
Melee and ranged slimes differ in color brightness. Slimes are color coded to the element they belong to.
Furthermore, there are small and large slimes, large slimes are rarer and have higher attack and hp values. Defence stats for all slimes are the same.


| **ID: 1**| **Title: Slime State Machine** |
| --- | --- |
| Description | Slimes have 3 states: <br> 1. **IDLE** If the PC is outside the  |
| Acceptance Criterion | Has to be implemented |
| Notes | None |


##### 2.2.5.1.1 State Machine

Slimes have two states:
1. **Moving**  If the player is outside the detection radius the slime randomly walks around slowly.  
If the player is inside the detection radius the slime moves towards the player.  
If the player is inside the attack range the slime switches to the attacking state.
2. **Attacking** In this state the slam goes through the attacking motion.
If it is a melee slime the slime will jump up aggressively and slam down damaging the player if they is to close.   
If it is a ranged slime it will shoot a small projectile towards the player.  
If the player leaves the attack radius the slime returns back to the moving state.


#### 2.2.5.2 Unicorns

Unicorns have different attack patterns:

- Charges at player, big damage if player is hit
- Shoots projectiles at player
- Mele attack by stomping on the player

If player is within a certain radius, only mele attack are used, otherwise one of the two ranged attacks is used at random.

### 2.2.6 Skill Tree

Each Element has its own small skill tree.  
Skill points for each tree are earned by clearing dungeons of that type.  
The player chooses an element at the beginning of a playthrough and unlocks with it the basic spell of that element for the intro dungeon. After completing the intro dungeon, the player automatically unlocks the basic spell of the element that his chosen element is weak against (e.g., player chooses sun and receives the basic dark spell after the intro dungeon).  
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

### 2.2.7 Augments

Instead of a traditional armor and weapon system, we only use augments.  
This means that there is no specific slot where an augment has to go. This will allow easy build crafting for the player.  
The player has a total of 5 augment slots.  
The first augment slot is unlocked after the intro dungeon.  
Additional slots are unlocked by playing the story dungeons.  
Augments can drop from regular enemies (low chance) and are guaranteed to drop from bosses.  
Each augment will have 1, 2 or 3 effects (in case they are the same effect, they will stack). The amount of effects determines the quality of the augment. The effects are decided randomly by the game when the augment is dropped.  
To allow build crafting for the player, it is possible to destroy one augment and to move one of its effects onto another augment, overwriting one of its previous effects.  
The most effects will have percentage values. This means that effects of the same type will stack multiplicative.  

#### 2.2.7.1 List of Effects

The effects of the augment are the following: (values are not fixed and can be changed to allow good balancing)

- 10 additional armor of a type
- 5 additional armor of all types
- 10% more hp
- 10% extra damage for one skill (one for each damaging ability)
- 5% extra damage for one element
- 1% life steal
- 10% bigger radius for one skill (exists for “summon sun”, “black hole”)
- 10% more stars for “star rain”
- Upon casting spell x also cast spell y (specific spells will be determined during balancing)
- Spell x explodes on impact with enemy (for directional skills only)
- Plus 10 attack for all spells of one element (this way supportive spells can also deal damage)
- 50% longer duration for skills that remain on field (“summon sun” and/or “black hole”) 
- Plus 20 attack for skill in slot 1/2/3 

More augment effects might have to be added or removed for balancing.

### 2.2.8 Dungeons

We want 5 handcrafted story dungeons plus a handcrafted intro dungeon. Additional dungeons will be procedurally generated.  
Dungeons are composed of a set of rooms which are connected through doors. Each room is an instance. In the final room of each dungeon is a boss.  
Dungeons are not linear. Instead the player is forced to find the boss room. However, only the boss has to be killed in order to clear a dungeon. Not every single room.  

The the rooms out of which the dungeon is created are handcrafted and not randomly generated. There should be at least 5 different rooms.  
The spawn locations for the enemies are also determined by hand. However not all possible spawn locations must also spawn enemies.  
Since there are curses that spawn additional monsters each room, not all spawn locations must spawn an enemy.
Instead whenever the room is first initialised the game will determine how many enemies should be spawned and then randomly pick the locations.

Each room is its own instance.
The player can go into another room by walking through a door of the room. 
This will then load the next room instance.  
The door leading to the boss room should be marked so that the player knows that by going through the door they will have to defeat the boss.

The first time the player enters a room, a bunch of slimes are spawned. The player can only exit the room after killing all enemies within it.  
The player can also return to a room they have already been to and cleared.
In that case no enemies are spawned and the player can immideately leave the room through any door.
Dungeons can be left early. When dying or leaving a dungeon voluntarily all previous collected items remain in the player’s inventory. The dungeon then has to be started again from the beginning.



#### 2.2.8.1 Intro Dungeon

A shorter dungeon that serves as a tutorial for the game. Before entering the dungeon, the player chooses the element with which they want to start. The slimes inside the dungeon will change their element according to the players choice to teach the player about the strengths and weaknesses of their element.  
At the end of the dungeon the boss is a large slime. All slimes before are small.

#### 2.2.8.2 Story Dungeons

There are 5 story dungeons.  
The bosses of the story dungeons 1 to 4 are unicorns. The last dungeon has a specially designed boss.

#### 2.2.8.3 Generated Dungeons

At the start of the generated dungeon, the player can choose which element the dungeon should have. This choice determines the element of the final boss and guarantees that at least 50% of all slimes in the dungeon are of this element.  
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

## 2.3. Nonfunctional requirements

Use component based architecture if possible.

Desired FPS: 60

Please include the coding style here, as well as the performance as the frame rate that is to be achieved % the requirements are written in such a way that they can also be checked - later this should also be checked explicitly in the results

### 2.3.1. User interface and human factors

Outside the dungeons the user can use the ui to choose their spells and augments and spent skill points. Furthermore, they can select a dungeon they want to enter.

User interface is controlled via mouse and keyboard.

### 2.3.2. Documentation

Every nontrivial function must have a comment above that explains the usage of this function.  
Plus external documentation.

### 2.3.3. Hardware considerations

For PCs that can run Godot 4 game engine.

### 2.3.4. Performance characteristics

60 FPS

### 2.3.5. Error handling and extreme conditions

In order to avoid problems, every method that can run into an error or return an error value has to be explicitly marked and then whenever such a function it’s called the error has to be handled immediately.

### 2.3.6. Quality issues

 Todo

### 2.3.7. System modifications

 Todo

### 2.3.8. Physical environment

Run on a computer inside a browser.

### 2.3.9. Security issues

Since the game will be a single player game hacking and accessing game data is irrelevant. If the player wants to cheat or change something in the game, they are free to do so as it does not affect anyone else.  
Furthermore, the game should also only be allowed to read and write in the designated folder for the game and does not pose a security risk to the player’s computer.

### 2.3.10. Resource issues

 Todo

## 2.4. Pseudo requirements


## 2.5. System models


### 2.5.1. Scenarios


### 2.5.2. Use case model


### 2.5.3. Object model


#### 2.5.3.1. Data dictionary


#### 2.5.3.2. Class diagrams


### 2.5.4. Dynamic models


### 2.5.5. User-interface -- navigational paths and screen mock-ups


# 3. Glossary
