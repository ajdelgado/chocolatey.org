﻿using System;
using System.Collections.Generic;
using System.Linq;
using NuGet;

namespace NuGetGallery
{
    public class DependencySetsViewModel
    {
        public DependencySetsViewModel(IEnumerable<PackageDependency> packageDependencies)
        {
            DependencySets = new Dictionary<string, IEnumerable<DependencyViewModel>>();

            var dependencySets = packageDependencies
                .GroupBy(d => d.TargetFramework)
                .OrderBy(ds => ds.Key);

            OnlyHasAllFrameworks = dependencySets.Count() == 1 && dependencySets.First().Key == null;

            foreach (var dependencySet in dependencySets)
            {
                var targetFramework = dependencySet.Key == null ? "All Frameworks" : VersionUtility.ParseFrameworkName(dependencySet.Key).ToFriendlyName();
                DependencySets.Add(targetFramework, dependencySet.Select(d => d.Id == null ? null : new DependencyViewModel(d.Id, d.VersionSpec)));
            }
        }

        public IDictionary<string, IEnumerable<DependencyViewModel>> DependencySets { get; private set; }
        public bool OnlyHasAllFrameworks { get; private set; }

        public class DependencyViewModel
        {
            public DependencyViewModel(string id, string versionSpec)
            {
                Id = id;


                if (!String.IsNullOrEmpty(versionSpec))
                {
                    RawVersionSpec = versionSpec;
                    VersionSpec = VersionUtility.PrettyPrint(VersionUtility.ParseVersionSpec(versionSpec));
                }
            }

            public string Id { get; private set; }
            public string VersionSpec { get; private set; }
            public string RawVersionSpec { get; private set; }
        }
    }
}