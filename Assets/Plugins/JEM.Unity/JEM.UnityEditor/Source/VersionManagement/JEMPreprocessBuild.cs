//
// Unity Editor Just Enough Methods Library Source
//
// Copyright (c) 2017-2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace JEM.UnityEditor.VersionManagement
{
    internal class JEMPreprocessBuild : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;
        public void OnPreprocessBuild(BuildReport report)
        {
            // before build starts, increase number of build...
            JEMBuildWindow.IncreaseBuildNumber();
        }
    }
}
