# Route Simulator

O projeto Route Simulator é responsável por encontrar a rota com o menor preço entre 2 localizações e também disponibiliza a opção de cadastro de novas rotas.
A solução está segregada em 4 projetos:
1) route-simulator: 
Projeto Core da aplicação, segregado em Domain, Infra e Services.
2) route-simulator.consoleapp: 
Aplicação Console que permite a leitura de um arquivo de entrada e obtém a melhor rota entre 2 localizações;
3) route-simulator.webapi: 
Aplicação WebAPI que permite as opções de obter a melhor rota entre 2 localizações e cadastrar uma nova rota.
4) route-simulator.test: 
Aplicação de Testes Unitários.

Toda a solução foi desenvolvida utilizando .Net Core 3.1.

### Pré-requisitos

Considerando que o .Net Core é cross platform, fique a vontade para escolher qualquer sistema operacional para rodar a aplicação.
 
Para executar a aplicação, por favor instale  [.Net Core 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1) 

---
### Aplicação Console

A aplicação necessita de um arquivo de input dentro do diretório ***sdtin*** para realizar as operações. Foi disponibilizado um exemplo no diretório, sinta-se a vontade para usar o de sua preferência.

Instruções para rodar a aplicação:

```
./run-consoleapp.sh input-routes.csv
```

### Aplicação WebAPI

Foram disponibilizados 2 endpoints para a aplicação:
#### 1) Consulta da melhor rota
Endpoint: 
```
/api/route/{origin}/{destination} [GET]
```
Parâmetros de request (header):
```
{origin} = Aeroporto de origem
{destination} = Aeroporto de destino
```
JSON de response:
```
{
  "TotalCost": {valor total da rota},
  "RoutePathDescription": "{caminho total da rota}"
}
```

#### 2) Cadastro de nova rota
Endpoint: 
```
/api/route [POST]
```
Parâmetros de request (body):
```
{
	"originAirport": "{aeroporto de origem}",
	"destinationAirport": "{aeroporto destino}",
	"flightPrice": {preço do voo}
}
```
JSON de response:
```
{
  "TotalCost": {valor total da rota},
  "RoutePathDescription": {caminho total da rota}
}
```

Instruções para rodar a aplicação:
```
./run-webapi.sh 
```
* A API será inicializada nas portas 5000 (HTTP) e 5001 (HTTPS).

Por padrão, a aplicação está configurada para ler o arquivo com nome ***input-routes.csv***, dentro do diretório ***sdtin***.
Caso necessite utilizar um arquivo com nome diferente, por favor siga os seguintes passos:
* Disponibilize o novo arquivo no diretório ***sdtin***;
* Altere a propriedade ***FileName*** no arquivo ***src/route-simulator.webapi/appsettings.Development.json*** com o nome do novo arquivo a ser utilizado.

### Rodando os testes

Dentro da solução foi disponibilizado um projeto de Testes Unitários.

Instruções para rodar os testes:
```
./test.sh
```
### Arquitetura

A arquitetura da aplicação foi baseada no modelo de arquitetura hexagonal, onde a manipulação de dados e o domínio estão isolados para manter o software com fácil manutenação e extensível. Procurei utilizar o padrão de repository interface como portas para o tratamento dos arquivos.
Para implementação da busca de rotas com o menor preço, foi implementada uma lógica com base no algorítimo de Dijkstra.


##### Desenvolvido com:

* [.Net Core](https://dotnet.microsoft.com/download) - Application framework
* [Nuget](https://www.nuget.org) - Dependency Management
* [Xunit](https://xunit.net/)  - Test Framework