# Process Changes

## Process Change table

| **Task number**| **Error type** | **Reason for Error** | **Process Change** | **Validation of changed process if error still occurs** |
| --- | --- | --- | --- | --- |
| Switching between dungeon rooms | Implementation led to crashes in some cases and was pushed to main. | A part of the game engine was misunderstood / it was unclear that special functions had to be used to implement the task correctly. The error did not appear everytime and was therefore not caught initially. | All pull requests have to be checked by a second person before being pushed to main. | Similar errors did not occur / were caught by the process change. |
| Implementation of player state machine | Tests had to be changed because implementing them according to how they were planned for the tests was not possible. | Godot is not designed for unit tests and the architecture for the game is not decided in full detail yet. | Basic structure of the code is implemented before writing the tests (not the full implementation). Furthermore, integration tests are used wherever it seems appropriate. | Error did not occur again. |