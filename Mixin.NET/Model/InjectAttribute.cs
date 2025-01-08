using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mixin.NET.Model;

public class CallbackInfo
{
    public bool isCanceled { get; private set; } = false;
    public object retVal { get; private set; }

    public void SetReturnValue(object retVal)
    {
        this.retVal = retVal;
        isCanceled = true;
    }

    public void Cancel()
    {
        isCanceled = true;
    }
}

public enum InjectAt
{
    HEAD,
    FOOT
}

public class InjectAttribute : Attribute
{
    internal InjectAt _at;
    internal string _sourceMethodName;
    public InjectAttribute(string sourceMethodName, InjectAt at = InjectAt.HEAD)
    {
        this._at = at;
        this._sourceMethodName = sourceMethodName;
    }
}
