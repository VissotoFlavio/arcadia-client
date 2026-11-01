# Câmera de exploração (M1)

## Contrato atual

`ExplorationCameraController` acompanha o `PlayerRoot` em `LateUpdate`. A câmera não altera o `Rigidbody2D` nem a posição do jogador; apenas calcula sua própria posição após o ciclo de física.

O controlador limita o centro da câmera usando a área navegável do mapa e o tamanho atual do viewport. Quando o mapa for menor que o viewport em algum eixo, a câmera permanece centralizada nesse eixo.

## Valores provisórios

| Valor | Configuração inicial | Situação |
| --- | --- | --- |
| Projeção | Ortográfica | Aplicada na `Main Camera`. |
| Zoom | `5` unidades ortográficas | Hipótese para landscape; requer teste em Android. |
| Suavização | `0,15` s | Hipótese para acompanhamento responsivo; requer teste em Android. |
| Limites técnicos | Retângulo `(-12, -7)` com tamanho `(24, 14)` | Placeholder até a criação do mapa técnico. |
| Pixel-perfect | Componente desativado; assets usam `64` PPU | Resolução virtual e tiles do cenário ainda não foram definidos. |

O pixel-perfect deverá ser configurado quando houver resolução virtual e tiles técnicos para validar; não deve ser assumido a partir do PPU dos sprites.
