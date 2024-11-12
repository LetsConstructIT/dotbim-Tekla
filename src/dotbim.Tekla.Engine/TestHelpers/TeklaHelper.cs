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
            var id = new Identifier(identifier);
            var modelObject = new Model().SelectModelObject(id);

            var objectsToSelect = new ArrayList()
            {
                modelObject
            };

            new TSMUI.ModelObjectSelector().Select(objectsToSelect);
        }
    }
}
