Relaciones:
Address
1:1 -> Client
1:N -> Order

Client
1:1 -> Address 
N:1 -> Order

Order
1:1 -> Address 
1:N -> Client
N:N -> Product

Product
N:N -> Order