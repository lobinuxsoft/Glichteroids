![Glichteroids](https://www.dynamite.com/previews/C1524101036/ArtofAtari124125.jpg)
##### imagen de `ART of ATARI` by `Tim Lapetino`
# **`Glichteroids`** a Asteroids clone game
---
## `Introduccion`
> Este solo es un clon de un juego clasico... pero como no me basta hacer un clon voy a buscar la manera de mejorar el juego clasico y darle un nuevo aire, con mecanicas nuevas, mi inspiracion viene de --> [JUICE IT OR LOSE IT](https://www.youtube.com/watch?v=Fy0aCDmgnxg)
---
## *`Mecanicas Base`*

* >✦ El juego arranca con 4 asteroides grandes moviéndose hacia una dirección random de la
pantalla.

* >✦ Cuando un asteroide llega a un extremo de la pantalla reaparece en el otro extremo (no es
necesario que esto suceda progresivamente, puede reubicarse el sprite cuando alcanza un
punto determinado).
* >✦ La nave se controla con las teclas de cursor y la barra espaciadora. Izquierda y Derecha
hacen girar la nave, Arriba le da impulso en la dirección donde se encuentra mirando,
Barra Espaciador dispara.
* >✦ Cuando la nave del jugador llega a un extremo de la pantalla, debe suceder lo mismo que
con los asteroides, se reubica en el extremo opuesto.
* >✦ Cuando un disparo alcanza un asteroide, se otorgan 10 puntos y sucede lo siguiente:

    * >✓ Si el asteroide es grande, se subdivide en dos medianos (desactivar sprite de asteroide
    grande, activar dos medianos y darles impulso random).

    * >✓ Si el asteroide es mediano, se subdivide en dos pequeños (desactivar sprite de
    asteroide mediano, activar dos pequeños y darles impulso random).

    * >✓ Si el asteroide es pequeño, desaparece.

* >✦ Cuando un asteroide toca la nave, si la misma no posee el escudo activado, explota y se
pierde una vida.

* >✦ Los asteroides no colisionan entre sí (se traspasan).

* >✦ El jugador arranca con 3 vidas, al perderla todas, el juego termina.
---
# `Futuros cambios`

* >Pasar todo el proyecto a `Android`, cambio en las mecanicas de movimiento para que responda a en pantalla tactil.

* >Agregar efectos visuales, que respondan a la musica de fondo.
* >Pasar a modelos `3D` simples, no demasiados complejos, una nave `modular` a la que se le puedan agregar nuevo armamento.
* >Plan "muy a futuro", implementar version `AR` y `VR`.