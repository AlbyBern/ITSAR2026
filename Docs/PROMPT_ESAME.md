# Elaborato d'esame – prompt di riferimento

**Consegna:** scrivi il prompt per far sviluppare all'AI un sistema di pickup che cura il player
o ripara il cristallo, compare periodicamente nell'arena e si integra con i sistemi esistenti.

## Il prompt

> **Contesto.** [Allego CLAUDE.md] Progetto Unity 6 (6000.6), URP, Input System, AI Navigation.
> Esistono già: `Health` (con `Heal(float)`, `Current`, `Max`, evento `Died`), il pooling statico
> `Pool.Spawn/Despawn` con l'interfaccia `IPoolable` (`OnSpawned`, `OnDespawned`), il layer
> `Pickup` che collide solo con il layer `Player`, il componente `Rotator`.
>
> **Obiettivo.** Un sistema di pickup: oggetti che compaiono periodicamente nell'arena e, quando
> il player li tocca, applicano un effetto.
>
> **Struttura.**
> - `Pickup`: classe astratta `MonoBehaviour`, implementa `IPoolable`. Richiede un collider trigger.
>   In `OnTriggerEnter` cerca il player (`GetComponentInParent<PlayerController>`) e chiama il metodo
>   astratto `bool TryApply(GameObject player)`; se restituisce true il pickup torna nel pool.
>   Metodo `Init(Health crystal)` chiamato dallo spawner.
> - `HealPickup : Pickup`: cura il player di `healAmount`. Se il player ha già la vita piena,
>   **non** viene consumato.
> - `RepairPickup : Pickup`: cura il cristallo di `repairAmount`. Se il cristallo è pieno, non viene consumato.
> - `PickupSpawner`: lista di prefab `Pickup`, un pickup ogni `interval` secondi in un punto casuale
>   a distanza `minRadius`–`maxRadius` dal cristallo, al massimo `maxActive` pickup attivi.
>   Il punto deve essere valido sulla NavMesh (`NavMesh.SamplePosition`).
>
> **Vincoli.** Tutti i parametri in Inspector con `[SerializeField]` privati. Usa `Pool.Spawn` e
> `Pool.Despawn`, mai `Instantiate`/`Destroy`. Nessun `GetComponent` o `Find` in `Update`.
> Il conteggio dei pickup attivi non deve allocare memoria (niente LINQ, niente lambda).
>
> **Criteri di accettazione.**
> - Un pickup riciclato dal pool può essere raccolto di nuovo (lo stato "raccolto" si azzera in `OnSpawned`)
> - Toccare un pickup con vita piena non lo consuma
> - Se il player tocca il pickup due volte nello stesso frame, l'effetto si applica una volta sola
> - Non compaiono pickup dentro le rocce o fuori dall'arena
>
> **Trappole da evitare.** Niente `FindObjectOfType` (deprecato in Unity 6). Non salvare stato nei
> prefab. Spiegami il codice classe per classe.

## Valutazione con la griglia

| Criterio | Dove si vede nel prompt |
|---|---|
| Contesto | versione, pacchetti, sistemi esistenti con i metodi da usare, CLAUDE.md allegato |
| Obiettivo e struttura | classi, ereditarietà (`Pickup` astratta), responsabilità dello spawner |
| Vincoli | pooling, `[SerializeField]`, niente allocazioni, niente ricerche in `Update` |
| Criteri di accettazione | quattro condizioni verificabili in Play Mode |
| Trappole prevenute | doppio trigger nello stesso frame, stato del pool, API deprecate, posizioni non valide |

Il codice ottenuto è in `Assets/_Project/Scripts/Pickups` (`Pickup`, `HealPickup`, `RepairPickup`, `PickupSpawner`).
