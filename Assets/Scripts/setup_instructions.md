================================================================================
                    SOVIET DREAM - INSTRUÇÕES DE CONFIGURAÇÃO
================================================================================

Este documento explica como configurar o jogo no Unity Editor.

================================================================================
1. ESTRUTURA DE SCRIPTS
================================================================================

Assets/Scripts/
├── Player/
│   ├── PlayerMovement.cs      → Movimento 4 direções (WASD/Setas)
│   └── PlayerInteraction.cs   → Pegar (E), Soltar (Q), Interagir (Espaço)
├── Items/
│   └── Item.cs                → Item coletável
├── Interaction/
│   └── InteractionZone.cs     → Zona de interação (transições de fase)
├── Level/
│   ├── LevelManager.cs        → Gerencia carregamento de fases
│   └── SpawnPoint.cs          → Ponto de spawn do jogador
└── UI/
    └── ItemIndicatorUI.cs     → UI mostrando item carregado (opcional)

================================================================================
2. CONFIGURAÇÃO DO JOGADOR
================================================================================

A) Criar o GameObject do Player:
   1. GameObject > Create Empty > Renomear para "Player"
   2. Adicionar Tag "Player" (IMPORTANTE!)

B) Adicionar Componentes ao Player:
   - Rigidbody2D
     * Body Type: Dynamic
     * Gravity Scale: 0
     * Constraints: Freeze Rotation Z (marcar)

   - BoxCollider2D ou CircleCollider2D
     * Ajustar tamanho conforme sprite

   - SpriteRenderer
     * Adicionar um sprite para o jogador

   - PlayerMovement (script)
     * Move Speed: 5 (ajustar conforme desejado)

   - PlayerInteraction (script)
     * Item Detection Radius: 1.5
     * Item Layer: Selecionar a layer "Item" (criar se não existir)

C) Criar ponto de ancoragem do item:
   1. Criar GameObject filho vazio dentro do Player
   2. Renomear para "ItemHoldPoint"
   3. Posicionar acima ou ao lado do jogador
   4. Arrastar para o campo "Item Hold Point" no PlayerInteraction

D) Adicionar PlayerInput (opcional, para novo Input System):
   1. Add Component > Player Input
   2. Actions: Arrastar o asset InputSystem_Actions
   3. Default Map: Player

================================================================================
3. CONFIGURAÇÃO DE ITENS
================================================================================

A) Criar Layer para Itens:
   1. Edit > Project Settings > Tags and Layers
   2. Em "Layers", adicionar uma nova layer chamada "Item"

B) Criar um Item:
   1. GameObject > Create Empty > Renomear para "Item_NomeDoItem"
   2. Definir Layer como "Item"

C) Adicionar Componentes ao Item:
   - SpriteRenderer
     * Adicionar sprite do item

   - BoxCollider2D ou CircleCollider2D
     * Is Trigger: DESMARCADO (para detecção via OverlapCircle)
     * Ajustar tamanho conforme sprite

   - Item (script)
     * Item Name: Nome do item (ex: "Chave", "Caixa")
     * Item Sprite: (opcional) sprite alternativo

D) Criar Prefab:
   1. Arrastar o GameObject para Assets/Prefabs/Items/
   2. Usar o prefab para criar mais instâncias

================================================================================
4. CONFIGURAÇÃO DE ZONAS DE INTERAÇÃO
================================================================================

A) Criar Zona de Transição:
   1. GameObject > Create Empty > Renomear (ex: "Zone_ToLevel2")

B) Adicionar Componentes:
   - BoxCollider2D
     * Is Trigger: MARCADO
     * Ajustar tamanho da zona

   - InteractionZone (script)
     * Zone Type: LevelTransition
     * Target Scene Name: Nome da cena destino (ex: "Level2")
     * Spawn Point Id: ID do spawn na cena destino (ex: "FromLevel1")
     * Requires Item: Marcar se precisa de item
     * Required Item Name: Nome do item necessário

C) Visualização:
   - No Editor, a zona aparece em verde semi-transparente

================================================================================
5. CONFIGURAÇÃO DE SPAWN POINTS
================================================================================

