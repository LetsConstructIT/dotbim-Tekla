using System.Collections.Generic;
using Tekla.Structures.Model;

namespace dotbim.Tekla.Engine.Selectors;

public interface ITeklaSelector
{
    IEnumerable<ModelObject> Get();
}