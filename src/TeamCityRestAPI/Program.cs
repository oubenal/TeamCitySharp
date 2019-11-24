using System;
using System.Collections.Generic;
using System.Linq;
using TeamCitySharp;
using TeamCitySharp.DomainEntities;
using TeamCitySharp.Locators;

namespace TeamCityRestAPI
{
    using TestOccurrenceByProject = Dictionary<string, Dictionary<string, TestOccurrence>>;
    internal static class Program
    {
        private static void Main()
        {
            var inspectInvestigation = new InspectInvestigations();
            inspectInvestigation.Run();
        }
    }

    internal sealed class InspectInvestigations
    {
        private readonly ITeamCityClient teamcityClient;
        public InspectInvestigations()
        {
            teamcityClient = new TeamCityClient("teamcity.jetbrains.com", true);
            //
            teamcityClient.ConnectAsGuest();
        }
        private static bool HasTestTarget(Investigation investigation) => investigation.Target.Tests != null; // only get investigation with a test target
        private IEnumerable<Investigation> ExtractInvestigation()
        {
            return teamcityClient.Investigations.All().Where(HasTestTarget);
        }
        private List<TestOccurrence> GetTestOccurrences(Test test)
        {
            return teamcityClient.Tests.ByTestLocator(TestLocator.WithId(test.Id)).TestOccurrence;
        }
        private List<TestOccurrences> AllTestsOccurrences(Project project)
        {
            return teamcityClient.Tests.All(ProjectLocator.WithId(project.Id));
        }
        public void Run()
        {
            Console.WriteLine("#Analyze Investigations");
            var investigations = ExtractInvestigation();

            var lastTestOccurrenceByProject = new TestOccurrenceByProject(); // cache
            foreach (var investigation in investigations)
            {
                Console.WriteLine($@"##Analyze investigation 
id=""{investigation.Id}"" 
state=""{investigation.State}"" 
resolution:""{investigation.Resolution.Type}""");
                // init
                var targetTest = investigation.Target.Tests.Test.First();
                var projectScope = investigation.Scope.Project;
                var resolutionType = investigation.Resolution.Type;
                
                // filter
                if (investigation.State == "GIVEN_UP") continue;
                if (resolutionType == Resolution.Manually) continue;
                /// additionnal filter if needed
                
                // get current test occurence & update cache if necessary
                Dictionary<string, TestOccurrence> lastRunTests;
                if (!lastTestOccurrenceByProject.ContainsKey(projectScope.Id))
                {
                    lastRunTests = AllTestsOccurrences(projectScope).SelectMany(_ => _.TestOccurrence).ToDictionary(_ => _.Id, _ => _);
                    lastTestOccurrenceByProject.Add(projectScope.Id, lastRunTests);
                } else
                    lastRunTests = lastTestOccurrenceByProject[projectScope.Id];
                //
                var testOccurrences = GetTestOccurrences(targetTest);
                var toBeRemoved = testOccurrences.All(testOccurrence => !lastRunTests.ContainsKey(testOccurrence.Id));
                if (toBeRemoved)
                    Console.WriteLine($@"#Should remove investigation href=""{investigation.Href}""");
            }
        }
    }

    public static class Helper
    {
        public static bool HasTestTarget(this Investigation investigation) => investigation.Target.Tests != null; // only get investigation with a test target
    }
}
