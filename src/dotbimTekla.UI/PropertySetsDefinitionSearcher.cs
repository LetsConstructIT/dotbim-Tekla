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

                settings.AddRange(GetPropertSetFiles(additionalPSetsDir));
            }

            return settings.OrderBy(s => Path.GetFileName(s)).ToList();
        }

        private IEnumerable<string> GetPropertSetFiles(string additionalPSetsDir)
        {
            return Directory.EnumerateFiles(additionalPSetsDir, "*.xml");
        }
    }
}
