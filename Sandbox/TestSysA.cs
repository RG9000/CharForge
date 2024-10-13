using System.Buffers;
using CharForge;

public class TestSysA : GameSystem
{
    public TestSysA():base(typeof(TestSysB)){
        
    }
    public override void OnUpdate()
    {
        TestSysB testSysB = GetDependentSystem<TestSysB>();
        Console.WriteLine("Working");
    }
}