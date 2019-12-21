using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unianio.Genesis
{
    [Flags]
    internal enum GenClothes : byte
    {
        Panties = 1 << 0,
        Bra = 1 << 1,
        SandalLeft = 1 << 2,
        SandalRight = 1 << 3
    }
}
