# Como Fazer o Deploy de uma API na Nuvem na Prática

![.NET](https://img.shields.io/badge/.NET-8-512BD4?logo=dotnet&logoColor=white)
![Azure](https://img.shields.io/badge/deploy-Azure_App_Service-0078D4?logo=microsoftazure&logoColor=white)
![Pipeline](https://img.shields.io/badge/CI%2FCD-Azure_DevOps-0078D7?logo=azuredevops&logoColor=white)
![Docker](https://img.shields.io/badge/container-Docker-2496ED?logo=docker&logoColor=white)

Exemplo prático de uma API ASP.NET Core pronta para deploy na Azure, com estrutura real de projeto, health check, Dockerfile, pipeline Azure DevOps e infraestrutura como código com Bicep.

O repositório foi melhorado para deixar de ser apenas um guia descritivo e virar um projeto reproduzível.

## O Que Este Projeto Entrega

- API ASP.NET Core executável
- Swagger habilitado
- Endpoint de health check
- Endpoint de status para ambiente/versionamento
- Endpoint de exemplo para previsão simples de vendas
- Dockerfile para containerização
- Pipeline Azure DevOps com build e deploy por slot
- Bicep para provisionar App Service e slot de staging

## Estrutura Atual

```text
.
├── README.md
├── Dockerfile
├── .dockerignore
├── .gitignore
├── azure-pipelines.yml
├── infra/
│   └── appservice.bicep
└── src/
    └── MinhaApi/
        ├── Controllers/
        ├── Program.cs
        ├── MinhaApi.csproj
        ├── appsettings.json
        └── appsettings.Development.json
```

## Endpoints Disponíveis

- `GET /`
  redireciona para o Swagger
- `GET /health`
  endpoint simples para health check do App Service
- `GET /api/status`
  retorna status, ambiente e versão da aplicação
- `POST /api/salesforecast/estimate`
  exemplo de endpoint de negócio
- `POST /api/echo`
  ecoa o payload recebido

## Exemplo de Requisição

### `POST /api/salesforecast/estimate`

```json
{
  "temperatureCelsius": 31,
  "isWeekend": true,
  "marketingCampaignActive": true
}
```

### Resposta esperada

```json
{
  "estimatedSalesUnits": 416,
  "generatedAtUtc": "2026-03-22T00:00:00Z",
  "assumptions": [
    "Modelo demonstrativo para deploy e observabilidade.",
    "Nao utiliza machine learning real; serve como API de exemplo para App Service."
  ]
}
```

## Como Executar Localmente

### 1. Restaurar e rodar

```bash
dotnet restore src/MinhaApi/MinhaApi.csproj
dotnet run --project src/MinhaApi/MinhaApi.csproj
```

### 2. Acessar a API

- Swagger: `https://localhost:xxxx/swagger`
- Health: `https://localhost:xxxx/health`

## Como Publicar com Docker

### Build da imagem

```bash
docker build -t minhaapi-demo .
```

### Executar container

```bash
docker run -p 8080:8080 minhaapi-demo
```

Depois disso:

- Swagger: `http://localhost:8080/swagger`
- Health: `http://localhost:8080/health`

## Pipeline Azure DevOps

O arquivo [azure-pipelines.yml](C:/Users/vitor/OneDrive/Documentos/Playground/repo-deploy-api-nuvem/azure-pipelines.yml) já traz um fluxo base com:

1. restore e build da API;
2. publish do artefato;
3. deploy para slot `staging`;
4. swap de `staging` para `production`.

Variáveis esperadas no pipeline:

- `azureServiceConnection`
- `webAppName`
- `resourceGroupName`

## Infraestrutura como Código

O arquivo [appservice.bicep](C:/Users/vitor/OneDrive/Documentos/Playground/repo-deploy-api-nuvem/infra/appservice.bicep) provisiona:

- App Service Plan
- Web App
- Slot `staging`
- Health check path em `/health`

Exemplo de deploy com Azure CLI:

```bash
az deployment group create \
  --resource-group rg-minhaapi-demo \
  --template-file infra/appservice.bicep \
  --parameters webAppName=minhaapi-demo-app
```

## Fluxo Recomendado de Deploy

1. Provisionar infraestrutura com Bicep.
2. Criar Service Connection no Azure DevOps.
3. Configurar variáveis do pipeline.
4. Fazer push no `main`.
5. Publicar no slot `staging`.
6. Validar `/health` e `/api/status`.
7. Executar swap para `production`.

## Melhorias Aplicadas Nesta Versão

- criação de uma API real em ASP.NET Core;
- inclusão de endpoints úteis para demonstração;
- health check pronto para App Service;
- Dockerfile funcional;
- pipeline YAML real para Azure DevOps;
- infraestrutura como código com Bicep;
- documentação alinhada com o projeto real.

## Próximos Passos

- adicionar testes automatizados;
- incluir autenticação com Azure AD;
- integrar Application Insights;
- adicionar deployment slot settings por ambiente;
- criar workflow GitHub Actions como alternativa ao Azure DevOps.

## Observação

Este projeto agora funciona como base prática de portfólio para deploy de APIs na Azure. Ele serve tanto como referência de estudo quanto como ponto de partida para uma aplicação real hospedada em App Service.
