using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKSoftware.Localization.MultiLanguages.Tests
{
    public static class TestHelper
    {
        public static string GetSolutionPath()
        {
            string currentPath = AppDomain.CurrentDomain.BaseDirectory;
            int count = 0;
            const int maxDepth = 1000;

            while (count < maxDepth)
            {
                string[] files = Directory.GetFiles(currentPath, "*.sln");

                if (files.Any())
                {
                    return currentPath;
                }

                string? parentPath = Path.GetDirectoryName(currentPath);

                //We are at the root we did not find anything
                if (parentPath == null || parentPath == currentPath)
                    throw new DirectoryNotFoundException("Could not find solution path for " + AppDomain.CurrentDomain.BaseDirectory);

                currentPath = parentPath;
                count++;
            }

            throw new DirectoryNotFoundException("Reached Max Depth. Could not find solution path for " + AppDomain.CurrentDomain.BaseDirectory);
        }
    }
}
