namespace FinActions.Domain.Shared.Movimentacoes
{
    public static class MovimentacaoConsts
    {
        public const int DescricaoMaxLength = 300;

        public const string ErroMovimentacaoNaoEncontrada = "Movimentação não existe";
        public const string ErroDataMovimentacaoInvalida = "A data de movimentação é menor ou maior do que podemos armazenar";
        public const string ErroMovimentacaoDescricaoVazio = "A descrição da movimentação é obrigatória";
        public static readonly string ErroMovimentacaoNomeTamanhoMax = $"O tamanho máximo da descrição da movimentação é de {DescricaoMaxLength}";
        public const string ErroValorMovimentadoInvalido = "O valor movimentado informado é menor ou maior do que podemos armazenar";
    }
}
