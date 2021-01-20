using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XNALib
{
    /// <summary>
    /// Replaces the IClonable (which is not compatible with the since 4.0 XBox)
    /// See also: http://blogs.msdn.com/b/brada/archive/2004/05/03/125427.aspx
    /// </summary>
    public interface IClone
    {
        object CloneShallow();
        object CloneDeep();
    }
}
