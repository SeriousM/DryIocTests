using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DryIoc.Tests
{
  [TestClass]
  public class PopulateInstancePropertiesWithInstanceFactory
  {
    [TestMethod]
    public void Test()
    {
      var container = new Container();
      container.Register<A>();
      container.Register<ILogger>(Reuse.Transient, Made.Of(() => new Logger(Arg.Index<string>(0)), r => r.Parent.ImplementationType.Name));

      var a = new A();
      container.InjectPropertiesAndFields(a);

      Assert.IsNotNull(a);
      Assert.IsNotNull(a.LoggerInstance);
    }

    public interface ILogger
    {
      string RandomId { get; }
      string NameOfLogger { get; }
    }

    public class Logger : ILogger
    {
      public string NameOfLogger { get; }
      public string RandomId { get; } = Guid.NewGuid().ToString();

      public Logger(string nameOfLogger)
      {
        NameOfLogger = nameOfLogger;
      }
    }

    public class A
    {
      public ILogger LoggerInstance { get; set; }
    }
  }
}