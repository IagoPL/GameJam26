# Inpacto Arena — Documento de Reglas Base

## 1. Resumen del juego

**Inpacto Arena** es un juego de combate local **1 vs 1** inspirado en la velocidad y tensión de juegos como *Nidhogg*, pero adaptado a una arena cerrada del tamaño de una pantalla.

Cada jugador controla a un personaje en una arena pequeña y debe derrotar al rival antes de que termine el tiempo. El combate es rápido, directo y se basa en aprovechar las zonas vulnerables del enemigo, llamadas **inpactos**.

El objetivo principal es crear una experiencia de combate frenética, fácil de entender y divertida en partidas cortas.

---

## 2. Género

- Juego de lucha 2D.
- Combate local 1 vs 1.
- Arena cerrada.
- Partidas rápidas.
- Estilo arcade.

---

## 3. Número de jugadores

El juego está pensado para:

- **2 jugadores locales**.
- Ambos jugadores comparten el mismo teclado o usan mandos si se implementa soporte.

No habrá modo online en la versión de la JAM.

---

## 4. Objetivo de la partida

El objetivo de cada jugador es reducir la vida del rival a **0** antes de que termine el tiempo.

Cada jugador empieza con:

```text
5 puntos de vida
```

La partida tiene una duración máxima de:

```text
90 segundos
```

Si el tiempo llega a 0 y ningún jugador ha sido derrotado, la partida termina en **empate**.

---

## 5. Condiciones de victoria

La partida puede terminar de tres formas:

### Victoria del Jugador 1

El Jugador 1 gana si la vida del Jugador 2 llega a 0.

```text
Vida Jugador 2 <= 0 → Gana Jugador 1
```

### Victoria del Jugador 2

El Jugador 2 gana si la vida del Jugador 1 llega a 0.

```text
Vida Jugador 1 <= 0 → Gana Jugador 2
```

### Empate

Si el temporizador llega a 0 y ambos jugadores siguen con vida, la partida termina en empate.

```text
Tiempo = 0 → Empate
```

---

## 6. Sistema de vida

Cada jugador empieza con:

```text
Vida inicial: 5
```

La vida se reduce al recibir ataques.

Tipos de daño:

| Tipo de golpe | Daño |
|---|---:|
| Golpe normal | 1 |
| Inpacto | 2 |

La vida nunca debería mostrarse por debajo de 0 en la interfaz.

Ejemplo:

```text
Jugador con 5 vidas recibe golpe normal → baja a 4
Jugador con 4 vidas recibe inpacto → baja a 2
Jugador con 2 vidas recibe inpacto → baja a 0 y pierde
```

---

## 7. Sistema de combate

El combate se basa en ataques cuerpo a cuerpo rápidos.

Cada jugador puede realizar un ataque en la dirección en la que está mirando. El ataque genera una zona de impacto temporal delante del personaje.

### Reglas del ataque

- El ataque tiene corto alcance.
- El ataque solo golpea durante una pequeña ventana de tiempo.
- El ataque debe tener un pequeño cooldown.
- Un mismo ataque no debería aplicar daño varias veces al mismo rival.
- Si el ataque golpea una zona vulnerable, se considera **inpacto**.

---

## 8. Tipos de golpe

### Golpe normal

Un golpe normal ocurre cuando el jugador acierta al rival sin golpear una zona vulnerable.

```text
Daño: 1
```

Debe usarse como daño básico del juego.

---

### Inpacto

Un **inpacto** ocurre cuando el jugador golpea una zona vulnerable del rival.

En la versión base de la JAM, las zonas de inpacto son fijas:

```text
Cabeza
Espalda
```

Un inpacto hace más daño que un golpe normal.

```text
Daño: 2
```

Los inpactos deben sentirse más importantes que los golpes normales, por lo que se recomienda añadir:

- Feedback visual.
- Sonido más fuerte.
- Pequeño texto en pantalla.
- Knockback superior si se implementa.

---

## 9. Zonas de inpacto

### Espalda

La espalda es una zona vulnerable fija.

Un golpe cuenta como inpacto por espalda si el atacante golpea al rival desde detrás.

Regla general:

