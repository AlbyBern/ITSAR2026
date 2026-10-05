# Macchina a stati dei nemici

```mermaid
stateDiagram-v2
    [*] --> SEEK: spawn dal pool (Warp)
    SEEK --> CHASE: player entro AggroRange\n(non per i Brute)
    SEEK --> ATTACK: bersaglio principale a portata
    CHASE --> ATTACK: player a portata
    CHASE --> SEEK: player oltre AggroRange × 1,6\n(non per i Runner)
    ATTACK --> SEEK: bersaglio morto o lontano\n(se arrivava da SEEK)
    ATTACK --> CHASE: player lontano\n(se arrivava da CHASE)
    SEEK --> DEAD: vita a zero
    CHASE --> DEAD: vita a zero
    ATTACK --> DEAD: vita a zero
    DEAD --> [*]: Pool.Despawn
```

| Stato | Enter | Tick | Exit |
|---|---|---|---|
| SEEK | sceglie il bersaglio in base a `EnemyData.Priority` | `SetDestination` ogni 0,25 s; controlla player e portata | – |
| CHASE | riattiva l'agente | `SetDestination` verso il player ogni 0,25 s | – |
| ATTACK | ferma l'agente | si gira verso il bersaglio, colpisce ogni `AttackInterval` | riattiva l'agente |
| DEAD | ferma l'agente | si rimpicciolisce, poi torna nel pool | – |

Bersaglio principale per tipo:

| Nemico | Priorità | Comportamento |
|---|---|---|
| Grunt | Crystal | va al cristallo, insegue il player se vicino |
| Runner | Player | insegue sempre il player |
| Brute | Towers | attacca la torre più vicina, poi il cristallo; ignora il player |
