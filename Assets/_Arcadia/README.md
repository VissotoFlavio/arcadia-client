# _Arcadia

Esta é a raiz de todos os assets e códigos próprios do cliente Arcadia. Não adicione novos assets diretamente em `Assets/`.

| Pasta | Responsabilidade |
| --- | --- |
| `Art/` | Sprites, tiles, animações e demais arte importada. |
| `Audio/` | Música e efeitos sonoros. |
| `Content/` | Dados de jogo, como ScriptableObjects e tabelas de configuração. |
| `Prefabs/` | GameObjects reutilizáveis salvos como prefabs. |
| `Scenes/` | Cenas do cliente. `Boot` é a primeira cena habilitada na build e abre `Prototype_Exploration`. |
| `Scripts/` | Código C# do cliente, organizado por domínio ou funcionalidade. |
| `Settings/` | Configurações próprias do projeto Arcadia. |
| `Tests/` | Testes do cliente. |

## Fluxo inicial de cenas

1. A build inicia em `Boot`.
2. `BootSceneLoader` troca para `Prototype_Exploration`.
3. As próximas histórias irão colocar o jogador, a câmera, os controles e o mapa nessa cena de protótipo.
