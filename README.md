<br />
<div align="center">
  
  ![C2F553F0-3194-47E5-BF5C-88A70DF98619_4_5005_c](https://user-images.githubusercontent.com/19569999/162860920-04c561a5-5dd2-4b99-9400-bf5d7b083692.jpeg)
  
  <h3 align="center">GISA MIC API</h3>
</div>

<!-- ABOUT THE PROJECT -->
## Sobre o projeto
API MIC com a funcionalidade de Reembolso para POC desenvolvida como parte da entrega do TCC para obtenção do título de 'Especialista em Arquitetura de Software Distribuído' pela PUC-MG.

### Tecnologias
* [JWT](https://jwt.io/introduction)
* [Entity Framework](https://docs.microsoft.com/pt-br/ef/)
* [Open API](https://swagger.io/specification/)
* [Net 5](https://docs.microsoft.com/pt-br/dotnet/core/compatibility/5.0)

## Iniciando

### Pré requisito
Faz-se necessário ter o [Docker](https://docs.docker.com/get-docker/) instalado e funcionando corretamente.

### Instalação
1. Clone o repositório
   ```sh
   git clone https://github.com/everton159/BoaSaudeRefund.git
   ```
2. Acesse o diretório da aplicação, por exemplo:
   ```sh
   cd .\repos\BoaSaudeRefund
   ```
3. Insira a senha do banco de dados em `appsettings.json` na propriedade `Password` em `DefaultConnection`
   ```json
   "DefaultConnection": "Initial Catalog=gisa-auth-db; Data Source=sqldb; Persist Security Info=True;User ID=SA;Password=PASSWORD_DATABASE;"
   ```
4. Ainda em `appsettings.json`, informe sua  `SecretKey` que será utilizada para gerar o token JWT 
   ```json
   "AppSettings": {
    "SecretKey": "MYSECRETSUPERSECRET"
    ...
    }
   ```
6. No terminal, gere a imagem dos containers
   ```sh
   docker-compose build
   ````
7. No terminal, suba os containers
   ```sh
   docker-compose up -d
   ````
8. Acesse a documentação da API através da url http://localhost:9001/swagger
