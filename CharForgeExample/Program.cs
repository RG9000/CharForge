using CharForge;
using CharForge.Systems;
using CharForgeExample;

Console.WriteLine("Hello, World!");

Scene scene = new();

scene.AddObject(new GameObject("ball_1", x: 10, y: 1, true)
    .AddSystem(new SpriteRenderer(() =>
    {
        return ["/@@@\\",
                "@@1@@",
                "\\@@@/"
                ];
    }))
    .AddSystem(new Collider(5,3))
    .AddSystem(new RigidBody(0.7f, 0.1f))
    .AddSystem(new ConstantAcceleration(0, 0.0098f))
    .AddSystem(new Impulse(0.8f,0))
).AddObject(new GameObject("ball_2", x: 30, y: 1, true)
    .AddSystem(new SpriteRenderer(() =>
    {
        return ["/@@@\\",
                "@@2@@",
                "\\@@@/"
                ];
    }))
    .AddSystem(new Collider(5,3))
    .AddSystem(new RigidBody(0.7f, 0.1f))
    .AddSystem(new ConstantAcceleration(0, 0.0098f))
    .AddSystem(new Impulse(-0.8f,0))
).AddObject(new GameObject("floor", x: 1, y: 15, true)
    .AddSystem(new SpriteRenderer(() =>
    {
        return ["=================================================="
                ];
    }))
    .AddSystem(new Collider(50,1))
    .AddSystem(new StaticRigidBody())
).AddObject(new GameObject("lwall", x: 0, y: 0, true)
    .AddSystem(new SpriteRenderer(() =>
    {
        List<string> list = new List<string>(new string[15]);
        list = Enumerable.Repeat("x", 15).ToList();
        return list;
    }))
    .AddSystem(new Collider(1,15))
    .AddSystem(new StaticRigidBody())
).AddObject(new GameObject("rwall", x: 50, y: 0, true)
    .AddSystem(new SpriteRenderer(() =>
    {
        List<string> list = new List<string>(new string[15]);
        list = Enumerable.Repeat("x", 15).ToList();
        return list;
    }))
    .AddSystem(new Collider(1,15))
    .AddSystem(new StaticRigidBody())
);

await scene.Activate();