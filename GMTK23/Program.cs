using SimulationFramework.Desktop;
using GMTK23;
using GMTK23.Scenes.GameplayScene;

var game = new GMTKGame(new GameplayScene());
game.Run(new DesktopPlatform());