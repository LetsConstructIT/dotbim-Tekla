using System;
using System.Collections.Generic;
using Tekla.Structures.Model;

namespace dotbimTekla.Engine.Selectors;

public class TeklaParts : ITeklaSelector
{
    public IEnumerable<ModelObject> Get()
    {
        var model = new Model();
        var moe = model.GetModelObjectSelector().GetAllObjectsWithType(new Type[] { typeof(Part) });

        while (moe.MoveNext())
        {
            yield return moe.Current;
        }
    }
}
