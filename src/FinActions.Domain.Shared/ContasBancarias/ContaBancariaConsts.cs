namespace FinActions.Domain.Shared.ContasBancarias
{
    public static class ContaBancariaConsts
    {
        public const int NomeMaxLength = 150;

        public const string ErroContaBancariaNaoEncontradaType = "ErroContaBancariaNaoEncontrada";
        public const string ErroContaBancariaNaoEncontrada = "Conta bancária não existe";
        public const string ErroContaBancariaNomeVazio = "O nome da conta bancária é obrigatório";
        public static readonly string ErroContaBancariaNomeTamanhoMax = $"O tamanho máximo do nome da conta bancária é de {NomeMaxLength}";
        public const string ErroContaBancariaSaldoInvalido = "O saldo informado é menor ou maior do que podemos armazenar";
    }
}
