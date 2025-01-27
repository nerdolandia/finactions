# Regras para contribuição

Todos os commits devem seguir o modelo proposto pelo [Conventional Commits](https://www.conventionalcommits.org/pt-br/v1.0.0/), a leitura desse documento por tanto é obrigatória para a compreensão das determinações adicionais propostas neste documento.

As sessões abaixo descrevem as definições adicionais para itens relacionados ao tema.

### 3.1. Tipos aceitáveis para os títulos de commit

O tipo do commit conforme descrito no Conventional Commits é uma sigla que inicia o título do commit, cada sigla possuí um significado e deve ser usados para um fim especifico, abaixo estão listados os tipos aceitáveis que DEVEM ser usados nos textos de commit:

- CHORE: Alterações de tarefas de build, automações e pipelines
- DOCS: Adição ou alterações de documentações
- FEAT: Nova funcionalidade ou rotina
- FIX: Correções de bugs
- REFACT: Refatoração de código
- TEST: Novo teste automatizado
- TYPO: Corrige erros de digitação ou palavras escritas errado
- WIP: Alterações que ainda não foram terminadas
- MERGE: Realizado merge com alguma branch

Observação: não há diferença entre escrever o tipo do commit em maiúscula ou minúscula.

### 3.2. Lista de _issue_ no texto de commit

Os commits realizados em um produto deveriam sempre estar vinculados a pelo menos um _issue_, commits sem _issues_ NÃO PODERIAM ser realizados. O relacionamento de um commit com um _issue_ deve ser feito por meio da inclusão do número dos _issues_ no corpo do commit, sendo sempre necessário o sufixo `- #` para o relacionamento automático do commit com o _issue_.

Exemplo:

```commit
FIX(escopo): Texto do commit

- Inclusão de um corpo opcional
- #1234
- #1235
```