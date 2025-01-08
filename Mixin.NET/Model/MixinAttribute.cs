using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mixin.NET.Model;

public class MixinAttribute : Attribute
{
    internal Type _classType;

    public MixinAttribute(Type classType) => _classType = classType;
}
