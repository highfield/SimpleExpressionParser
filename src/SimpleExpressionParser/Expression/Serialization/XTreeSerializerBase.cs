using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Cet.Core.Expression
{
    public abstract class XTreeSerializerBase<T>
    {
        public abstract T Serialize(XTreeNodeBase xtree);
    }
}
