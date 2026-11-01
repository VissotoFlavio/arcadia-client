# Input de exploração (M0)

## Contrato atual

`ArcadiaInputActions.inputactions` expõe `Exploration/Move`, uma ação `Value` que produz `Vector2` normalizado.

- Editor: `W`, `A`, `S`, `D` e setas direcionais.
- Mobile: `OnScreenStick` escreve no `<Gamepad>/leftStick` virtual; a ação lê esse mesmo controle.
- `ExplorationInputReader` possui a ação, mas não conhece nem move um jogador. A task de movimento consumirá o vetor por uma dependência explícita.

## Valores provisórios

| Valor | Configuração inicial | Situação |
| --- | --- | --- |
| Zona morta do stick | mínimo `0,15`; máximo `0,95` | Hipótese para teste em dispositivo; ainda não é balanceamento. |
| Raio visual do joystick | `72` px de UI | Hipótese para toque confortável em landscape. |
| Velocidade espacial do jogador | `4` unidades/s | Referência para a próxima task; não é aplicada nem homologada nesta entrega. |

Esses valores deverão ser medidos em Android antes de serem considerados definitivos.
