using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Cet.Core.Expression
{
    public class XTreeXmlSerializer 
        : XTreeSerializerBase<XElement>
    {

        //TODO use the visitor pattern rather a fixed procedure
        public override XElement Serialize(XTreeNodeBase xtree)
        {
            var xelem = new XElement(
                xtree.Token.GetType().Name,
                xtree.GetChildren().Select(_ => this.Serialize(_))
                );

            if (xtree.Token.Data != null)
            {
                xelem.Add(new XAttribute("data", xtree.Token.Data));
            }

            var mp = xtree.Token as XTokenMatchParam;
            if (mp != null)
            {
                xelem.Add(new XAttribute("flags", mp.Flags));
            }
            return xelem;
        }

    }
}
