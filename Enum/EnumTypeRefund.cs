using System;

namespace BoaSaudeRefund.Enum
{
    [Flags]
    public enum EnumTypeRefund
    {
        Consulta = 1,
        Exame_Procedimento_Terapia = 2,
        Internacao = 3
    }
}
