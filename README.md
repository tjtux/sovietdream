# Soviet Dream

Um jogo 2D top-down desenvolvido em Unity.

## Descrição

Soviet Dream é um jogo 2D com visão top-down onde o jogador explora diferentes áreas (quarteirões) interconectadas. O jogador pode coletar e carregar itens, interagir com o cenário e navegar entre as fases do jogo.

### Características

- Movimento em 4 direções
- Sistema de coleta de itens (um item por vez)
- Transição entre fases através de zonas de interação
- 4 fases conectadas em formato de cruzamento

## Requisitos

### Para Desenvolvimento

- **Unity**: 6000.3.6f1 (Unity 6)
- **Sistema Operacional**: Windows 10/11, macOS 10.15+, ou Linux (Ubuntu 20.04+)
- **IDE Recomendada**: Visual Studio Code, Visual Studio 2022, ou JetBrains Rider

### Para Jogar (Build)

- Windows 10/11 (64-bit)
- macOS 10.15+ (Intel ou Apple Silicon)
- Linux (Ubuntu 20.04+ ou equivalente)

## Como Abrir o Projeto

1. **Instale o Unity Hub**
   - Baixe em: https://unity.com/download

2. **Instale a versão correta do Unity**
   - Abra o Unity Hub
   - Vá em "Installs" > "Install Editor"
   - Selecione a versão **6000.3.6f1**

3. **Clone o repositório**
   ```bash
   git clone https://github.com/seu-usuario/soviet-dream.git
   cd soviet-dream
   ```

4. **Abra o projeto**
   - No Unity Hub, clique em "Open" > "Add project from disk"
   - Selecione a pasta do projeto clonado
   - Aguarde o Unity importar os assets (pode demorar na primeira vez)

5. **Abra a cena principal**
   - No Unity, navegue até `Assets/Scenes/`
   - Abra `SampleScene.unity`

## Estrutura do Projeto

```
Assets/
├── Scenes/                    # Cenas do jogo
│   └── SampleScene.unity      # Cena principal
├── Scripts/                   # Scripts C#
│   ├── Player/                # Movimento e interação do jogador
│   ├── Items/                 # Sistema de itens
│   ├── Interaction/           # Zonas de interação
│   ├── Level/                 # Gerenciamento de fases
│   ├── Camera/                # Câmera
│   └── UI/                    # Interface do usuário
├── Settings/                  # Configurações do URP
└── InputSystem_Actions.inputactions  # Mapeamento de controles
```

## Controles

| Tecla | Ação |
|-------|------|
| `W` / `↑` | Mover para cima |
| `S` / `↓` | Mover para baixo |
| `A` / `←` | Mover para esquerda |
| `D` / `→` | Mover para direita |
| `E` | Pegar item |
| `Q` | Soltar item |
| `Espaço` | Interagir com cenário |

### Gamepad

| Botão | Ação |
|-------|------|
| Analógico Esquerdo | Movimento |
| Botão Norte (Y/△) | Interagir |
| Botão Sul (A/✕) | Pular/Confirmar |

## Como Jogar

1. Use **WASD** ou as **setas** para se mover pelo cenário
2. Aproxime-se de um item e pressione **E** para pegá-lo
3. Pressione **Q** para soltar o item que está carregando
4. Entre em uma zona de transição e pressione **Espaço** para mudar de fase

## Build do Jogo

### Windows
```bash
# Via linha de comando (substitua pelo caminho do seu Unity)
"C:\Program Files\Unity\Hub\Editor\6000.3.6f1\Editor\Unity.exe" -batchmode -projectPath . -buildTarget StandaloneWindows64 -buildWindows64Player build/SovietDream.exe
```

### Linux
```bash
/path/to/Unity -batchmode -projectPath . -buildTarget StandaloneLinux64 -buildLinux64Player build/SovietDream
```

### macOS
```bash
/Applications/Unity/Hub/Editor/6000.3.6f1/Unity.app/Contents/MacOS/Unity -batchmode -projectPath . -buildTarget StandaloneOSX -buildOSXUniversalPlayer build/SovietDream.app
```

## Tecnologias Utilizadas

- **Unity 6** (6000.3.6f1)
- **Universal Render Pipeline (URP)** para renderização 2D
- **Unity Input System** para controles
- **C#** como linguagem de programação

## Licença

Este projeto está licenciado sob a Licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.

## Contribuição

Contribuições são bem-vindas! Por favor, leia o [CONTRIBUTING.md](CONTRIBUTING.md) para detalhes sobre como contribuir.
