using System;

namespace BoaSaudeRefund.Enum
{
    [Flags]
    public enum EnumStateRefund
    {
        Novo = 1,
        Em_Analise = 2,
        Aguardando_Pagamento = 3,
        Pagamento_Realizado = 4,
        Indeferido = 5,

    }
}
