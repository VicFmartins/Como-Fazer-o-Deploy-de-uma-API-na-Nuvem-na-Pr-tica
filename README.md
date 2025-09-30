# Como-Fazer-o-Deploy-de-uma-API-na-Nuvem-na-Pr-tica

# Deploy de API na Nuvem Azure - Visual Studio e Azure DevOps - Guia Completo

## Visão Geral do Processo

Este documento apresenta um guia passo a passo completo para realizar o deploy de uma API .NET na nuvem Azure utilizando Visual Studio e Azure DevOps. O processo abrange desde a criação da aplicação até a implementação de um pipeline CI/CD automatizado com deployment slots.

### Componentes Principais

- **Visual Studio 2022**: IDE para desenvolvimento e configuração inicial
- **Azure App Service**: Hospedagem da API na nuvem
- **Azure DevOps**: Plataforma para CI/CD e controle de versão
- **Azure Repos**: Repositório de código fonte
- **Azure Pipelines**: Sistema de build e deployment automatizado
- **Deployment Slots**: Ambientes de teste e produção

## Pré-Requisitos

### Ferramentas Necessárias

- Visual Studio 2022 (Community, Professional ou Enterprise)
- .NET 6.0 ou superior SDK
- Git para controle de versão
- Conta Microsoft Azure (gratuita ou paga)
- Organização Azure DevOps

### Configurações de Conta

- Conta Azure ativa com permissões de criação de recursos
- Organização Azure DevOps configurada
- Subscription Azure válida

## Parte 1: Preparação do Projeto

### Passo 1: Criação do Projeto Web API

1. **Abrir Visual Studio 2022**
   - Selecionar "Create a new project"
   - Buscar por "ASP.NET Core Web API"
   - Selecionar o template e clicar "Next"

2. **Configurar o Projeto**
   - Nome do projeto: "MinhaAPI"
   - Local: Escolher diretório apropriado
   - Solution name: "MinhaAPI.Solution"
   - Clicar "Next"

3. **Configurações Adicionais**
   - Framework: .NET 8.0 (Long-term support)
   - Authentication type: None (para simplicidade inicial)
   - Configure for HTTPS: Marcado
   - Enable OpenAPI support: Marcado
   - Use controllers: Marcado
   - Do not use top-level statements: Desmarcado
   - Clicar "Create"

### Passo 2: Validação do Projeto Local

1. **Executar a API Localmente**
   - Pressionar F5 ou clicar em "Start Debugging"
   - Verificar se o Swagger UI abre no navegador
   - Testar os endpoints padrão (WeatherForecast)
   - Parar a execução (Shift+F5)

2. **Estrutura do Projeto**
   ```
   MinhaAPI/
   ├── Controllers/
   │   └── WeatherForecastController.cs
   ├── Properties/
   │   └── launchSettings.json
   ├── appsettings.json
   ├── appsettings.Development.json
   ├── Program.cs
   └── MinhaAPI.csproj
   ```

## Parte 2: Configuração do Controle de Versão

### Passo 3: Inicialização do Git Local

1. **Configurar Git no Projeto**
   - No Solution Explorer, clicar com botão direito na Solution
   - Selecionar "Add Solution to Source Control"
   - Escolher "Git" como sistema de controle de versão
   - Confirmar a criação do repositório local

2. **Primeiro Commit**
   - Abrir "Git Changes" no Visual Studio (View > Git Changes)
   - Adicionar mensagem de commit: "Initial commit - Web API project setup"
   - Clicar "Commit All"

### Passo 4: Criação do Repositório Azure Repos

1. **Acessar Azure DevOps**
   - Navegar para https://dev.azure.com
   - Fazer login com conta Microsoft
   - Selecionar ou criar uma organização

2. **Criar Novo Projeto**
   - Clicar "New project"
   - Nome do projeto: "MinhaAPI-DevOps"
   - Visibility: Private
   - Version control: Git
   - Work item process: Agile
   - Clicar "Create"

3. **Conectar Repositório Local ao Azure Repos**
   - No Visual Studio, ir para "Git Changes"
   - Clicar em "Push to remote repository"
   - Selecionar "Azure DevOps"
   - Escolher a organização e projeto criados
   - Confirmar a conexão

## Parte 3: Configuração dos Recursos Azure

### Passo 5: Criação do Resource Group

1. **Acessar Azure Portal**
   - Navegar para https://portal.azure.com
   - Fazer login com a mesma conta do Azure DevOps

