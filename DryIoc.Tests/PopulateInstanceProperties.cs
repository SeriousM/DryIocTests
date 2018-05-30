using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DryIoc.Tests
{
  [TestClass]
  public class PopulateInstanceProperties
  {
    [TestMethod]
    public void Test()
    {
      var container = new Container();
      container.Register<B>();

      var a = new A();
      container.InjectPropertiesAndFields(a);

      Assert.IsNotNull(a);
      Assert.IsNotNull(a.B);
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