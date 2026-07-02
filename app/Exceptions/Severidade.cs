namespace gstok_api.Exceptions;

public enum Severidade
{
    Alerta, // Operação bloqueada por regra ou conflito — o usuário pode corrigir
    Erro    // Falha inesperada ou estado inválido — requer atenção técnica
}