2. **Criar Resource Group**
   - Pesquisar "Resource groups" na barra de busca
   - Clicar "Create"
   - Subscription: Selecionar a subscription apropriada
   - Resource group name: "rg-minhaapi-prod"
   - Region: "East US" (ou região preferida)
   - Clicar "Review + create" e depois "Create"

### Passo 6: Criação do App Service Plan

1. **Criar App Service Plan**
   - No Azure Portal, pesquisar "App Service plans"
   - Clicar "Create"
   - Resource Group: "rg-minhaapi-prod"
   - Name: "asp-minhaapi-prod"
   - Operating System: Windows
   - Region: "East US" (mesma do Resource Group)
   - Pricing Tier: "Free F1" (para teste) ou "Basic B1" (para produção)
   - Clicar "Review + create" e depois "Create"

### Passo 7: Criação do App Service

1. **Criar Web App**
   - Pesquisar "App Services" no Azure Portal
   - Clicar "Create" > "Web App"
   - Resource Group: "rg-minhaapi-prod"
   - Name: "minhaapi-prod-webapp" (deve ser único globalmente)
   - Publish: Code
   - Runtime stack: ".NET 8 (LTS)"
   - Operating System: Windows
   - Region: "East US"
   - App Service Plan: Selecionar "asp-minhaapi-prod"
   - Clicar "Review + create" e depois "Create"

### Passo 8: Configuração de Deployment Slots

1. **Acessar o App Service Criado**
   - Navegar para o App Service "minhaapi-prod-webapp"
   - No menu lateral, ir para "Deployment" > "Deployment slots"

2. **Criar Slot de Staging**
   - Clicar "Add Slot"
   - Name: "staging"
   - Clone settings from: "minhaapi-prod-webapp" (produção)
   - Clicar "Add"

3. **Verificar Configuração**
   - Confirmar que agora existem dois slots: "Production" e "staging"
   - Anotar as URLs de ambos os slots

## Parte 4: Configuração do Service Connection

### Passo 9: Criação do Service Principal

1. **Acessar Azure Active Directory**
   - No Azure Portal, pesquisar "Azure Active Directory"
   - Ir para "App registrations"
   - Clicar "New registration"

2. **Configurar App Registration**
   - Name: "MinhaAPI-DevOps-ServicePrincipal"
   - Supported account types: "Accounts in this organizational directory only"
   - Redirect URI: Deixar vazio
   - Clicar "Register"

3. **Obter Informações do Service Principal**
   - Anotar "Application (client) ID"
   - Anotar "Directory (tenant) ID"
   - Ir para "Certificates & secrets"
   - Clicar "New client secret"
   - Description: "DevOps Pipeline Secret"
   - Expires: "24 months"
   - Clicar "Add" e anotar o valor do secret (aparece apenas uma vez)

### Passo 10: Configuração de Permissões

1. **Atribuir Permissões no Resource Group**
   - Voltar ao Resource Group "rg-minhaapi-prod"
   - Ir para "Access control (IAM)"
   - Clicar "Add" > "Add role assignment"
   - Role: "Contributor"
   - Assign access to: "User, group, or service principal"
   - Select: Buscar pelo nome do Service Principal criado
   - Clicar "Save"

### Passo 11: Criação do Service Connection no Azure DevOps

1. **Acessar Configurações do Projeto**
   - No Azure DevOps, ir para o projeto "MinhaAPI-DevOps"
   - Clicar em "Project settings" (canto inferior esquerdo)
   - No menu lateral, ir para "Service connections"

2. **Criar Nova Service Connection**
   - Clicar "New service connection"
   - Selecionar "Azure Resource Manager"
   - Clicar "Next"
   - Escolher "Service principal (manual)"
   - Clicar "Next"

3. **Configurar Service Connection**
   - Environment: Azure Cloud
   - Scope Level: Subscription
   - Subscription Id: Obter do Azure Portal (Subscriptions)
   - Subscription Name: Nome da sua subscription
   - Service Principal Id: Application ID anotado anteriormente
   - Credential: Service Principal Key
   - Service Principal Key: Secret anotado anteriormente
   - Tenant Id: Directory ID anotado anteriormente
   - Service connection name: "Azure-MinhaAPI-Connection"
   - Description: "Service connection for MinhaAPI deployment"
   - Security: Deixar "Grant access permission to all pipelines" desmarcado
   - Clicar "Verify and save"

## Parte 5: Deploy Manual via Visual Studio

### Passo 12: Configuração do Deploy Direto