```text
Si el atacante está detrás del rival y lo golpea → Inpacto
```

Para detectar esto, se puede usar la dirección en la que mira el jugador defensor.

Ejemplo:

```text
Jugador 2 mira hacia la derecha.
Jugador 1 está a la izquierda de Jugador 2 y le golpea.
Ese golpe impacta por la espalda.
Resultado: 2 de daño.
```

---

### Cabeza

La cabeza es otra zona vulnerable fija.

Un golpe cuenta como inpacto en cabeza si el ataque colisiona con la zona superior del personaje rival.

Regla general:

```text
Si el hitbox de ataque toca la zona de cabeza del rival → Inpacto
```

En caso de que detectar la cabeza complique demasiado el desarrollo, esta mecánica puede dejarse como secundaria y priorizar el inpacto por espalda.

---

## 10. Prioridad de detección de daño

Si un golpe puede contar como golpe normal e inpacto al mismo tiempo, siempre debe tener prioridad el inpacto.

Orden recomendado:

```text
1. Comprobar si es inpacto por espalda.
2. Comprobar si es inpacto en cabeza.
3. Si no es inpacto, aplicar golpe normal.
```

Resultado:

```text
Inpacto → 2 daño
No inpacto → 1 daño
```

---

## 11. Movimiento

Cada jugador debe poder moverse horizontalmente dentro de la arena.

Movimiento mínimo para la versión base:

- Moverse a la izquierda.
- Moverse a la derecha.
- Girarse según la dirección de movimiento.
- Atacar hacia la dirección en la que mira.

El salto es opcional para la versión de la JAM. Si se implementa, debe ser simple y no debe retrasar el combate base.

---

## 12. Arena

El mapa será una arena cerrada del tamaño de la pantalla.

Características:

- Sin scroll.
- Sin cambio de cámara.
- Sin plataformas complejas en el MVP.
- Límites laterales para evitar que los jugadores salgan del escenario.
- Espacio suficiente para que los jugadores puedan esquivar, girarse y atacar.

La arena debe estar pensada para generar enfrentamientos constantes y rápidos.

---

## 13. Temporizador

Cada partida dura como máximo:

```text
90 segundos
```

El temporizador debe mostrarse en pantalla.

Cuando llega a 0:

```text
Si ambos jugadores siguen vivos → Empate
```

El temporizador debe detenerse cuando la partida termina por victoria o empate.

---

## 14. Controles propuestos

### Jugador 1

| Acción | Tecla |
|---|---|
| Mover izquierda | A |
| Mover derecha | D |
| Saltar | W |
| Atacar | F |

### Jugador 2

| Acción | Tecla |
|---|---|
| Mover izquierda | Flecha izquierda |
| Mover derecha | Flecha derecha |
| Saltar | Flecha arriba |
| Atacar | K |

Si se decide no implementar salto, se eliminará la acción de saltar del esquema de controles.

---

## 15. Flujo de partida

El flujo base del juego será:

```text
Pantalla inicial
↓
Inicio de partida
↓
Combate 1 vs 1
↓
Victoria de un jugador o empate por tiempo
↓
Pantalla de resultado
↓
Reiniciar partida
```

---

## 16. Pantalla inicial

La pantalla inicial debe mostrar como mínimo:

- Nombre del juego.
- Botón o tecla para empezar.
- Controles básicos.
- Explicación breve de la regla principal.

Ejemplo de explicación:

```text
Golpea al rival para quitarle vida.
Ataca la cabeza o la espalda para hacer un INPACTO y causar 2 de daño.
Gana quien reduzca la vida rival a 0 antes de 90 segundos.
```

---

## 17. Pantalla de resultado

Al terminar la partida, se debe mostrar:

- Ganador, si hay uno.
- Empate, si termina el tiempo.
- Opción para reiniciar.

Ejemplos:

```text
Gana Jugador 1
Gana Jugador 2
Empate
Pulsa R para reiniciar
```

---

## 18. Feedback visual y sonoro

El jugador debe entender claramente cuándo ha golpeado y qué tipo de golpe ha realizado.

### Golpe normal

Feedback recomendado:

