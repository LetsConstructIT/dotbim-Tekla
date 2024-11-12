using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Tekla.Structures;
using Tekla.Structures.Model;
using TSMUI = Tekla.Structures.Model.UI;

namespace dotbim.Tekla.Engine.TestHelpers
{
    public static class TeklaHelper
    {
        public static void UnselectAll()
        {
            var emptyList = new ArrayList();

            new TSMUI.ModelObjectSelector().Select(emptyList);
        }

        public static void Select(string identifier)
        {
            var modelObject = GetModelObject(identifier);

            var objectsToSelect = new ArrayList()
            {
                modelObject
            };

            new TSMUI.ModelObjectSelector().Select(objectsToSelect);
        }

        public static ModelObject GetModelObject(string identifier)
        {
            var id = new Identifier(identifier);
            return new Model().SelectModelObject(id);
        }

        public static Part GetPart(string identifier)
        {
            var id = new Identifier(identifier);
            return (Part)new Model().SelectModelObject(id);
        }
    }
}