1. **Iniciar Processo de Publish**
   - No Visual Studio, clicar com botão direito no projeto da API
   - Selecionar "Publish"

2. **Configurar Target**
   - Selecionar "Azure"
   - Clicar "Next"
   - Selecionar "Azure App Service (Windows)"
   - Clicar "Next"

3. **Selecionar App Service**
   - Fazer login na conta Azure (se necessário)
   - Expandir o Resource Group "rg-minhaapi-prod"
   - Selecionar "minhaapi-prod-webapp"
   - Clicar "Next"

4. **Configurar API Management (Opcional)**
   - Para este exemplo, marcar "Skip this step"
   - Clicar "Finish"

5. **Executar Deploy Manual**
   - Na tela de Publish Summary, clicar "Publish"
   - Aguardar o processo de build e deploy
   - Verificar se o navegador abre com a API funcionando

### Passo 13: Teste do Deploy Manual

1. **Validar Funcionamento**
   - Acessar a URL do App Service
   - Verificar se o Swagger UI carrega corretamente
   - Testar os endpoints disponíveis
   - Verificar logs no Azure Portal se necessário

## Parte 6: Configuração do Pipeline CI/CD

### Passo 14: Criação do Build Pipeline

1. **Acessar Azure Pipelines**
   - No Azure DevOps, ir para "Pipelines" > "Pipelines"
   - Clicar "New pipeline"

2. **Configurar Source Code**
   - Selecionar "Azure Repos Git"
   - Escolher o repositório "MinhaAPI-DevOps"

3. **Selecionar Template**
   - Escolher "ASP.NET Core"
   - O Azure DevOps criará automaticamente um arquivo azure-pipelines.yml

4. **Configurar Pipeline YAML**
   ```yaml
   trigger:
   - main

   pool:
     vmImage: 'windows-latest'

   variables:
     buildConfiguration: 'Release'
     dotNetFramework: 'net8.0'
     dotNetVersion: '8.0.x'
     targetRuntime: 'win-x64'

   stages:
   - stage: Build
     displayName: 'Build Stage'
     jobs:
     - job: Build
       displayName: 'Build Job'
       steps:
       - task: UseDotNet@2
         displayName: 'Use .NET SDK'
         inputs:
           packageType: 'sdk'
           version: $(dotNetVersion)

       - task: DotNetCoreCLI@2
         displayName: 'Restore NuGet Packages'
         inputs:
           command: 'restore'
           projects: '**/*.csproj'

       - task: DotNetCoreCLI@2
         displayName: 'Build Application'
         inputs:
           command: 'build'
           projects: '**/*.csproj'
           arguments: '--configuration $(buildConfiguration) --no-restore'

       - task: DotNetCoreCLI@2
         displayName: 'Run Tests'
         inputs:
           command: 'test'
           projects: '**/*Tests.csproj'
           arguments: '--configuration $(buildConfiguration) --no-build --verbosity normal --collect:"XPlat Code Coverage" --results-directory $(Agent.TempDirectory)'
         continueOnError: true

       - task: DotNetCoreCLI@2
         displayName: 'Publish Application'
         inputs:
           command: 'publish'
           publishWebProjects: true
           arguments: '--configuration $(buildConfiguration) --output $(Build.ArtifactStagingDirectory) --no-build'
           zipAfterPublish: true

       - task: PublishBuildArtifacts@1
         displayName: 'Publish Build Artifacts'
         inputs:
           PathtoPublish: '$(Build.ArtifactStagingDirectory)'
           ArtifactName: 'drop'
           publishLocation: 'Container'
   ```

### Passo 15: Configuração do Release Pipeline

1. **Criar Release Pipeline**
   - Ir para "Pipelines" > "Releases"
   - Clicar "New pipeline"
   - Selecionar "Azure App Service deployment" template
   - Renomear stage para "Staging"

2. **Configurar Artifact**
   - Na seção "Artifacts", clicar "Add"
   - Source type: "Build"
   - Project: "MinhaAPI-DevOps"
   - Source (build pipeline): Selecionar o pipeline criado anteriormente
   - Default version: "Latest"
   - Source alias: "drop"
   - Clicar "Add"

3. **Configurar Continuous Deployment Trigger**
   - Clicar no ícone de raio no artifact
   - Habilitar "Continuous deployment trigger"
   - Branch filters: "main"

### Passo 16: Configuração do Stage de Staging

1. **Configurar Deploy para Staging**
   - Clicar no link "1 job, 1 task" no stage "Staging"
   - Agent job: Usar "Azure Pipelines" com "windows-latest"

