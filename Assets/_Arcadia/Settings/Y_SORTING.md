# Ordenação visual por Y

## Regra global

Todo personagem ou prop alto que possa cruzar visualmente com outro elemento usa `YSortOrder` e um `SortingGroup`.

```text
sortingOrder = -round(Y_do_pé × 100)
```

Quanto menor o Y do ponto de apoio, maior a ordem de desenho e mais à frente o objeto aparece. O ponto de apoio é sempre o pé físico do objeto, não o centro nem o topo do sprite.

## Personagem

`PlayerRoot` continua sendo a origem física do jogador. O filho `VisualRoot` contém o `SortingGroup`; por isso, futuras camadas como corpo, cabelo, armadura e equipamentos compartilharão uma única ordem global. A ordenação interna dessas camadas será uma preocupação futura do compositor visual.

## Cenário técnico

Árvore e rocha usam a mesma regra e informam explicitamente seu transform de pé ao `YSortOrder`. Assim, o jogador aparece à frente ao estar abaixo do obstáculo e atrás ao estar acima dele.

`GroundTilemap` usa a ordem fixa `-10000`, e `ObstacleTilemap` usa `-9000`. Essas faixas deixam o chão atrás de todos os grupos ordenados por Y.

## Limitação observada

O `TilemapRenderer` está no modo `Chunk`. Sua ordem é aplicada ao conjunto do Tilemap, não a cada célula; portanto, as paredes técnicas não podem se intercalar individualmente com o jogador usando somente esta configuração. Para props que exijam oclusão por célula, a evolução será avaliar `Individual Mode` e seu custo em Android, ou representar o prop alto como `SpriteRenderer` com `YSortOrder`.
