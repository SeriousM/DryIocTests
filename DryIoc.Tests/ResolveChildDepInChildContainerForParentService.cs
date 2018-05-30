using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DryIoc.Tests
{
  [TestClass]
  public class ResolveChildDepInChildContainerForParentService
  {
    [TestMethod]
    public void Test()
    {
      var container = new Container();
      container.Register<A>();

      var childContainer = container.With(rules => rules.WithFactorySelector(Rules.SelectLastRegisteredFactory()));
      childContainer.Register<B>();

      childContainer.Resolve<A>();
    }

    public class A
    {
      public B B { get; set; }
    }

    public class B
    {
    }
  }
}