2. **Configurar Deploy do Azure App Service**
   - Na task "Azure App Service Deploy"
   - Azure subscription: Selecionar a service connection criada
   - App Service type: "Web App on Windows"
   - App Service name: "minhaapi-prod-webapp"
   - Deploy to Slot or App Service Environment: Marcado
   - Resource group: "rg-minhaapi-prod"
   - Slot: "staging"
   - Package or folder: "$(System.DefaultWorkingDirectory)/drop/*.zip"

### Passo 17: Adição do Stage de Produção

1. **Clonar Stage de Staging**
   - Voltar para a visão geral do Release Pipeline
   - No stage "Staging", clicar no menu de contexto (...)
   - Selecionar "Clone"
   - Renomear para "Production"

2. **Configurar Aprovação Manual**
   - No stage "Production", clicar no ícone de usuário (Pre-deployment conditions)
   - Habilitar "Pre-deployment approvals"
   - Approvers: Adicionar seu usuário
   - Timeout: "30 days"

3. **Configurar Deploy para Produção**
   - Editar o stage "Production"
   - Na task "Azure App Service Deploy"
   - Desmarcar "Deploy to Slot or App Service Environment"
   - Isso fará deploy diretamente para o slot de produção

4. **Configurar Swap de Slots**
   - Adicionar nova task "Azure App Service Manage"
   - Azure subscription: Mesma service connection
   - App Service name: "minhaapi-prod-webapp"
   - Action: "Swap Slots"
   - Source Slot: "staging"
   - Target Slot: "production"
   - Preserve Vnet: Marcado

### Passo 18: Configuração de Variáveis

1. **Variáveis do Pipeline**
   - Na aba "Variables" do Release Pipeline
   - Adicionar variáveis:
     - BuildConfiguration: "Release"
     - WebAppName: "minhaapi-prod-webapp"
     - ResourceGroupName: "rg-minhaapi-prod"

2. **Variáveis por Stage**
   - Stage "Staging": SlotName = "staging"
   - Stage "Production": SlotName = "production"

## Parte 7: Configuração Avançada

### Passo 19: Configuração de Health Checks

1. **Adicionar Health Check na API**
   - No Visual Studio, editar Program.cs:
   ```csharp
   var builder = WebApplication.CreateBuilder(args);

   // Adicionar serviços
   builder.Services.AddControllers();
   builder.Services.AddEndpointsApiExplorer();
   builder.Services.AddSwaggerGen();
   builder.Services.AddHealthChecks();

   var app = builder.Build();

   // Configurar pipeline
   if (app.Environment.IsDevelopment())
   {
       app.UseSwagger();
       app.UseSwaggerUI();
   }

   app.UseHttpsRedirection();
   app.UseAuthorization();
   app.MapControllers();
   
   // Adicionar endpoint de health check
   app.MapHealthChecks("/health");

   app.Run();
   ```

2. **Configurar Health Check no Azure**
   - No App Service, ir para "Monitoring" > "Health check"
   - Habilitar health check
   - Health check path: "/health"
   - Load balancing: "Least Connections"

### Passo 20: Configuração de Application Insights

1. **Criar Application Insights**
   - No Azure Portal, buscar "Application Insights"
   - Clicar "Create"
   - Resource Group: "rg-minhaapi-prod"
   - Name: "ai-minhaapi-prod"
   - Region: "East US"
   - Resource Mode: "Workspace-based"
   - Criar novo Log Analytics Workspace se necessário

2. **Conectar ao App Service**
   - No App Service, ir para "Settings" > "Application Insights"
   - Clicar "Turn on Application Insights"
   - Selecionar o Application Insights criado
   - Clicar "Apply"

3. **Configurar na Aplicação**
   - No Visual Studio, instalar pacote NuGet:
   ```xml
   <PackageReference Include="Microsoft.ApplicationInsights.AspNetCore" Version="2.21.0" />
   ```
   
   - Adicionar no Program.cs:
   ```csharp
   builder.Services.AddApplicationInsightsTelemetry();
   ```

### Passo 21: Configuração de Variáveis de Ambiente

1. **App Settings no Azure**
   - No App Service, ir para "Settings" > "Environment variables"
   - Adicionar configurações:
     - ASPNETCORE_ENVIRONMENT: "Production" (slot produção) / "Staging" (slot staging)
     - APPLICATIONINSIGHTS_CONNECTION_STRING: (obtida do Application Insights)

