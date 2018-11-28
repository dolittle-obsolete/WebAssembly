/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.IO;
using Microsoft.DotNet.PlatformAbstractions;

public class Folders
    {
        private static string[] GetDefaultProbeDirectories() =>
            GetDefaultProbeDirectories(RuntimeEnvironment.OperatingSystemPlatform);

        internal static string[] GetDefaultProbeDirectories(Platform osPlatform)
        {
            var probeDirectories = AppDomain.CurrentDomain.GetData("PROBING_DIRECTORIES");
            var listOfDirectories = probeDirectories as string;

            if (!string.IsNullOrEmpty(listOfDirectories))
            {
                return listOfDirectories.Split(new char[] { Path.PathSeparator }, StringSplitOptions.RemoveEmptyEntries);
            }

            var packageDirectory = Environment.GetEnvironmentVariable("NUGET_PACKAGES");

            if (!string.IsNullOrEmpty(packageDirectory))
            {
                return new string[] { packageDirectory };
            }

            string basePath;
            if (osPlatform == Platform.Windows)
            {
                basePath = Environment.GetEnvironmentVariable("USERPROFILE");
            }
            else
            {
                basePath = Environment.GetEnvironmentVariable("HOME");
            }

            if (string.IsNullOrEmpty(basePath))
            {
                return new string[] { string.Empty };
            }

            return new string[] { Path.Combine(basePath, ".nuget", "packages") };

        }

    }
}