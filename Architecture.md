# Main Scenes

These are the main scenes of the architecture.

## Main Menu

- MainMenu
    - Picture
    - Buttons

The main menu shows the buttons for New Game, Continue, Settings and Exit.a

### Settings Menu

- SettingsMenu
    - Buttons

## Main Game

- Root
    - Camera
    - Player
    - DungeonHandler
    - RoomHandler
    - HomeScene/UI

Once the game is loaded (either by loading a save file or starting a new game) the game always has this scene layout. <br>
When the player is outside the dungeons, the DungeonHandler and the player are invisible and don't to anything, while the HomeScene/UI is the only thing visible. <br>
When inside a Dungeon, The HomeScene/UI is invisible and the Player and the DungeonHandler together with the dungeon as a child of the DungeonHandler are visible. <br>

This layout has the advantage that the player is constantly loaded, making sure that player data is always correct. <br>
We also do not need any scene transitions and loading screens, since the main thing we need to do is change visibility and add or remove the scene for the current room as a child of DungeonHandler.


# Sub Scenes

These are the scenes used in other scenes.

## Entities

#TODO: All entities need a death state, also add to requirements document

### Player 

- Player
    - AnimatedSprite2D
    - Inventory
    - StateMachine
        - Idle
        - Moving
        - Dashing
        - SpellCasting
        - Death
    - MovementComponent
    - HealthComponent

The Player scene contains the entire player with all their data.
The state machine of the player is implemented 

### Slime

- Slime
    - AnimatedSprite
    - StateMachine
        - Idle
        - Moving
        - Attacking
        - Death
    - MovementComponent
    - HealthComponent

Will need 4 different slime scenes for the 4 different types (melee, ranged and small, big), but each will be structured the same way

### Unicorn

- Unicorn
    - AnimatedSprite
    - StateMachine
        - Idle
        - StompingAttack
        - ChargeAttack
        - ShootingAttack
        - Death
    - MovementComponent
    - HealthComponent

## Dungeon Handler

- DungeonHandler
   

The dungeon handler generates the layout of the dungeon and loads/unloads the individual rooms whenever the player enters a new room.
It is also responsible to position the player at the correct door inside each room.

### Dungeon Room

 - Room
    - TileMap (three layers: background, middle and foreground)
    - RoomExit (one for each possible exit direction)
    - RoomEntrance (one for each possible enter direction)
    - EnemySpawn (multiple)
    - Entities (Slime1, Slime2)

When initialized the room will spawn monsters if it was loaded the first time, otherwise it does not create entities.


# MISC

## Components

These are Nodes that contain important functionality. 
They are designed in a way such that adding the node to an object adds the functionality to the object. 

For example a HealthComponent contains all necessary data for an object to have hp and take damage. <br>
Adding a HealthComponent to any entity such as a slime, the player but also a chest or a wall, makes it so the entity can take damage and be destroyed.

### HealthComponent

Member variables:
- armor_values
- max_hp
- current_hp

Member functions:
- take_damage()
- heal()

See [here](https://www.youtube.com/watch?v=74y6zWZfQKk) for an introduction.

### StateMachine

The state machine consists of one StateMachine manager node and multiple nodes that each are one state, for example the Idle state.

The StateMachine manager node knows which state is currently active and propagates all relevant calls (like process and physics_process) down from the player. <br>
The individual state nodes, like Idle, implement the behaviour of the state.

This implementation has the advantage that we do write a large if else file with the logic of all states but instead keep the states completely seperate.
This also has the advantage that adding new states or adding existing states to an entity only requires adding the node and changing the other skripts to transition to the new state when required.

A good explenation can be found in these two videos: <br>
[Here](https://www.youtube.com/watch?v=oqFbZoA2lnU) <br>
[And here](https://www.youtube.com/watch?v=bNdFXooM1MQ)

Or [alternatively](https://www.youtube.com/watch?v=ow_Lum-Agbs)


### MovementComponent

TODO we could consider scrapping this and just making different walking states for player and AI // I think we should do that (David) I agree with this (Katrin)

This component devines movement behavoiur of an entity. 
For the player this consist of getting the player input.
For enemies, this constits of an AI skript.

Having this as a separete component to the statemachine, allows us to have the same walking state for both player and enemies.

## Classes

These classes are used to encompass data and functionality

### Attack Class

This class is used as an input for the take damage functions.<br>
It contains:
- magic type
- damage value
- reference to who attacked
- potenitally stuff related to curses and augments

The advantage of making a class and not just passing the values themself is that it allows great extendability with regards to new functionality.


[Short introduction](https://www.youtube.com/watch?v=y3faMdIb2II&t=148s)

### Augment

Every augment contains a list of 1-3 augment effects.

### Augment Effect

Every individual augment effect is a class that derives of the base augment effect class. <br>
This base class defines functions that are called whenever the augment gets equiped or unequiped.

### Curse Effect

Works the same as augment effect.
