using System.Collections.Generic;
using Tekla.Structures.Model;

namespace dotbimTekla.Engine.Selectors;

public interface ITeklaSelector
{
    IEnumerable<ModelObject> Get();
}