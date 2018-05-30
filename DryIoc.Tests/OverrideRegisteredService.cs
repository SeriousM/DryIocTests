using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DryIoc.Tests
{
  [TestClass]
  public class OverrideRegisteredService
  {
    [TestMethod]
    public void Test_WithContainerAndDelegate()
    {
      var container = new Container();
      container.Register<A>();
      container.Register<IDepOfA, DepOfA>();

      var childContainer = container.With(rules => rules.WithFactorySelector(Rules.SelectLastRegisteredFactory()));
      childContainer.RegisterDelegate<IDepOfA>(r => new TestDepOfA());

      var a = childContainer.Resolve<A>();
      Assert.IsNotNull(a);
      Assert.IsInstanceOfType(a.dep, typeof(TestDepOfA));
    }

    [TestMethod]
    public void Test_WithContainer()
    {
      var container = new Container();
      container.Register<A>();
      container.Register<IDepOfA, DepOfA>();

      var childContainer = container.With(rules => rules.WithFactorySelector(Rules.SelectLastRegisteredFactory()));
      childContainer.Register<IDepOfA, TestDepOfA>();

      var a = childContainer.Resolve<A>();
      Assert.IsNotNull(a);
      Assert.IsInstanceOfType(a.dep, typeof(TestDepOfA));
    }

    [TestMethod]
    public void Test_WithFacade()
    {
      var container = new Container();
      container.Register<A>();
      container.Register<IDepOfA, DepOfA>();

      var facade = container.CreateFacade();
      facade.Register<IDepOfA, TestDepOfA>(serviceKey: ContainerTools.FacadeKey);

      var a = facade.Resolve<A>();
      Assert.IsNotNull(a);
      Assert.IsInstanceOfType(a.dep, typeof(TestDepOfA));
    }

    public class TestDepOfA : IDepOfA
    {
    }

    public class DepOfA : IDepOfA
    {
    }

    public interface IDepOfA
    {
    }

    public class A
    {
      public readonly IDepOfA dep;

      public A(IDepOfA dep)
      {
        this.dep = dep;
      }
    }
  }
}