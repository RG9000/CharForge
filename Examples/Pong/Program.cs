using CharForge;
using Pong;

var scene = new Scene();

scene
    .AddEntity(new Player1Paddle())
    .AddEntity(new Player2Paddle())
    .AddEntity(new Ball());

await scene.Activate();


