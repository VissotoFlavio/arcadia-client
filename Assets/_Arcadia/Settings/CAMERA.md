# Câmera de exploração (M1)

## Contrato atual

`ExplorationCameraController` acompanha o `PlayerRoot` em `LateUpdate`. A câmera não altera o `Rigidbody2D` nem a posição do jogador; apenas calcula sua própria posição após o ciclo de física.

O controlador limita o centro da câmera usando a área navegável do mapa e o tamanho atual do viewport. Quando o mapa for menor que o viewport em algum eixo, a câmera permanece centralizada nesse eixo.

## Valores provisórios

| Valor | Configuração inicial | Situação |
| --- | --- | --- |
| Projeção | Ortográfica | Aplicada na `Main Camera`. |
| Zoom | `5` unidades ortográficas | Valor homologado no protótipo para landscape. |
| Deslocamento de enquadramento | `(0; 1,25)` unidades de mundo | A câmera olha à frente e mantém o jogador levemente abaixo do centro. |
| Suavização | `0,15` s | Valor inicial mantido para acompanhamento responsivo. |
| Limites técnicos | Retângulo `(-12, -7)` com tamanho `(24, 14)` | Placeholder até a criação do mapa técnico. |
| Pixel-perfect | Componente desativado; assets usam `64` PPU | Resolução virtual e tiles do cenário ainda não foram definidos. |

O deslocamento é aplicado antes da limitação pelos `worldBounds`; perto das bordas, o limite prevalece para não revelar áreas externas. A rotação, o zoom manual e a alteração de posição pelo jogador continuam fora do escopo.

O pixel-perfect deverá ser configurado quando houver resolução virtual e tiles técnicos para validar; não deve ser assumido a partir do PPU dos sprites.
