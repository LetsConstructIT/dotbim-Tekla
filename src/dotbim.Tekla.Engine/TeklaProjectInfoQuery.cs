using System;
using Tekla.Structures.Model;

namespace dotbim.Tekla.Engine;

public class TeklaProjectInfoQuery
{
    public string GetModelName()
    {
        var model = new Model();
        return model.GetInfo().ModelName;
    }
}