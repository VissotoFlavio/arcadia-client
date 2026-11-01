# Arcadia Client — Regras do repositório

## Escopo

Este repositório contém exclusivamente o cliente Unity do Arcadia. A API, banco de dados e documentação canônica vivem em repositórios próprios; consulte `../STRUCTURE.md` na área de trabalho quando ela estiver disponível.

## Git e GitHub

- Use `gh` exclusivamente em operações do GitHub e valide `gh auth status` antes delas.
- Toda entrega deve estar associada a uma issue.
- Antes de abrir um Pull Request, revise o diff, execute as validações relevantes e atualize o changelog se o projeto vier a adotar um.
- PRs de issue devem incluir `Closes #<numero>` na descrição. Merge e publicação exigem autorização explícita.

## Branches

| Tipo | Formato | Origem | Destino |
|---|---|---|---|
| Funcionalidade | `feature/<issue-id>-<slug>` | `develop` | `develop` |
| Release | `release/<versao>` | `develop` | `master` |
| Correção | `hotfix/<issue-id>-<slug>` | `master` | `master` |

- Não faça commits diretamente em `develop` ou `master`.
- Antes de criar `feature/` ou `hotfix/`, confirme explicitamente com o usuário a branch-base.
- O commit de bootstrap de um repositório vazio é a única exceção operacional: ele cria a base inicial em `master`; após isso, criar `develop` a partir de `master` e usar o fluxo acima.

## Unity

- `Assets/`, `Packages/` e `ProjectSettings/` são a raiz funcional do projeto Unity e devem ser preservados juntos.
- Versione arquivos `.meta`; não versione `Library/`, `Temp/`, `Logs/`, `UserSettings/`, builds ou arquivos de solução/projeto gerados pela IDE.
- Arquivos binários grandes usam Git LFS conforme `.gitattributes`.
- Não altere configurações de plataforma, dependências, assets ou cenas fora do escopo da issue.
