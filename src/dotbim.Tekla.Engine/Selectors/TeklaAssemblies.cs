using System.Collections.Generic;
using Tekla.Structures.Model;

namespace dotbimTekla.Engine.Selectors;

public class TeklaAssemblies : ITeklaSelector
{
    public IEnumerable<ModelObject> Get()
    {
        var model = new Model();
        var moe = model.GetModelObjectSelector().GetAllObjectsWithType(ModelObject.ModelObjectEnum.ASSEMBLY);

        while (moe.MoveNext())
        {
            yield return moe.Current;
        }
    }
}
