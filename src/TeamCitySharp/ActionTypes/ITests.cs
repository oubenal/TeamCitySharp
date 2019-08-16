using System;
using System.Collections.Generic;
using System.Text;
using TeamCitySharp.DomainEntities;
using TeamCitySharp.Locators;

namespace TeamCitySharp.ActionTypes
{
    public interface ITests
    {
        TestOccurrences ByBuildLocator(BuildLocator locator);
    }
}
