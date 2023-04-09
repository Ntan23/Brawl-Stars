# Brawl-Stars
 
Brawlz is a simple local multiplayer game where player 1 become a thrower and player 2 become a shooter.

Optimization : 

I use coroutine instead of invoke.
I use object pooling for the bullet
I use CompareTag instead of tag with string

SOLID Principles : 

I use SRP for all almost all the code except for "Bullet.cs" and "AttackTrail.cs". 
In "Bullet.cs" and "AttackTrail.cs" I use ISP.
