using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace dotbimTekla.UI
{
    public class PropertySetsDefinitionSearcher
    {
        public IReadOnlyList<string> GetFullSettingPaths()
        {
            var files = new Tekla.Structures.TeklaStructuresFiles();
            var paths = files.PropertyFileDirectories;

            var settings = new List<string>();
            foreach (var path in paths)
            {
                var additionalPSetsDir = Path.Combine(path, "AdditionalPSets");
                if (!Directory.Exists(additionalPSetsDir))
                    continue;

                settings.AddRange(GetPropertySetFiles(additionalPSetsDir));
            }

            return settings.OrderBy(s => Path.GetFileName(s)).ToList();
        }

        public string? FindSettingsPath(string settingsName)
        {
            foreach (var path in GetFullSettingPaths())
            {
                if (Path.GetFileNameWithoutExtension(path) == settingsName)
                    return path;
            }

            return null;
        }

        private IEnumerable<string> GetPropertySetFiles(string additionalPSetsDir)
        {
            return Directory.EnumerateFiles(additionalPSetsDir, "*.xml");
        }
    }
}
