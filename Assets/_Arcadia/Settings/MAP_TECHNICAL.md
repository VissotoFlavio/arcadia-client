# Mapa técnico de exploração

## Propósito

O mapa da cena `Prototype_Exploration` valida leitura isométrica, deslocamento e colisão sem depender de arte final ou de conteúdo de reinos.

## Estrutura visual

- `Grid`: usa o layout **Isometric** e define a conversão entre células lógicas e o mundo 2D.
- `GroundTilemap`: chão placeholder de 24 × 14 células, alternando dois tons verdes somente para facilitar a leitura da malha.
- `ObstacleTilemap`: paredes provisórias nas bordas norte e oeste, além de uma parede curta interna.
- `TechnicalTreePlaceholder` e `TechnicalRockPlaceholder`: elementos altos/visuais provisórios gerados para testar entendimento de profundidade e ordenação por Y.

O componente `TechnicalIsometricMapBuilder`, presente no objeto `Grid`, monta esses placeholders quando a cena é executada. Ele não é a solução de conteúdo final: no futuro, o mapa será composto por tiles e dados de nível versionados.

## Colisão

- O chão não recebe colisor.
- Cada tile de parede recebe `Tile.ColliderType.Grid`, portanto o `TilemapCollider2D` cobre somente as células ocupadas pela base das paredes.
- Árvore e rocha têm apenas um `CircleCollider2D` no pé. A área visual acima do chão não bloqueia o jogador.
- `PlayerRoot` já possui `Rigidbody2D` dinâmico e `CircleCollider2D` nos pés; por isso, ao pressionar uma parede, a física impede deslocamento efetivo através dela.

## Limites desta entrega

Não há arte final, regras de oclusão, biomas, reinos nem mapa navegável persistido. Esses pontos serão entregas posteriores.

Consulte [a regra de ordenação por Y](Y_SORTING.md) para os valores de ordem, agrupamento visual e a limitação atual do Tilemap.
