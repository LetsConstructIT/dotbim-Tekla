using dotbim.Tekla.Engine.ValueObjects;

namespace dotbim.Tekla.Engine.Selectors;

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
