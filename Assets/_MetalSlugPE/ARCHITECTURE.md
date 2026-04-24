# MetalSlugPE - Arquitectura Base

Este documento define la estructura objetivo y el plan incremental para construir el juego con enfoque **code first**: primero sistemas y flujo jugable, luego assets finales.

## Principios

- Todo contenido propio vive en `Assets/_MetalSlugPE/`.
- Todo contenido de terceros vive en `Assets/ThirdParty/`.
- Cada sistema se divide por responsabilidad (input, movimiento, combate, UI, datos).
- Datos de tuning y configuracion via ScriptableObjects.
- Sistemas desacoplados con eventos (`EventBus` o `UnityEvent`) cuando aplique.

## Estructura objetivo

```text
Assets/
  _MetalSlugPE/
    Art/
      Sprites/
        Characters/
        Environment/
        Weapons/
        VFX/
        UI/
      Animations/
        Player/
        Enemies/
      Tilemaps/
    Audio/
      BGM/
      SFX/
    Prefabs/
      Player/
      Enemies/
      Weapons/
      Level/
      UI/
      VFX/
    Scenes/
      Boot.unity
      MainMenu.unity
      MainGameplay.unity
      Level_02_Ciudad.unity
      GameOver.unity
    Scripts/
      Core/
      Player/
      Enemies/
      Weapons/
      UI/
      Data/
      Utils/
    Settings/
    Sandbox/
```

## Estado actual (ya aplicado)

- Se migro todo el contenido jugable desde `Assets/_Project/` a `Assets/_MetalSlugPE/`.
- `Assets/_Project/` se elimino para evitar duplicidades y deuda tecnica.
- Scripts en `Assets/_MetalSlugPE/Scripts/`:
  - `Player/PlayerController.cs`
  - `Weapons/Bullet.cs`
  - `Enemies/EnemyPatrol.cs`
  - `Enemies/EnemyDamage.cs`
- Prefabs en `Assets/_MetalSlugPE/Prefabs/`:
  - `Player/Player.prefab`
  - `Weapons/Bala.prefab`
- Escenas en `Assets/_MetalSlugPE/Scenes/` y pruebas en `Assets/_MetalSlugPE/Sandbox/Scenes/`.
  - Escena principal actual: `MainGameplay.unity`
- Se mantuvieron GUID de assets y scripts movidos para no romper referencias.
- Se definieron namespaces base:
  - `MetalSlugPE.Player`
  - `MetalSlugPE.Weapons`
  - `MetalSlugPE.Enemies`

## Convenciones de codigo

- Namespace por carpeta: `MetalSlugPE.<Modulo>`.
- Nombres por concepto, no por accion:
  - Correcto: `EnemyChaseState`
  - Evitar: `MoveToPlayer`
- `MonoBehaviour` pequenos y enfocados.
- Evitar clases gigantes con demasiadas responsabilidades.

## Arquitectura de gameplay propuesta

- `PlayerController`: coordinador de componentes del jugador.
- `PlayerMovement`, `PlayerShoot`, `PlayerHealth`: componentes especializados.
- `EnemyAI`: maquina de estados (`Patrol`, `Chase`, `Attack`, `Die`).
- `WeaponSystem`: datos en `SO_WeaponData` + pool de proyectiles.
- `GameManager` (Core): flujo global de partida.
- `UIManager`/HUD escuchando eventos de gameplay.

## Plan incremental

1. **Fundacion**
   - Separar `PlayerController` en componentes (`Movement`, `Shoot`, `Health`).
   - Crear `Core/GameManager`, `Core/SceneLoader` y `Utils/EventBus` basico.
2. **Combate base**
   - Introducir `IDamageable` y pipeline de dano comun.
   - Reemplazar destruccion directa por Object Pool para proyectiles.
3. **IA escalable**
   - Migrar `EnemyPatrol` a maquina de estados modular.
4. **Data-driven**
   - Crear `ScriptableObjects` para armas y enemigos.
5. **Loop jugable**
   - Checkpoints, HUD, game over, transicion de escenas.

## Nivel blockout actual

- El suelo unico del nivel lo maneja el `Tilemap` (sin GameObject `Suelo` adicional).
- El tilemap tiene `TilemapCollider2D + CompositeCollider2D` como colision principal.
- La extension horizontal de suelo se genera en runtime con:
  - `MetalSlugPE.World.InfiniteGroundTilemap`
  - archivo: `Assets/_MetalSlugPE/Scripts/World/InfiniteGroundTilemap.cs`

## Regla de sandbox

`Assets/_MetalSlugPE/Sandbox/` es para pruebas rapidas. Nada de sandbox debe quedar referenciado en escenas de build final.