A) Criar Spawn Point:
   1. GameObject > Create Empty > Renomear (ex: "SpawnPoint_FromLevel1")

B) Adicionar Componente:
   - SpawnPoint (script)
     * Spawn Id: Identificador único (ex: "FromLevel1", "FromNorth")

C) Posicionar:
   - Colocar onde o jogador deve aparecer ao entrar na fase

================================================================================
6. CONFIGURAÇÃO DO LEVEL MANAGER
================================================================================

A) Criar o Level Manager (apenas uma vez, na primeira cena):
   1. GameObject > Create Empty > Renomear para "LevelManager"

B) Adicionar Componente:
   - LevelManager (script)
     * Fade Time: 0.5 (tempo de transição)

IMPORTANTE: O LevelManager usa DontDestroyOnLoad e persiste entre cenas.
            Criar apenas na primeira cena do jogo.

================================================================================
7. CRIAR AS 4 FASES
================================================================================

Estrutura de Cruzamento:

    [ Level1 ] | [ Level2 ]
    -----------+-----------
    [ Level3 ] | [ Level4 ]

A) Criar as Cenas:
   1. File > New Scene > Save As "Level1"
   2. Repetir para Level2, Level3, Level4

B) Adicionar cenas ao Build Settings:
   1. File > Build Settings
   2. Arrastar todas as 4 cenas para "Scenes In Build"

C) Configurar Zonas de Saída em cada fase:

   LEVEL 1 (Superior Esquerdo):
   - Saída LESTE  → Target: "Level2", SpawnId: "FromWest"
   - Saída SUL    → Target: "Level3", SpawnId: "FromNorth"

   LEVEL 2 (Superior Direito):
   - Saída OESTE  → Target: "Level1", SpawnId: "FromEast"
   - Saída SUL    → Target: "Level4", SpawnId: "FromNorth"

   LEVEL 3 (Inferior Esquerdo):
   - Saída NORTE  → Target: "Level1", SpawnId: "FromSouth"
   - Saída LESTE  → Target: "Level4", SpawnId: "FromWest"

   LEVEL 4 (Inferior Direito):
   - Saída NORTE  → Target: "Level2", SpawnId: "FromSouth"
   - Saída OESTE  → Target: "Level3", SpawnId: "FromEast"

D) Adicionar Spawn Points correspondentes em cada fase:
   - Level1: "FromEast", "FromSouth"
   - Level2: "FromWest", "FromSouth"
   - Level3: "FromNorth", "FromEast"
   - Level4: "FromNorth", "FromWest"

================================================================================
8. CONFIGURAÇÃO DA CÂMERA
================================================================================

Opção 1 - Câmera Fixa:
- Manter a Main Camera na posição central da fase

Opção 2 - Câmera Seguindo o Player:
Criar um script simples:

```csharp
public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 5f;
    public Vector3 offset = new Vector3(0, 0, -10);

    void LateUpdate()
    {
        if (target == null) return;
        Vector3 desired = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desired, smoothSpeed * Time.deltaTime);
    }
}
```

================================================================================
9. CONTROLES DO JOGO
================================================================================

WASD ou Setas  → Movimento (4 direções)
E              → Pegar item próximo
Q              → Soltar item carregado
Espaço         → Interagir com cenário (transição de fase)

================================================================================
10. CHECKLIST FINAL
================================================================================

[ ] Player tem tag "Player"
[ ] Player tem Rigidbody2D com Gravity Scale = 0
[ ] Player tem Collider2D
[ ] Player tem PlayerMovement e PlayerInteraction
[ ] PlayerInteraction tem ItemHoldPoint configurado
[ ] PlayerInteraction tem ItemLayer configurado

[ ] Itens estão na layer "Item"
[ ] Itens têm Collider2D (Is Trigger = false)
[ ] Itens têm script Item

[ ] Zonas de interação têm Collider2D (Is Trigger = true)
[ ] Zonas têm InteractionZone com Target Scene e Spawn Point

[ ] Cada fase tem seus Spawn Points
[ ] LevelManager existe na primeira cena

[ ] Todas as cenas estão no Build Settings

================================================================================
