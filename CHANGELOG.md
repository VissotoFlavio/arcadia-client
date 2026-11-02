# Changelog

Todas as mudanças relevantes deste projeto serão registradas neste arquivo.

O formato segue [Keep a Changelog](https://keepachangelog.com/pt-BR/1.1.0/) e este projeto utiliza versionamento semântico.

## [Unreleased]

### Added

- Enquadramento fixo da câmera com deslocamento de leitura à frente e jogador abaixo do centro (#22).
- Transição entre mapas técnicos com fade curto, bloqueio de entrada e posicionamento no `SpawnPoint` de destino (#20).
- Prefab técnico de portal visível, com gatilho de solicitação única e rotas de ida e volta entre os mapas técnicos (#19).
- Segundo mapa técnico `Prototype_Exploration_Second`, definições de mapas seguros e pontos de entrada estáveis para as duas cenas (#18).
- Contratos de mapa, portal, ponto de entrada e sessão local em memória para o M2 (#17).
- Perfil de build Android de desenvolvimento e validação concluída em dispositivo físico (#8).
- Ordenação visual global por Y dos pés para jogador e props altos técnicos (#7).
- Mapa técnico isométrico com chão placeholder, paredes, árvore, rocha e colisão por ocupação no pé (#6).
- PlayerRoot técnico com movimento físico 2D e visual placeholder (#4).
- Câmera ortográfica de exploração com acompanhamento e limites técnicos de mapa (#5).

## [0.1.0] - 2026-11-01

### Added

- Estrutura inicial do projeto Arcadia, cenas `Boot` e `Prototype_Exploration`, e fluxo de inicialização (#2).
- Ação `Exploration/Move`, orientação mobile, controles de teclado e joystick virtual editável baseado no Input System (#3).
