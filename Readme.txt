MEDEX is composed of the following files:
CombatSprites.cs
Game1.cs
Menu.cs
Projectile.cs
Shield.cs
ShipSprite.cs
Stars.cs

All sprites, fonts, and sound effects are located in Team-Project-Game-devlopment>Content.

The text file for saving is located here:
Team-project-Game-development>bin>Windows>86.MedExSave.txt

If relative paths are not working, the following solutions might help:
Use find and replace and replace "System.IO.Path.GetFullPath(@"..\MedExSave.txt")" with a direct path to the save file.
Clone the repository from Monogame.
If the above do not work, another option would be to pull the current version of the game from GitHub; this version should work since it runs on all the group's systems.