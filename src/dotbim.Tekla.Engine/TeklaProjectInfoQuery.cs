using System;
using Tekla.Structures.Model;

namespace dotbimTekla.Engine;

public class TeklaProjectInfoQuery
{
    public string GetModelName()
    {
        var model = new Model();
        return model.GetInfo().ModelName;
    }
}