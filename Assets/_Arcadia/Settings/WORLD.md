# Contratos de mundo e sessão (M2)

## Responsabilidades

- `MapDefinition` é um `ScriptableObject` de conteúdo. Ele associa um `mapId` estável à cena Unity, indica se o mapa é seguro e lista os destinos dos portais.
- `PortalDefinition` usa um `portalId` estável e aponta para `destinationMapId` e `destinationSpawnPointId`.
- `Portal` é o componente de cena do prefab reutilizável. Seu gatilho físico solicita uma única transição enquanto o jogador permanece na área.
- `PortalVisual` desenha o marcador técnico temporário: um anel ciano e uma seta que aponta a saída. A arte final dos reinos substituirá somente essa apresentação.
- `SpawnPointDefinition` representa o ID e a posição de uma entrada sem conhecer um `GameObject` da cena.
- `SpawnPoint` é o componente de cena que converte sua posição em uma `SpawnPointDefinition`.
- `ExplorationLocation` guarda o mapa, o ponto de entrada e a última posição do jogador.
- `GameSession` mantém a última `ExplorationLocation` somente em memória enquanto o aplicativo está aberto.

## Regra de identidade

Os sistemas usam IDs como `map.valdora.village` e `spawn.south_gate`. Eles não usam o texto que aparece ao jogador, o nome do `GameObject` nem seu caminho na hierarquia como identificadores.

## Mapas técnicos atuais

| Asset | ID estável | Cena | Zona segura | Ponto de entrada |
| --- | --- | --- | --- | --- |
| `Content/Maps/ExplorationMap` | `map.prototype.exploration` | `Prototype_Exploration` | Sim | `spawn.exploration.start` |
| `Content/Maps/OutpostMap` | `map.prototype.outpost` | `Prototype_Exploration_Second` | Sim | `spawn.outpost.arrival` |

Os dois pontos de entrada ficam em uma área livre de colisores. Nesta etapa eles registram a posição de chegada; o reposicionamento do jogador será conectado pela transição de mapas.

## Portais técnicos atuais

| Mapa de origem | Prefab e posição | Portal | Destino |
| --- | --- | --- | --- |
| `Prototype_Exploration` | `Portal` em `(4.5, -2.5)`, seta à direita | `portal.exploration.to.outpost` | `map.prototype.outpost` / `spawn.outpost.arrival` |
| `Prototype_Exploration_Second` | `Portal` em `(-4.5, 2.5)`, seta à esquerda | `portal.outpost.to.exploration` | `map.prototype.exploration` / `spawn.exploration.start` |

## Limite deste marco

Os portais já são visíveis e comunicam a direção da saída, mas ainda não carregam cenas nem movem o jogador para um ponto de entrada. A próxima issue do M2 consumirá a solicitação do gatilho e fará a transição com fade.

O conteúdo de `GameSession` não é persistido: ao encerrar o aplicativo, o estado é perdido. A integração com API e banco de dados continua fora de escopo.
