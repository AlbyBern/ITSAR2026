# Contesto del progetto per l'AI

Questo file descrive il progetto a un assistente AI (Claude, ChatGPT, Copilot, Cursor…).
Allegalo o incollalo all'inizio di ogni conversazione.

## Il gioco

**Crystal Survivors**: action tower defense con visuale dall'alto e attacco automatico, in stile
Vampire Survivors. Il giocatore si muove in un'arena 40 × 40, le armi colpiscono da sole il
nemico più vicino, il cristallo al centro va difeso dalle ondate di nemici.

## Tecnologia

- Unity **6000.6.0f1** (Unity 6), Universal Render Pipeline
- **Input System** (pacchetto nuovo, *Active Input Handling = Input System Package*):
  il vecchio `UnityEngine.Input` (`Input.GetAxis`, `Input.GetKey`) **non funziona** in questo progetto
- Input: asset `Assets/_Project/Input/GameControls.inputactions`, classe generata `GameControls`,
  Action Map `Player` con `Move` (Vector2), `Sprint`, `PlaceTower`, `Pause` (Button)
- UI: uGUI con `UnityEngine.UI.Text` (non TextMeshPro)
- Grafica: solo primitive e materiali URP Lit

## Convenzioni

- Una classe per file, nome del file uguale al nome della classe
- Campi `private` con `[SerializeField]`; niente campi `public` per la configurazione
- Riferimenti tra componenti recuperati in `Awake`; mai `GetComponent` o `Find` in `Update`
- Movimenti e timer moltiplicati per `Time.deltaTime`
- Iscrizione agli eventi in `OnEnable`, disiscrizione in `OnDisable`
- Commenti in italiano, nomi di classi e metodi in inglese
- Nessun namespace

## Struttura

```
Assets/_Project/
  Scenes/    Arena (gioco), Sandbox_Lifecycle (esercizi)
  Scripts/   Core, Player, UI
  Prefabs/   Crystal
  Input/     GameControls
```

## Scena Arena

- `Environment`: terreno, muri (`Walls`), ostacoli (`Obstacles`)
- `Gameplay`: `Player`, `Crystal`, `SpawnPoints` (8 punti in cerchio)
- `Main Camera`: visuale dall'alto
- `UI`: canvas con `PausePanel`

## Comandi verso Unity

Se puoi eseguire comandi nel terminale, usa il bridge descritto qui: @UNITY_BRIDGE.md
