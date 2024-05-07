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
HP: Hit Points, shows the current state of an entity (PC and enemies), are reduced when the entity is attacked. If the HP are reduced to 0, the entity dies. <br>
Augment: item that can be equipped by the player for additional stats or effects, dropped by enemies. <br>
Load-out: Equipped augments and magic skills that the player uses currently. Up to 3 magic skills and 5 augments can be equipped at a time. 
AOE: Area of effect, area in which a damaging skill effects enemies or the PC. 
**Player Character (PC)**: This term refers to the character controlled by the player within the game. The player's decisions and actions dictate the character's behavior and progression.<br/>
**Hit Points (HP)**: Represent the health or vitality of entities, such as player characters and enemies. Hit Points decrease when the entity takes damage. An entity is defeated or dies when its Hit Points reach zero.<br/>
**Augment**: A type of item that can be equipped by the player to gain additional statistics or effects. Augments are typically obtained as drops from defeated enemies.<br/>
**Load-out**: The combination of augments and magic skills a player currently equips. A player can equip up to five augments and three magic skills simultaneously.

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

| **ID: 2**| **Title: Speed** |
| --- | --- |
| Description | Every entity has a speed value that determines how quickly it can move across the game environment. This affects both the player character (PC) and enemies. |
| Acceptance Criterion | Speed should be accurately reflected in the movement rate of entities in the game. |
| Notes | Speed may be modified by certain augments or dungeon curses. |


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
| Description | Slimes have 3 states: <br> **IDLE** If the PC is outside the detection radius they randomly walk araound <br> **MOVING** If the PC is inside the detection radius but outside the attack radius the slime walks towards the PC <br> **ATTACKING** IF the PC is inside the attack radius the slime attacks the player |
| Acceptance Criterion | Has to be implemented |
| Notes | None |


##### State Machine

Slimes have two states:
1. **Moving**  If the player is outside the detection radius the slime randomly walks around slowly.  
If the player is inside the detection radius the slime moves towards the player.  
If the player is inside the attack range the slime switches to the attacking state.
2. **Attacking** In this state the slam goes through the attacking motion.
If it is a melee slime the slime will jump up aggressively and slam down damaging the player if they is to close.   
If it is a ranged slime it will shoot a small projectile towards the player.  
If the player leaves the attack radius the slime returns back to the moving state.


##### 2.2.1.4.2 Bosses

| **ID: 1**| **Title: Unicorns** |
| --- | --- |
| Description | The unicorn is the boss monster of the dungeons. It looks like a unicorn but is colored according to its element. <br> The unicorn has 3 different attacks that are chosen depending on the distance to the PC and by chance. <br> &nbsp; 1. Melee attack by stomping the ground before it if the PC is there. <br> &nbsp; 2. Charging towards the PC if they are further away. <br> &nbsp; 3. Shoot a beam from the horn towards the player  |
| Acceptance Criterion | Has to be implemented |
| Notes | None |


Unicorns have different attack patterns:

- Charges at player, big damage if player is hit
- Shoots projectiles at player
- Mele attack by stomping on the player

If player is within a certain radius, only mele attack are used, otherwise one of the two ranged attacks is used at random.

### 2.2.2 Areas

#### 2.2.2.1 Main Hub 

##### 2.2.2.1.1 Skill tree

TODO: ADD TO PLAYER SKILLS FURTHER ABOVE

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
This will then load the next room instance.  
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
