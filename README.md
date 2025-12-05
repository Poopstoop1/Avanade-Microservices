# 🛍️ Desafio Técnico - Microserviços E-commerce

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat&logo=.net)](https://dotnet.microsoft.com/)
[![SQL Server](https://img.shields.io/badge/Microsoft_SQL_Server-17.x-blue?logo=microsoftsqlserver&logoColor=white)](https://www.microsoft.com/pt-br/sql-server)
[![RabbitMQ](https://img.shields.io/badge/RabbitMQ-3.13-FF6600?style=flat&logo=rabbitmq&logoColor=white)](https://www.rabbitmq.com/)
[![Docker](https://img.shields.io/badge/Docker-Container-2496ED?style=flat&logo=docker&logoColor=white)](https://www.docker.com/)

---

## 📋 Índice

- [1. 📖 Sobre o Projeto](#sobre) </br>
- [2. 🔧 Tecnologias usadas](#tecnologias)
- [3. 🚀 Como Executar](#como-executar)
- [4. 🏗️ Arquitetura](#arquitetura)
- [5. 🧩 Componentes](#componentes)
- [6. 🗄️ Banco de Dados](#banco-de-dados)
- [7. 🔌 API Endpoints](#api-endpoints)
- [8. 🐰 RabbitMQ](#rabbitmq)
- [9. 📁 Estrutura do Projeto](#estrutura)
- [10. 📚 Documentação Adicional](#documentacao)

<a id="sobre"></a>

## ​1. 📖 Sobre o Projeto

Projeto foi construído para um Desafio técnico da Avanade.
Trata-se de um Microserviços de Sistema de Estoque e Vendas com API Gateway. Esse projeto foi construído com as melhores práticas de arquitetura moderna utilizando Clean Architecture, CQRS, DDD e EDA.

<a id="tecnologias"></a>

## 2. 🔧 Tecnologias usadas

- **Backend Framework**:

  - `.NET (8.0)`: Plataforma utilizada para construir toda a aplicação backend, aproveitando os recursos modernos da linguagem e do runtime.
  - `ASP.NET Core`: Framework utilizado para criação das APIs, permitindo alto desempenho, modularidade e extensibilidade.
  - `Entity Framework Core`: ORM utilizado para facilitar o mapeamento objeto-relacional e abstrair interações com o banco de dados.
  - `EF Core Tools & EF Core Design`: Ferramentas usadas para gerenciar migrações, scaffolding, design-time e outras funcionalidades do Entity Framework.
  - `MediatR`: Biblioteca utilizada para implementar o padrão CQRS, promovendo desacoplamento entre camadas e facilitando manutenção e testes.

- **Banco de Dados**:

  - `SQL Server`: Banco de dados relacional utilizado para persistência das informações de estoque, vendas e identidade.

- **Mensageria**:

  - `RabbitMQ 3.13`: Utilizado como broker de mensagens para comunicação assíncrona entre os microserviços, garantindo desacoplamento, resiliência e processamento orientado a eventos.

- **Authenticação e Segurança**

  - `JWT Bearer Authentication`: Utilizado para autenticação e autorização baseada em tokens de forma segura e escalável.
  - `PasswordHasher`: Recomendado pela Microsoft, É Utilizado para realizar o hashing seguro das senhas de usuários, seguindo boas práticas de segurança adotadas pelo ASP.NET Core Identity.

- **API Gateway**

  - `YARP (Reverse Proxy)`: Utilizado no API Gateway para roteamento, balanceamento e agregação de chamadas aos microserviços.

- **Logs & Monitoramento**

- **Documentação & Testes**

  - `Swagger / OpenAPI`: Utilizado para documentação e testes interativos das APIs durante o desenvolvimento.

- **Containerização**

  - `Docker` - Containerização
  - `Docker Compose` - Orquestração local para RabbitMQ e SQLServer

- **Em Progresso**

  - `xUnit`: Framework de testes utilizado para implementação dos testes unitários do domínio e da aplicação.

  - `Moq` - Mocking para testes

<a id="como-executar"></a>

## 3. 🚀 Como Executar

### 🔧 Pré-requisitos

Certifique-se de ter instalado:  
✅ [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)  
✅ [Docker Desktop](https://www.docker.com/products/docker-desktop) (recomendado)

```bash
# 1 Clone o repositório

git clone <repository-url>
cd Avanade-Microservices

# 2. Inicie os containers (SQL Server + RabbitMQ)

Certifique de estar com o Docker aberto
docker-compose up -d

# 3. Execute o build de inicialização completa

Na pasta Avanade-Microservices
dotnet build


# 4. Para start a aplicação

- Certifique-se de que o SQL Server no docker está rodando e o RabbitMQ
- Não esqueça também que para startar é preciso entrar na pasta API de cada serviço e startar
- Ao startar o projeto vai fazer a atualização das migrations automaticamente.
  cd <Em uma pasta>/api
  abra outro terminal
  cd <EmOutraPasta>/api
  abra outro terminal
  cd <EmOutraPasta>/api
```

<a id="arquitetura"></a>

## 4. 🏗️ Arquitetura

### Diagrama da Arquitetura

```mermaid
graph TB
    Cliente[👤 Usuarios]
    Gateway[🌐 API Gateway]

    Estoque[📦 Estoque Service<br/>Consumers:<br/>- pedido-criado<br/>- pedido.confirmado<br/><br/>Producers:<br/>- estoque_pedido_reservado<br/>- estoque_pedido_confirmado<br/>- estoque-indisponivel]

    Vendas[🛒 Vendas Service<br/>Producers:<br/>- pedido-criado<br/>- pedido.confirmado<br/>- pedido_cancelado<br/><br/>Consumers:<br/>-estoque-indisponivel]

    subgraph "📬 RabbitMQ"
        queue1[(pedido-criado)]
        queue2[(pedido.confirmado)]
        queue3[(pedido_cancelado)]
        queue4[(estoque_pedido_reservado)]
        queue5[(estoque_pedido_confirmado)]
        queue6[(estoque-indisponivel)]
    end

    Cliente --> Gateway
    Gateway --> Vendas
    Gateway --> Estoque

    %% Producers -> Queues
    Vendas --> queue1
    Vendas --> queue2
    Vendas --> queue3

    Estoque --> queue4
    Estoque --> queue5
    Estoque --> queue6

    %% Consumers <- Queues
    queue1 --> Estoque
    queue2 --> Estoque
    queue6 --> Vendas
```

---

### Fluxo de Comunicação

**📱 Usuarios(Clientes)** → Autentica via **API Gateway**
**🌐 API Gateway(Yarp)** → Roteia requisições para microserviços
**🔄 Comunicação assíncrona** Comunicação Assíncrona entre serviços via RabbitMQ

1️⃣ Fluxo: Criação do Pedido

**🛒 Vendas Service**

- Cria o pedido inicial

- Publica o evento pedido-criado no RabbitMQ

**📦 Estoque Service**

- Consome o evento pedido-criado

- Valida disponibilidade de estoque

- Atualiza QuantidadeReservada

- Se o estoque for insuficiente, publica estoque-indisponivel

**🛒 Vendas Service**

- Consome estoque-indisponivel

- Atualiza o status do pedido para "cancelado"

2️⃣ Fluxo: Confirmação do Pedido

🛒 Vendas Service

Após validação bem-sucedida do estoque

Atualiza o pedido para status Confirmado

Publica o evento pedido-confirmado

📦 Estoque Service

Consome pedido-confirmado

Atualiza o estoque final (baixa definitiva)

<a id="componentes"></a>

## 5. 🧩 Componentes

### 🌐 API Gateway

- **Responsabilidade**: Ponto de entrada único, autenticação, roteamento
- **Porta**: 5035
- **Funcionalidades**:
  ✅ Autenticação JWT
  ✅ Crud de Usuarios
  ✅ Roteamento via YARP
  ✅ Swagger UI
  ✅ Swagger com multiplos documentos

### 📦 Estoque Service

- **Responsabilidade**: Gerenciamento de produtos e estoque
- **Porta**: 5285
- **Funcionalidades**:
  ✅ CRUD de produtos
  ✅ Controle de estoque
  ✅ Validação de disponibilidade
  ✅ Publicação de Eventos RabbitMQ
  ✅ Consumo de mensagens RabbitMQ
  ✅ Swagger

- **Em progresso e em estudo**
  ✅ Logs estruturados (Serilog)
  ✅ Testes

### 🛒 Vendas Service

- **Responsabilidade**: Gerenciamento de vendas e pedidos
- **Porta**: 5156
- **Funcionalidades**:
  ✅ CRUD de pedidos
  ✅ Cálculo de totais
  ✅ Publicação de eventos RabbitMQ
  ✅ Consumo de mensagens RabbitMQ
  ✅ Swagger

- **Em progresso e em estudo**
  ✅ Logs estruturados (Serilog)
  ✅ Testes

### 🌟 Verificação da Instalação

Após a execução, verifique se os serviços estão rodando:

| Serviço                    | URL                                                               | Status |
| -------------------------- | ----------------------------------------------------------------- | ------ |
| � **Gateway**              | http://localhost:5038/swagger                                     | ✅     |
| � **Gateway**              | http://localhost:5038/swagger/index.html?urls.primaryName=Vendas  | ✅     |
| � **Gateway**              | http://localhost:5038/swagger/index.html?urls.primaryName=Estoque | ✅     |
| 📦 **Estoque Service**     | http://localhost:5285/swagger                                     | ✅     |
| 🛒 **Vendas Service**      | http://localhost:5156/swagger                                     | ✅     |
| 🐰 **RabbitMQ Management** | http://localhost:15672                                            | ✅     |

**🔑 Credenciais:**

- **RabbitMQ**: `guest` / `guest`

---

<a id="banco-de-dados"></a>

## 6. 🗄️ Banco de Dados

### 📊 Configuração Geral

#### **Credenciais SQLServer:**

- 🔑 **Servidor** - `localhost,1433`
- 🔑 **Usuário**: `sa`
- 🔐 **Senha**: `YourStrong@2025`
- 🌐 **Host**: `localhost`
- 🔌 **Porta**: `1433`

### 🗃️ Estrutura dos Bancos

#### 🔐 Usuario_DB (API Gateway)

```sql
-- Tabela de usuários para autenticação
Usuarios
├── Id (PK, uniqueIdentifier, not null)
├── Username (nvarchar(200), not null)
├── Email (nvarchar(200), not null)
├── PasswordHash (nvarchar(max), not null)
└── Role (nvarchar(50), not null)
```

#### 📦 Estoque (Estoque Service)

```sql
-- Tabela de produtos
Produtos
├── Id (PK, uniqueidentifier, not null)
├── Name (nvarchar(200))
├── Descricao (nvarchar(200), not null)
├── Preco (decimal(18,2), not null)
├── Quantidade (int, not null)
└── QuantidadeReservada(int, not null)
```

#### 🛒 Vendas_DB (Vendas Service)

```sql
-- Tabela de pedidos
Pedidos
├── Id (PK, uniqueidentifier, not null)
├── UsuarioId (uniqueidentifier, not null)
├── DataCriacao (datatime2(7)), not null)
├── ValorTotal (decimal(18,2), not null)
└── Status (int, not null)

-- Tabela de itens do pedido
PedidoItens
├── Id (PK, uniqueidentifier, not null)
├── PedidoId (PK, uniqueidentifier, not null)
├── ProdutoId (uniqueidentifier, not null)
├── NomeProduto (nvarchar(200), not null)
├── Quantidade (int, not null)
└── PrecoUnitario(decimal(18,2), not null)
```

<a id="api-endpoints"></a>

## 7. 🔌 API Endpoints {#api-endpoints}

### 🔐 Autenticação

Pelo Swagger do Gateway você consegue registrar e fazer login e receber o token para acessar outras rotas

#### POST `/api/usuarios/login`

```json
{
  "email": "admin@hotmail.com",
  "password": "admin123"
}
```

**Resposta:**

```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

#### POST `/api/usuarios/register`

```json
{
  "name": "string",
  "email": "string@hotmail.com",
  "role": "user",
  "password": "string"
}
```

<a id="rabbitmq"></a>

## 8. 🐰 RabbitMQ

### 📋 Configuração

#### **Conexão:**

- 🌐 **Host**: `localhost`
- 🔌 **Porta AMQP**: `15672`
- 🖥️ **Management UI**: `15672`
- 🔑 **Usuário**: `guest`
- 🔐 **Senha**: `guest`

### 🔄 Fluxo de Mensagens

#### **🛒 Cliente cria pedido** → Vendas Service

- **📝 Vendas Service** → Persiste pedido no banco
- **📤 Vendas Service**→ Publica mensagem na fila pedido-criado
- **📥 Estoque Service** → Consome pedido-criado
- **📦 Estoque Service** → Verifica e reserva estoque

#### **❌ Se o estoque estiver indisponível**

- **📤 Estoque Service** → Publica estoque-indisponivel
- **📥 Vendas Service** → Consome estoque-indisponivel
- **🛑 Vendas Service** → Atualiza pedido para “Cancelado”

#### **✔️ Fluxo de Confirmação**

- **🛒 Vendas Service** → Confirma pedido
- **📤 Vendas Service** → Publica pedido.confirmado
- **📥 Estoque Service** → Consome pedido.confirmado
- **📦 Estoque Service** → Baixa estoque definitivo
- **🛒 Vendas Service** → Finaliza o pedido

#### Interface Web:

- **URL**: http://localhost:15672
- **Login**: guest / guest
- Navegue para **Queues** → Selecione fila → **Get messages**

---

<a id="estrutura"></a>

## 9. 📁 Estrutura do Projeto

```
Avanade-Microservices/
│   ├── 🌐 Gateway/                  # API Gateway
│   │   ├── Api/                        # Camada de exposição HTTP da aplicação (Web API), Usuario,
│   │   │   ├── appsettings.json            # Configurações de conexão com o banco, JWT e YARP (API Gateway)
│   │   │   ├── Program.cs                  # Configuração inicial da aplicação
│   │   │   └── Startup.cs                  # Registro de serviços, injeções de dependência e inicialização de componentes
│   ├── 📦 Estoque/                # Serviço de Estoque
│   │   ├── Api/                # WebApi, Controller, Startup, HostedService, DB
│   │   ├── Application/          # Camada Application, Command, Queries, Interface, Consumers, DTOs, DependencyInjection
│   │   ├── Domain/                   # Camada Core, Entity, Event, IRepository, ValueObjects, Exceptions
│   │   ├── Infrastructure/            # Camada Infrastrucure, DBContext, MessageBus, Migrations, Repositories, DependencyInjection
│   │   ├── Tests/            # Em progresso ainda
│   ├── 🛒 Vendas/               # Serviço de Vendas
│   │   ├── Api/                # WebApi, Controller, Startup, HostedService
│   │   ├── Application/          # Camada Application, Command, Queries, Interface, Consumers, DTOs, DependencyInjection
│   │   ├── Domain/                   # Camada Core, Entity, Event, IRepository, Enums, ValueObjects, Exceptions
│   │   ├── Infrastructure/            # Camada Infrastrucure, DBContext,Migrations, MessageBus, Repositories
│   │   ├── Tests/            # Em progresso ainda
├── 📄 docker-compose.yml             # SQL Server + RabbitMQ
├── 📄 Avanade-Microservices.sln     # Solution

```

### 🎯 Arquivos de Configuração Principais

#### appsettings.json (Exemplo - API Gateway)

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "Microsoft.AspNetCore": "Warning"
    }
  },

  "ConnectionStrings": {
    "GatewayConnection": "Server=localhost,1433;Database=Usuario;User ID=sa;Password=YourStrong@2025;TrustServerCertificate=True;"
  },

  "AllowedHosts": "*",

  "JwtSettings": {
    "Secret": "NDlkZTIxZjQtOTg1OC00YzEwLTk3NDctOTYzMjkyODlkNWM3",
    "Issuer": "SeuIssuer",
    "Audience": "SeuAudience",
    "ExpiryMinutes": 60
  },

  "ReverseProxy": {
    "Routes": {
      "estoque_route": {
        "ClusterId": "estoque_cluster",
        "Match": {
          "Path": "/estoque/{**catch-all}"
        },
        "Transforms": [{ "PathRemovePrefix": "/estoque" }]
      },
      "vendas_route": {
        "ClusterId": "vendas_cluster",
        "Match": {
          "Path": "/vendas/{**catch-all}"
        },
        "Transforms": [{ "PathRemovePrefix": "/vendas" }]
      }
    },

    "Clusters": {
      "estoque_cluster": {
        "Destinations": {
          "dest1": {
            "Address": "http://localhost:5285/"
          }
        }
      },
      "vendas_cluster": {
        "Destinations": {
          "dest1": {
            "Address": "http://localhost:5156/"
          }
        }
      }
    }
  }
}
```

<a id="documentacao"></a>

## 10. 📚 Documentação Adicional

Se você tem interesse em contribuir ou se deseja ajudar a melhorar o projeto, fique à vontade para enviar um pull request!