- Pequeño flash.
- Sonido de golpe normal.
- Pequeño retroceso del rival.

### Inpacto

Feedback recomendado:

- Texto grande: `INPACTO`.
- Sonido más potente.
- Flash más llamativo.
- Knockback más fuerte.
- Pausa muy breve opcional para dar sensación de impacto.

---

## 19. Reglas de balance inicial

Valores recomendados para la primera versión:

| Parámetro | Valor inicial |
|---|---:|
| Vida inicial | 5 |
| Daño normal | 1 |
| Daño inpacto | 2 |
| Duración partida | 90 segundos |
| Cooldown ataque | 0.4s - 0.6s |
| Duración hitbox ataque | 0.2s - 0.3s |
| Invulnerabilidad tras recibir daño | 0.2s - 0.4s |

Estos valores pueden ajustarse durante el testing.

---

## 20. Reglas de simplificación para la JAM

Como el proyecto se desarrolla en 24 horas, se establecen las siguientes restricciones:

### No incluido en el MVP

- Modo online.
- IA enemiga.
- Selector de personajes.
- Más de un mapa.
- Sistema complejo de armas.
- Campaña.
- Guardado de progreso.
- Matchmaking.
- Inpactos aleatorios antes de tener el MVP completo.

### Posibles extras si sobra tiempo

- Inpactos aleatorios por partida.
- Música.
- Mejoras visuales.
- Animaciones más pulidas.
- Partidas al mejor de 3.
- Compatibilidad con mando.
- Más efectos de partículas.
- Mejor pantalla de inicio.

---

## 21. Criterio de MVP terminado

El MVP se considera terminado cuando se cumple lo siguiente:

```text
[ ] Hay una arena cerrada.
[ ] Hay dos jugadores en pantalla.
[ ] Ambos jugadores pueden moverse.
[ ] Ambos jugadores pueden atacar.
[ ] Los ataques normales hacen 1 de daño.
[ ] Los inpactos hacen 2 de daño.
[ ] Cada jugador empieza con 5 vidas.
[ ] La vida se muestra en pantalla.
[ ] Hay temporizador de 90 segundos.
[ ] El juego detecta victoria por vida a 0.
[ ] El juego detecta empate por tiempo.
[ ] Se puede reiniciar la partida.
```

Cuando todo esto funcione, el juego ya es entregable.

---

## 22. Criterio de partida divertida

Una partida se considera suficientemente divertida si:

- El combate es rápido.
- Los jugadores entienden cuándo reciben daño.
- Los inpactos se sienten más potentes.
- Hay posibilidad de remontar.
- El tiempo de 90 segundos no se siente demasiado largo.
- Las partidas invitan a repetir.

---

## 23. Descripción corta para la página de la JAM

**Inpacto Arena** es un juego de combate local 1 vs 1 en una arena cerrada. Cada jugador tiene 5 vidas y debe derrotar al rival antes de que pasen 90 segundos. Los golpes normales hacen 1 punto de daño, pero atacar la cabeza o la espalda provoca un **INPACTO**, causando 2 puntos de daño y cambiando el ritmo del combate.

---

## 24. Frase de presentación

```text
Golpea rápido, gira antes de que te alcancen y busca el INPACTO perfecto.
```

---

## 25. Decisiones cerradas

Para evitar cambios de alcance durante la JAM, se consideran cerradas estas decisiones:

```text
El juego será 1 vs 1 local.
La arena será de una sola pantalla.
Cada jugador tendrá 5 vidas.
La partida durará 90 segundos.
El daño normal será de 1.
El daño por inpacto será de 2.
Los inpactos base serán cabeza y espalda.
El empate ocurre si se acaba el tiempo.
Los extras solo se harán cuando el MVP esté terminado.
```

---

## 26. Notas para el equipo

Durante el desarrollo, se recomienda seguir estas reglas:

- Priorizar siempre que el juego sea jugable.
- No añadir extras si el MVP no está terminado.
- Si una tarea se bloquea más de 20-30 minutos, simplificarla.
- Hacer pruebas reales entre jugadores lo antes posible.
- Crear una build funcional antes de la recta final.
- No dejar la entrega para el último momento.

---
