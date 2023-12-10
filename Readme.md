# Final herramientas de programación

Gestion de **ordenes**, **clientes**, **productos**


## Comenzando 🚀

En el proyecto se encontrarán 4 secciones principales y una de administracción.

**Ordenes:** Desde esta seccíon se podrá generar nuevas ordenes, revisar y agregar productos a las ordenes ya existes, y eliminarlas en caso de ser necesario.

**Clientes:** El sector comercial podrá cargar nuevos clientes y gestionar su información. 

**Direcciones:** En relación a la sección anterior, el sector comercial podrá cargar administrar las direcciones de los clientes asi como crear nuevas en los casos que asi se requiera. 

**Productos:** Solo disponible para el sector de deposito, esta sección está diseñada para que se pueda cargar y administrar tanto productos como stock de los mismos.

**Administradores:** Los administradores podran acceder a la gestion de roles y usuarios para permitir a los nuevos usuarios los accesos a direfentes secciones.


### Accesos 📋

Se dispone de usuarios ya configurados para verificar accesos y funcionalidades:

Sin Roles:
```
Usuario: SinRol@mail.com
Contraseña: SinRol.1s

No tendrá acceso a las funcionalidades de la web.
```
Deposito: 
```
Usuario: Deposito@mail.com
Contraseña: Deposito.1

Tendrá acceso al CRUD de productos.
```
Comercial:
```
Usuario: Comercial@mail.com
Contraseña: Comercial.1

Tendrá acceso al CRUD de clientes, al CRUD de direcciones y a la gestion de ordenes.
```
Administración:
```
Usuario: Administrador@mail.com
Contraseña: Administrador.1

Tendrá acceso total a la web.
```
<br>

### Relaciones 🔧

_Detalle de las relaciones de los modelos_

Address
```
1:1 -> Client
1:N -> Order
```
Client
```
1:1 -> Address 
N:1 -> Order
```
Order
```
1:N -> Address 
1:N -> Client
N:N -> Product
N:1 -> StockMovement
```
Product
```
N:N -> Order
N:1 -> StockMovement
```
StockMovement
```
1:N -> Order
1:n -> Product
```



<br>

## Detalle de funcionalidades 📖

### Index
```
• Las funcionalidades mostradas dependerán de los roles asignados al usuario conectado.
• La sección de Órdenes solo estará disponible si hay al menos un producto y cliente cargado.
• La sección de Direcciones solo estará disponible si hay al menos un cliente cargado.
```
<br>

### Seccion Productos
```
• Dispondrá de un buscador para localizar los productos.
• Podrá verificar los datos de los productos como su precio o su stock.
• Podrá dar de alta un producto, gestionar su stock y eliminarlo.

_Importante: No se puede eliminar un producto si tiene alguna orden asociada._
```
<br>

### Seccion Clientes 
```
• Dispondrá de un buscador para localizar los clientes.
• Podrá dar de alta un cliente, gestionar sus datos y dirección o eliminarlo.
• Cada cliente tendrá una dirección unica.

_Importante: La eliminación de un cliente no es posible si tiene alguna orden asociada._
```

<br>

### Seccion Direcciones
```
• Dispondrá de un buscador para localizar las direcciones.
• Podrá dar de alta una direccion siempre y cuando haya algun cliente disponible para el proceso.
• Podrá modificar o eliminar la direccion asociada a un cliente.
• En caso de eliminar una dirección, el cliente está disponible para cargar una nueva. 

_Importante: No se puede eliminar una dirección si el cliente asociado tiene alguna orden pendiente._
```
<br>

### Seccion Ordenes
```
Dispondrá de un buscador para localizar las ordenes.
• Al generar una nueva orden se mostrarán solo los productos con stock disponible y clientes con dirección cargada.
• La cantidad maxima ingresable para cada producto coincidirá con el stock del mismo.
• Está disponible la edición de la orden para cambiar la fecha, cliente.
• Desde edición se podrá modificar la cantidda solicitada de cada producto.
• La cantidad disponible de los productos será igual a su stock actual + la cantidad previamente asignada en la orden.
• Al generar una orden o editarla, disminuirá el stock disponible del producto seleccionado.
• Los detalles de las ordenes incluirán los datos del cliente asi como los productos y cantidades.

```
<br>

## Tecnologías Principales 🔩

ASP.NET Core 7.0

Entity Framework Core

Identity Framework

### Configuración y Servicios ⌨️

Dependency Injection

DbContext

Identity Services

**Scoped Services:** ClientService, AddressService, ProductService, OrderService, HomeService

### Paquetes y Dependencias 📦

Microsoft.AspNetCore.**Identity.EntityFrameworkCore**

Microsoft.AspNetCore.**Identity.UI**

Microsoft.EntityFrameworkCore.**Design**

Microsoft.EntityFrameworkCore.**SQLite**

Microsoft.EntityFrameworkCore.**Tools**

Microsoft.VisualStudio.Web.**CodeGeneration.Design**