2. **Configuração no appsettings.json**
   ```json
   {
     "Logging": {
       "LogLevel": {
         "Default": "Information",
         "Microsoft.AspNetCore": "Warning"
       }
     },
     "ApplicationInsights": {
       "ConnectionString": ""
     },
     "AllowedHosts": "*"
   }
   ```

## Parte 8: Testes e Validação

### Passo 22: Teste do Pipeline Completo

1. **Fazer Alteração no Código**
   - No Visual Studio, adicionar um novo controller:
   ```csharp
   [ApiController]
   [Route("api/[controller]")]
   public class StatusController : ControllerBase
   {
       [HttpGet]
       public IActionResult Get()
       {
           return Ok(new { 
               Status = "API is running",
               Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
               Timestamp = DateTime.UtcNow
           });
       }
   }
   ```

2. **Commit e Push**
   - Fazer commit das alterações
   - Push para o repositório Azure Repos
   - Verificar se o build pipeline inicia automaticamente

3. **Monitorar Deploy**
   - Acompanhar o progresso no Azure DevOps
   - Verificar deploy no slot staging
   - Testar a API no ambiente staging
   - Aprovar o deploy para produção
   - Verificar o swap de slots

### Passo 23: Configuração de Monitoramento

1. **Alertas no Application Insights**
   - Criar alertas para:
     - Response time > 5 segundos
     - Error rate > 5%
     - Availability < 95%

2. **Dashboard de Monitoramento**
   - Criar dashboard personalizado
   - Incluir métricas de performance
   - Configurar visualizações de logs

### Passo 24: Backup e Recovery

1. **Configurar Backup Automático**
   - No App Service, ir para "Settings" > "Backups"
   - Criar storage account para backups
   - Configurar schedule de backup diário

2. **Documentar Processo de Recovery**
   - Procedimento para restaurar backup
   - Processo de rollback via slot swap
   - Contatos para emergências

## Configurações de Segurança

### Passo 25: Configuração de HTTPS e SSL

1. **Certificado SSL**
   - No App Service, ir para "Settings" > "TLS/SSL settings"
   - Configurar "HTTPS Only" como habilitado
   - Verificar certificado SSL automático

2. **Configurações de Segurança Adiccionais**
   - Habilitar "Always On" no App Service
   - Configurar "Minimum TLS Version" como 1.2
   - Revisar configurações de CORS se necessário

### Passo 26: Controle de Acesso

1. **Configuração de IP Restrictions**
   - Se necessário, configurar restrições de IP
   - No App Service: "Settings" > "Networking" > "Access restriction"

2. **Authentication (Opcional)**
   - Para APIs que necessitam autenticação
   - Configurar Azure AD ou outros provedores

## Manutenção e Otimização

### Passo 27: Otimização de Performance

1. **Configurações de Performance**
   - Verificar configurações do App Service Plan
   - Considerar upgrade para tiers superiores se necessário
   - Configurar Auto-scaling se aplicável

2. **Otimização de Build**
   - Otimizar dockerfile se usar containers
   - Configurar cache de dependências no pipeline
   - Reduzir tempo de build com paralelização

### Passo 28: Documentação Final

1. **Documentação de Deployment**
   - URLs dos ambientes (staging/production)
   - Credenciais de acesso necessárias
   - Processo de troubleshooting

2. **Runbook de Operações**
   - Procedimentos de deploy manual se necessário
   - Contatos da equipe
   - Processo de escalação de problemas

## Troubleshooting Comum

### Problemas de Deploy

1. **Erro de Service Connection**
   - Verificar se service principal tem permissões adequadas
   - Validar se secret não expirou
   - Confirmar subscription ID e tenant ID

2. **Erro de Build**
   - Verificar se todas as dependências estão no repositório
   - Confirmar versão do .NET no pipeline
   - Validar sintaxe do YAML

3. **Erro de Health Check**
   - Verificar se endpoint /health está respondendo
   - Confirmar configurações de timeout
   - Validar logs da aplicação

### Problemas de Performance

1. **Lentidão na API**
   - Verificar métricas no Application Insights
   - Analisar logs de performance
   - Considerar upgrade do App Service Plan

2. **Erro de Timeout**
   - Aumentar timeout nas configurações do pipeline
   - Otimizar queries de database se aplicável
   - Verificar configurações de connection pool



**Benefícios Implementados:**
- Deploy automatizado via CI/CD
- Ambientes separados (staging/production)
- Processo de aprovação para produção
- Monitoramento integrado com Application Insights
- Rollback rápido via slot swap
- Health checks automatizados
- Backup e recovery configurados
