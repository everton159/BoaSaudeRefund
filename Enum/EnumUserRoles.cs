using System;

namespace BoaSaudeRefund.Enum
{
    [Flags]
    public enum EnumUserRoles
    {
        Admin = 1,
        Associado = 2,
        Prestador = 3
    }
}
