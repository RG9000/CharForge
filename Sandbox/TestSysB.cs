using CharForge;

namespace Sandbox;

public class TestSysB : GameSystem
{
    public TestSysB():base(typeof(TestSysC)){
        
    }

    public override void OnUpdate()
    {
        TestSysC testSysC = GetDependentSystem<TestSysC>();
        testSysC.OnUpdate();

    }
}