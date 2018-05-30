using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DryIoc.Tests
{
  [TestClass]
  public class DisposeTests
  {
    [TestMethod]
    public void DisposeRootContainerDisposesAllInstances()
    {
      var rootContainer = new Container();
      var childContainer = rootContainer.With(rules => rules.WithFactorySelector(Rules.SelectLastRegisteredFactory()));
      var childScope = childContainer.OpenScope();
      
      Assert.IsFalse(childScope.IsDisposed);
      Assert.IsFalse(childContainer.IsDisposed);
      Assert.IsFalse(rootContainer.IsDisposed);

      rootContainer.Dispose();
      
      Assert.IsTrue(childScope.IsDisposed);
      Assert.IsTrue(childContainer.IsDisposed);
      Assert.IsTrue(rootContainer.IsDisposed);
    }

    [TestMethod]
    public void DisposeChildContainerDisposesAllInstances()
    {
      var rootContainer = new Container();
      var childContainer = rootContainer.With(rules => rules.WithFactorySelector(Rules.SelectLastRegisteredFactory()));
      var childScope = childContainer.OpenScope();
      
      Assert.IsFalse(childScope.IsDisposed);
      Assert.IsFalse(childContainer.IsDisposed);
      Assert.IsFalse(rootContainer.IsDisposed);

      childContainer.Dispose();
      
      Assert.IsTrue(childScope.IsDisposed);
      Assert.IsTrue(childContainer.IsDisposed);
      Assert.IsTrue(rootContainer.IsDisposed);
    }

    [TestMethod]
    public void DisposeCustomChildContainerDoesNotDisposesAllInstances()
    {
      var rootContainer = new Container();
      var childContainer = rootContainer.With(rootContainer.Rules.WithFactorySelector(Rules.SelectLastRegisteredFactory()), rootContainer.ScopeContext, RegistrySharing.CloneAndDropCache, null);
      var childScope = childContainer.OpenScope();
      
      Assert.IsFalse(childScope.IsDisposed);
      Assert.IsFalse(childContainer.IsDisposed);
      Assert.IsFalse(rootContainer.IsDisposed);

      childContainer.Dispose();
      
      Assert.IsTrue(childScope.IsDisposed);
      Assert.IsTrue(childContainer.IsDisposed);
      Assert.IsFalse(rootContainer.IsDisposed);
    }
  }
}
