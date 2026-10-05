# Contesto del progetto per l'AI

Questo file descrive il progetto a un assistente AI (Claude, ChatGPT, Copilot, Cursor…).
Allegalo o incollalo all'inizio di ogni conversazione. **Va aggiornato ogni volta che si
aggiunge un sistema**: un contesto vecchio porta l'AI a inventare.

## Il gioco

**Crystal Survivors**: action tower defense con visuale dall'alto e attacco automatico, in stile
Vampire Survivors. Il giocatore si muove in un'arena 40 × 40, le armi colpiscono da sole il
nemico più vicino, il cristallo al centro va difeso dalle ondate di nemici.

## Tecnologia

- Unity **6000.6.0f1** (Unity 6), Universal Render Pipeline
- **Input System** (*Active Input Handling = Input System Package*): `UnityEngine.Input` NON funziona.
  Asset `Input/GameControls.inputactions`, classe generata `GameControls`, Action Map `Player`:
  `Move` (Vector2), `Sprint`, `PlaceTower`, `Pause` (Button)
- UI: uGUI con `UnityEngine.UI.Text` (non TextMeshPro)
- API di Unity 6: `Rigidbody.linearVelocity` (non `velocity`), `FindFirstObjectByType` (non `FindObjectOfType`)

## Convenzioni

- Una classe per file, nome del file uguale al nome della classe, nessun namespace
- Campi `private` con `[SerializeField]`; proprietà in sola lettura per esporre i valori
- Riferimenti recuperati in `Awake`; mai `GetComponent`/`Find` in `Update`
- Iscrizione agli eventi in `OnEnable`, disiscrizione in `OnDisable`
- Commenti in italiano, nomi in inglese

## Layer e fisica

`Ground`, `Obstacle`, `Player`, `Enemy`, `Projectile`, `Pickup`, `Structure`.
`Projectile` tocca solo `Enemy` e `Obstacle`; `Pickup` tocca solo `Player`; `Enemy` non tocca `Enemy`.

## Sistemi esistenti

| Sistema | Classi principali |
|---|---|
| Player | `PlayerController` (CharacterController), `CameraFollow`, `GemCollector` |
| Vita | `Health` (eventi `HealthChanged`, `Died`), `AreaEffect` (zone trigger) |
| Combattimento | `AutoTargeter` (OverlapSphere + Linecast), `SimpleShooter`, `Projectile` (Rigidbody cinematico) |
| Nemici (forniti) | `SimpleChaser`, `ContactDamage`, `EnemySpawner`, `DropOnDeath` |
| UI | `PauseMenu` |

## Scene

`Arena` (gioco), `Sandbox_Lifecycle` (esercizi).

## Comandi verso Unity

Se puoi eseguire comandi nel terminale, usa il bridge descritto qui: @UNITY_BRIDGE.md
