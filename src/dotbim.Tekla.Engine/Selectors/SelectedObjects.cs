using System.Collections.Generic;
using Tekla.Structures.Model;
using TSMUI = Tekla.Structures.Model.UI;

namespace dotbimTekla.Engine.Selectors;

public class SelectedObjects : ITeklaSelector
{
    public IEnumerable<ModelObject> Get()
    {
        var moe = new TSMUI.ModelObjectSelector().GetSelectedObjects();

        while (moe.MoveNext())
        {
            yield return moe.Current;
        }
    }
}
