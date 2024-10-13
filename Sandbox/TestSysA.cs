using CharForge;

namespace Sandbox;

public class TestSysA : GameSystem
{
    public TestSysA():base(typeof(TestSysB)){
        
    }
    public override void OnUpdate()
    {
        TestSysB testSysB = GetDependentSystem<TestSysB>();
        TestSysC testSysC = testSysB.GetDependentSystem<TestSysC>();
        if (Owner?.Owner == null) return;
        if (Owner.Owner.CurrentKeyPressed == ConsoleKey.A)
        {
            Console.WriteLine("Hello from SysA! Value in SysC was " + testSysC.message );
        }
    }
}