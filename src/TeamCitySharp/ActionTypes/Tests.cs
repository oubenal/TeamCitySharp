﻿using System;
using System.Collections.Generic;
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

        public TestOccurrences ByTestId(string testId)
        {
            return m_caller.Get<TestOccurrences>($"/testOccurrences?locator=test:(id:{testId})");
        }

        #endregion
    }
}
