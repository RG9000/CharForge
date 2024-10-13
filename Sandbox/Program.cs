using CharForge;

Console.WriteLine("Hello, World!");

var scene = new Scene();

scene.AddEntity(new Entity().AddSystem(new TestSysA()).AddSystem(new TestSysB()));

await scene.Activate();

