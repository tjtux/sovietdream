# Guia de Contribuição

Obrigado pelo interesse em contribuir com o Soviet Dream! Este documento contém as diretrizes para contribuir com o projeto.

## Sumário

- [Código de Conduta](#código-de-conduta)
- [Como Contribuir](#como-contribuir)
- [Configuração do Ambiente](#configuração-do-ambiente)
- [Organização de Pastas](#organização-de-pastas)
- [Padrões de Código](#padrões-de-código)
- [Boas Práticas de Commit](#boas-práticas-de-commit)
- [Pull Requests](#pull-requests)
- [Reportando Bugs](#reportando-bugs)

---

## Código de Conduta

- Seja respeitoso com outros contribuidores
- Aceite críticas construtivas
- Foque no que é melhor para o projeto
- Mantenha discussões técnicas e objetivas

---

## Como Contribuir

1. **Fork** o repositório
2. **Clone** seu fork localmente
3. Crie uma **branch** para sua feature/fix
4. Faça suas alterações seguindo os padrões do projeto
5. **Teste** suas alterações
6. Faça **commit** seguindo as convenções
7. **Push** para seu fork
8. Abra um **Pull Request**

---

## Configuração do Ambiente

### Requisitos

- Unity 6000.3.6f1 (versão exata)
- Git 2.30+
- IDE: VS Code, Visual Studio 2022, ou JetBrains Rider

### Configuração do Unity

1. Em **Edit > Project Settings > Editor**:
   - Asset Serialization: `Force Text`
   - Version Control Mode: `Visible Meta Files`

2. Em **Edit > Project Settings > Player**:
   - Mantenha as configurações padrão do projeto

### Configuração do Git (opcional, para merge de YAML)

Adicione ao seu `.gitconfig` global:

```ini
[merge "unityyamlmerge"]
    name = Unity YAML Merge
    driver = '<caminho-do-unity>/Editor/Data/Tools/UnityYAMLMerge.exe' merge -p %O %B %A %A
```

---

## Organização de Pastas

Mantenha a seguinte estrutura:

```
Assets/
├── Scenes/                    # Cenas do jogo
│   ├── MainMenu.unity
│   ├── Level1.unity
│   └── ...
│
├── Scripts/                   # Scripts C#
│   ├── Player/                # Scripts do jogador
│   ├── Items/                 # Sistema de itens
│   ├── Interaction/           # Zonas de interação
│   ├── Level/                 # Gerenciamento de fases
│   ├── Camera/                # Scripts de câmera
│   ├── UI/                    # Interface do usuário
│   └── Utils/                 # Utilitários e helpers
│
├── Prefabs/                   # Prefabs do jogo
│   ├── Player/
│   ├── Items/
│   ├── Enemies/
│   └── UI/
│
├── Sprites/                   # Imagens e sprites
│   ├── Player/
│   ├── Items/
│   ├── Environment/
│   └── UI/
│
├── Audio/                     # Arquivos de áudio
│   ├── Music/
│   ├── SFX/
│   └── Ambient/
│
├── Animations/                # Animações
│   ├── Player/
│   └── ...
│
├── Materials/                 # Materiais
│
├── Settings/                  # Configurações do URP e projeto
│
└── Resources/                 # Assets carregados em runtime (usar com moderação)
```

### Regras de Organização

- **Nomes de arquivo**: PascalCase (ex: `PlayerMovement.cs`, `EnemySprite.png`)
- **Nomes de pasta**: PascalCase (ex: `Player/`, `Items/`)
- **Prefabs**: Prefixe com o tipo se necessário (ex: `Item_Key.prefab`)
- **Cenas**: Nome descritivo (ex: `Level1.unity`, `MainMenu.unity`)

---

## Padrões de Código

### Nomenclatura C#

```csharp
// Classes e Structs: PascalCase
public class PlayerMovement { }

// Interfaces: I + PascalCase
public interface IInteractable { }

// Métodos públicos: PascalCase
public void MovePlayer() { }

// Métodos privados: PascalCase
private void CalculateDamage() { }

// Variáveis públicas: camelCase (evite, prefira propriedades)
public float moveSpeed;

// Variáveis privadas: camelCase
private float currentHealth;

// Campos serializados: camelCase com [SerializeField]
[SerializeField] private float jumpForce;

// Constantes: UPPER_SNAKE_CASE
private const int MAX_HEALTH = 100;

// Propriedades: PascalCase
public bool IsAlive { get; private set; }
```

### Estrutura de Script

```csharp
using UnityEngine;

/// <summary>
/// Descrição breve do que o script faz.
/// </summary>
public class ExampleScript : MonoBehaviour
{
    // 1. Constantes
    private const float DEFAULT_SPEED = 5f;

    // 2. Campos serializados (visíveis no Inspector)
    [Header("Configurações")]
    [SerializeField] private float moveSpeed = DEFAULT_SPEED;
    [SerializeField] private Transform target;

    // 3. Campos privados
    private Rigidbody2D rb;
    private bool isMoving;

    // 4. Propriedades públicas
    public bool IsMoving => isMoving;

    // 5. Métodos Unity (em ordem de ciclo de vida)
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        HandleInput();
    }

    private void FixedUpdate()
    {
        Move();
    }

    // 6. Métodos públicos
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    // 7. Métodos privados
    private void Initialize()
    {
        // ...
    }

    private void HandleInput()
    {
        // ...
    }

    private void Move()
    {
        // ...
    }
}
```

### Boas Práticas

- Use `[SerializeField]` em vez de campos públicos
- Agrupe campos com `[Header("Nome")]`
- Evite `Find()` e `GetComponent()` em `Update()`
- Cache referências em `Awake()` ou `Start()`
- Use `CompareTag()` em vez de `== "tag"`
- Prefira composição sobre herança

---

## Boas Práticas de Commit

### Formato da Mensagem

```
<tipo>: <descrição curta>

[corpo opcional]

[rodapé opcional]
```

### Tipos de Commit

| Tipo | Descrição |
|------|-----------|
| `feat` | Nova funcionalidade |
| `fix` | Correção de bug |
| `refactor` | Refatoração de código |
| `docs` | Documentação |
| `style` | Formatação (não altera lógica) |
| `test` | Adição/correção de testes |
| `chore` | Tarefas de manutenção |
| `asset` | Adição/modificação de assets |

### Exemplos

```bash
# Boa mensagem
feat: adiciona sistema de coleta de itens

# Boa mensagem com corpo
fix: corrige jogador atravessando paredes

O Rigidbody2D estava com collision detection em Discrete.
Alterado para Continuous para evitar tunneling.

# Mensagens ruins (evite)
fix: correções
update: atualizações
wip: work in progress
```

### Regras

1. **Descrição curta**: máximo 72 caracteres
2. **Tempo verbal**: imperativo ("adiciona", não "adicionado")
3. **Sem ponto final** na descrição curta
4. **Commits atômicos**: uma mudança lógica por commit
5. **Não commite arquivos quebrados**: o projeto deve compilar

### O que NÃO commitar

- Arquivos da pasta `Library/`
- Arquivos `.csproj` e `.sln` (regenerados pelo Unity)
- Configurações locais de IDE
- Builds do jogo
- Assets de terceiros sem licença apropriada

---

## Pull Requests

### Antes de Abrir um PR

- [ ] Código segue os padrões do projeto
- [ ] Projeto compila sem erros
- [ ] Testado no Unity Editor
- [ ] Commits seguem as convenções
- [ ] Branch atualizada com `main`

### Título do PR

Use o mesmo formato dos commits:

```
feat: adiciona sistema de inventário
fix: corrige bug na transição de fases
```

### Descrição do PR

```markdown
## Descrição
Breve descrição do que foi feito.

## Mudanças
- Item 1
- Item 2

## Como Testar
1. Passo 1
2. Passo 2

## Screenshots (se aplicável)
[Adicione imagens ou GIFs]

## Checklist
- [ ] Testado no Unity
- [ ] Documentação atualizada
```

### Processo de Review

1. Pelo menos 1 aprovação necessária
2. Todos os comentários devem ser resolvidos
3. CI deve passar (se configurado)
4. Merge via "Squash and merge" para manter histórico limpo

---

## Reportando Bugs

### Use o Template de Issue

```markdown
## Descrição do Bug
Descrição clara e concisa do bug.

## Como Reproduzir
1. Vá para '...'
2. Clique em '...'
3. Veja o erro

## Comportamento Esperado
O que deveria acontecer.

## Comportamento Atual
O que está acontecendo.

## Screenshots
Se aplicável, adicione screenshots.

## Ambiente
- OS: [ex: Windows 11]
- Unity: [ex: 6000.3.6f1]

## Informações Adicionais
Qualquer contexto adicional.
```

---

## Dúvidas?

Abra uma issue com a tag `question` ou entre em contato com os mantenedores.

Obrigado por contribuir!
