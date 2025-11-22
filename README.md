# 📘 WorkBalance API — README (Versão Simples e Direta)

## 📌 Visão Geral

WorkBalance é uma API REST desenvolvida em **C# (.NET 8)** seguindo **Clean Architecture**, **DDD**, **Clean Code** e boas práticas modernas.  
Foi criada para a Global Solution da FIAP, realizando:

- CRUD completo de Usuários, Mood Entries e Recommendations  
- Integração com Oracle DB  
- Uso de Stored Procedures no Oracle (PRC_INSERT_USER, etc.)  
- API Versioning (v1 e v2)  
- Swagger totalmente configurado  
- Health Check  
- Testes automatizados com InMemoryDatabase  
- JWT Authentication  

---

## 🏛️ Arquitetura do Projeto

A estrutura segue Clean Architecture:

```
/Workbalance
 ├── Application
 │    ├── Dtos
 │    ├── JWT
 │    ├── Services
 │    └── Swagger
 ├── Controllers
 ├── Domain
 │    ├── Entity
 │    └── Enums
 ├── Hateoas
 ├── Infrastructure
 │    ├── Configurations
 │    ├── Context
 │    └── Repository
 ├── Migrations
 ├── Program.cs
```

---

### 🔹 Camada Domain

Contém as entidades puras:

- User  
- MoodEntry  
- Recommendation  

---

### 🔹 Camada Application

Contém:

- DTOs  
- Regras de serviço (UserServiceV1, V2, etc.)  
- JWT Token Service  
- Configurações do Swagger  

---

### 🔹 Camada Infrastructure

- Oracle EF Context (AppDbContext)  
- Repository Pattern (`Repository<T>`)  
- Execução de Stored Procedures  
- Config de conexão  

---

### 🔹 Camada API (Controllers)

- UserController  
- MoodEntryController  
- RecommendationController  

Cada controller possui rotas versionadas:

```
/api/1.0/users
/api/2.0/users
```

---

## 🗄️ Banco de Dados (Resumo)

O Oracle DB contém:

- Tabelas: `WB_USER`, `WB_MOOD_ENTRY`, `WB_RECOMMENDATION`, `WB_AUDIT_LOG`  
- Triggers de auditoria  
- Package `PKG_WORKBALANCE`  
  - PRC_INSERT_USER  
  - PRC_INSERT_MOOD_ENTRY  
  - PRC_INSERT_RECOMMENDATION  
  - FN_VALIDATE_EMAIL  
  - PRC_EXPORT_JSON (não usado no C#, mas existe)  

A API chama as procedures via:

```
Repository<T>.ExecutarProcedureAsync
```

---

## 🔑 Autenticação JWT

Implementada no `JwtTokenService`.

- Login gera token  
- Token é validado via Middleware  
- Swagger contém configuração para Bearer  

---

## 📘 Documentação — Swagger

Disponível automaticamente ao rodar o projeto:

```
https://localhost:5000/swagger
```

As versões aparecem separadamente:

- WorkBalance API 1.0  
- WorkBalance API 2.0  

---

## ❤️ Health Check

Atende ao requisito da GS:

```
GET /health
```

Retorna:

```json
{ "status": "Healthy" }
```

---

## 🧪 Testes Automatizados

Projeto: **Workbalance.tests**

Contém:

- `HealthCheckTests.cs`
- `UserV1_CreateTests.cs`
- `UserV1_ControllerTests.cs`
- `CustomWebApplicationFactory.cs`  
  - remove Oracle  
  - substitui por InMemoryDatabase  
  - ambiente isolado para testes  

Para executar:

```
dotnet test
```

---

## 🚀 Como Executar o Projeto

### 1. Configure o Oracle no `appsettings.json`

```json
"ConnectionStrings": {
  "Oracle": "User Id=RMxxxxxx;Password=xxxx;Data Source=oracle.fiap.com.br:1521/orcl"
}
```

### 2. Rode o projeto

```
dotnet run
```

API iniciará em:

```
https://localhost:5000
```

---

## 🎥 Link do Vídeo

https://youtu.be/z68CzfWDt88

---

## 🤝 Equipe

| Nome                                  | RM       |
|---------------------------------------|----------|
| Caroline Souza do Amaral              | RM558012 |
| Eduardo Augusto Pelegrino Einsfeldt   | RM556460 |
| Vinicius Souza Carvalho               | RM556089 |

---

## 📄 Licença

Este projeto é entregue exclusivamente para a Global Solution FIAP.
