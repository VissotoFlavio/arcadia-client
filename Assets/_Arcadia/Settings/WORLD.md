# Contratos de mundo e sessão (M2)

## Responsabilidades

- `MapDefinition` é um `ScriptableObject` de conteúdo. Ele associa um `mapId` estável à cena Unity, indica se o mapa é seguro e lista os destinos dos portais.
- `PortalDefinition` usa um `portalId` estável e aponta para `destinationMapId` e `destinationSpawnPointId`.
- `SpawnPointDefinition` representa o ID e a posição de uma entrada sem conhecer um `GameObject` da cena.
- `ExplorationLocation` guarda o mapa, o ponto de entrada e a última posição do jogador.
- `GameSession` mantém a última `ExplorationLocation` somente em memória enquanto o aplicativo está aberto.

## Regra de identidade

Os sistemas usam IDs como `map.valdora.village` e `spawn.south_gate`. Eles não usam o texto que aparece ao jogador, o nome do `GameObject` nem seu caminho na hierarquia como identificadores.

## Limite deste marco

Estes contratos ainda não carregam cenas nem movem o jogador. O prefab visual do portal, os componentes de `SpawnPoint` na cena e a transição com fade pertencem às issues posteriores do M2.

O conteúdo de `GameSession` não é persistido: ao encerrar o aplicativo, o estado é perdido. A integração com API e banco de dados continua fora de escopo.
