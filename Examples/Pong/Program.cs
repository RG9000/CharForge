using CharForge;
using CharForge.Systems;
using CharForge.Systems.Graphics;
using CharForge.Systems.Physics;
using Pong;

var scene = new Scene();

scene
    .AddEntity(new Entity()
        .AddSystem(new CameraSystem(20,20))
        .AddSystem(new PositionSystem(0,0)))
    .AddEntity(new Player1Paddle())
    .AddEntity(new Player2Paddle())
    .AddEntity(new Ball())
    .AddEntity(new Entity()
    //LEFT
        .AddSystem(new PositionSystem(0,0))
        .AddSystem(new CollisionSystem(1,25)))
    //RIGHT
    .AddEntity(new Entity()
        .AddSystem(new PositionSystem(33,0))
        .AddSystem(new CollisionSystem(1,25)))
    .AddEntity(new Entity()
    //BOTTOM
        .AddSystem(new PositionSystem(0,0))
        .AddSystem(new CollisionSystem(33,1)))
    //TOP
    .AddEntity(new Entity()
        .AddSystem(new PositionSystem(0,25))
        .AddSystem(new CollisionSystem(33,1)));

await scene.Activate();


