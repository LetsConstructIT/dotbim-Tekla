using dotbimTekla.Engine.ValueObjects;

namespace dotbimTekla.Engine.Selectors;

public class TeklaSelectorFactory
{
    public ITeklaSelector Create(ExportMode exportMode)
    {
        return exportMode switch
        {
            ExportMode.Selection => new SelectedObjects(),
            _ => new TeklaAssemblies()
        };
    }
}
