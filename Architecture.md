# Main Scenes

These are the main scenes of the architecture.

## Main Menu

- MainMenu
    - Picture
    - Buttons

## Main Game

- Root
    - Player
    - DungeonHandler
    - HomeScene


# Sub Scenes

These are the scenes used in other scenes.

## Entities

### Player 

- Player
    - AnimatedSprite2D
    - Inventory
    - StateMachine
        - Idle
        - Moving
        - Dashing
        - SpellCasting
    - MovementComponent

### Slime

- SLime
    - AnimatedSprite
    - StateMachine
        - Idle
        - Moving
        - Attacking
    - MovementComponent

### Unicorn

- Unicorn
    - StateMachine
        - Idle
        - StompingAttack
        - ChargeAttack
        - ShootingAttack

## Dungeon Handler

- DungeonHandler
    - Room
        - Structure
        - Slime1
        - Slime2