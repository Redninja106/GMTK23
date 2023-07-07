using SimulationFramework.Desktop;
using GMTK23;
using GMTK23.Scenes.HelloWorld;

var game = new GMTKGame(new HelloWorldScene());
game.Run(new DesktopPlatform());