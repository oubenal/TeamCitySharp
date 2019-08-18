using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeamCitySharp.Connection;
using TeamCitySharp.DomainEntities;
using TeamCitySharp.Locators;

namespace TeamCitySharp.ActionTypes
{
  public class Tests : ITests
  {
    #region Attributes

    private ITeamCityCaller m_caller;

    #endregion

    #region Constructor

    internal Tests(ITeamCityCaller caller)
    {
      m_caller = caller;
    }

    #endregion

    #region Public Methods

    public TestOccurrences ByBuildLocator(BuildLocator locator)
    {
      return m_caller.Get<TestOccurrences>($"/testOccurrences?locator=build:({locator})");
    }

    public TestOccurrences ByProjectLocator(ProjectLocator locator)
    {
      return m_caller.Get<TestOccurrences>($"/testOccurrences?locator=currentlyFailing:true,affectedProject:({locator})");
    }

    public TestOccurrences ByTestLocator(TestLocator locator)
    {
      return m_caller.Get<TestOccurrences>($"/testOccurrences?locator=test:({locator})");
    }

    public List<TestOccurrences> All(BuildLocator locator)
    {
      var result = new List<TestOccurrences>() { ByBuildLocator(locator) };
      while (!(string.IsNullOrEmpty(result.Last().NextHref)))
      {
        var response = m_caller.Get<TestOccurrences>(result.Last().NextHref, false);
        result.Add(response);
      }
      return result;
    }

    public List<TestOccurrences> All(ProjectLocator locator)
    {
      var result = new List<TestOccurrences>() { ByProjectLocator(locator) };
      while (!(string.IsNullOrEmpty(result.Last().NextHref)))
      {
        var response = m_caller.Get<TestOccurrences>(result.Last().NextHref, false);
        result.Add(response);
      }
      return result;
    }

    public List<TestOccurrences> All(TestLocator locator)
    {
      var result = new List<TestOccurrences>() { ByTestLocator(locator) };
      while (!(string.IsNullOrEmpty(result.Last().NextHref)))
      {
        var response = m_caller.Get<TestOccurrences>(result.Last().NextHref, false);
        result.Add(response);
      }
      return result;
    }
    #endregion
  }
}
