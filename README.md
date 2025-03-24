# Martiello - Sistema de Autoatendimento para Lanchonete

## 📝 Sobre o Projeto

Martiello é um sistema de autoatendimento desenvolvido para uma lanchonete em expansão, projetado para resolver problemas de controle de pedidos, como confusão entre atendentes e cozinha, atrasos e insatisfação dos clientes. A aplicação permite que clientes façam pedidos personalizados de forma eficiente, acompanhem o status em tempo real e realizem pagamentos integrados, enquanto o estabelecimento gerencia produtos, clientes e pedidos por um painel administrativo. Este projeto foi desenvolvido como parte do **Tech Challenge Fase 02**, refatorando a aplicação da Fase 01 para seguir padrões de **Clean Code** e **Clean Architecture**, além de implantar uma infraestrutura escalável em Kubernetes.

## 🚀 Funcionalidades Principais

- **Pedidos**: Interface de autoatendimento para clientes escolherem lanches, acompanhamentos e bebidas, com opção de identificação por CPF, cadastro (nome e e-mail) ou anonimato.
- **Pagamento**: Integração com Mercado Pago via QR Code para pagamentos rápidos, com atualização automática do status do pedido por meio de Webhook.
- **Acompanhamento**: Monitoramento em tempo real do status do pedido (Recebido → Em Preparação → Pronto → Finalizado).
- **Entrega**: Notificação ao cliente quando o pedido está pronto para retirada.
- **Administração**:
  - Gerenciamento de clientes para campanhas promocionais.
  - Cadastro de produtos (nome, categoria, preço, descrição, imagem) em categorias fixas: Lanche, Acompanhamento, Bebida, Sobremesa.
  - Acompanhamento de pedidos em andamento com tempo de espera.

## 💻 Arquitetura da Aplicação

### Visão Geral
A aplicação segue os princípios de **Clean Architecture**, dividida em camadas para garantir separação de responsabilidades e manutenibilidade:
- **Martiello.Domain**: Entidades (ex.: Pedido, Produto) e regras de negócio (ex.: validação de pedidos).
- **Martiello.Application**: Casos de uso (UseCases) que orquestram a lógica da aplicação.
- **Martiello.Infrastructure**: Integrações externas (MongoDB, Mercado Pago) e acesso a dados.
- **Martiello**: API RESTful que expõe os endpoints para clientes e cozinha.

### Padrão UseCase
Cada operação da API é implementada como um **UseCase**, estruturado em:
1. **Input**: Validação dos dados de entrada (ex.: produtos do pedido).
2. **Output**: Definição do formato de resposta (ex.: ID do pedido, status).
3. **UseCase**: Execução da lógica de negócio, garantindo encapsulamento e testabilidade.

**Benefícios**:
- Código organizado e modular.
- Facilidade para adicionar novas funcionalidades.
- Alta testabilidade com mocks nos UseCases.

### Banco de Dados
Utilizamos o **MongoDB** por:
- Flexibilidade para armazenar pedidos complexos como documentos únicos.
- Escalabilidade horizontal para suportar o crescimento da lanchonete.
- Performance otimizada em operações de leitura/escrita.

## 🔄 Sistema de Status de Pedidos

O fluxo de status dos pedidos é atualizado automaticamente:
1. **Recebido**: Quando o pedido é registrado (1-3 minutos para confirmação de pagamento).
2. **Em Preparação**: Após pagamento aprovado (30s-1min, ajustável por multiplicador).
3. **Pronto**: Preparação concluída, notificação enviada ao cliente.
4. **Finalizado**: Após retirada pelo cliente.

A cozinha utiliza esses status para priorizar pedidos, garantindo que nenhum seja perdido.

## 💳 Integração com Mercado Pago e Webhook

### Configuração das Credenciais
1. Acesse o [Mercado Pago](https://www.mercadopago.com.br/developers).
2. Em "Credenciais", obtenha:
   - `AccessToken` (armazenado em Secrets).
   - `PublicKey` (armazenado em Secrets).
3. Configure em `appsettings.json` ou via ConfigMap/Secrets no Kubernetes:
   ```json
   {
     "MercadoPago": {
       "AccessToken": "SEU_ACCESS_TOKEN",
       "PublicKey": "SUA_PUBLIC_KEY"
     }
   }
   ```

### Webhook Integrado com Mercado Pago

O sistema possui um webhook que integra com o Mercado Pago para receber notificações de pagamento e atualizar o status dos pedidos em tempo real. Para ambientes de desenvolvimento local, recomenda-se o uso do ngrok, que permite expor sua aplicação local para a internet. Siga os passos abaixo:

#### 1. Expondo a Aplicação com Ngrok

Caso o seu sistema não esteja hospedado em um ambiente público, utilize o [ngrok](https://ngrok.com) para gerar uma URL pública temporária:

- **Baixar e Instalar:**
  1. Acesse o site oficial do ngrok em [ngrok.com/download](https://ngrok.com/download).
  2. Faça o download da versão correspondente ao seu sistema operacional.
  3. Extraia o executável para um diretório de sua preferência.

- **Configuração Inicial:**
  1. Crie uma conta gratuita no site do ngrok.
  2. Após o login, obtenha seu token de autenticação (authtoken).
  3. No terminal, configure o ngrok com o comando:
     ```bash
     ngrok config add-authtoken $YOUR_AUTHTOKEN
     ```

- **Expondo a Aplicação Local:**
  1. Com sua aplicação rodando localmente (por exemplo, na porta 5000), inicie o ngrok:
     ```bash
     ngrok http 5000
     ```
  2. O ngrok fornecerá uma URL pública, por exemplo, `https://exemplo.ngrok.io`, que redirecionará as requisições para sua aplicação local.

#### 2. Configurando o Webhook no Mercado Pago

Após expor sua aplicação, siga os passos abaixo:

- **Acessar o Painel de Desenvolvedores:**
  1. Entre no [Painel de Desenvolvedores do Mercado Pago](https://www.mercadopago.com.br/developers/panel/app).
  2. Crie um projeto, caso ainda não exista, e navegue até a seção **"Webhooks"**.

- **Inserir a URL do Webhook:**
  1. Combine a URL pública gerada pelo ngrok com a rota específica do webhook da aplicação.  
     Por exemplo:
     - **URL do ngrok:** `https://ad09-181-222-45-60.ngrok-free.app/`
     - **Rota do webhook:** `/webhook/payment`
     - **URL completa:** `https://ad09-181-222-45-60.ngrok-free.app/webhook/payment`
  2. Na seção **"Eventos recomendados para integrações com CheckoutPro"**, marque a opção **"Pagamentos"**.
  3. Salve as configurações.

#### 3. Testando a Integração

Após configurar o webhook, você pode testar a integração diretamente pelo painel do Mercado Pago:
- Utilize a opção **"Simular notificação"** para enviar uma notificação de teste ao seu webhook.
- Verifique nos logs da aplicação se a notificação foi recebida e processada corretamente.

#### 4. Considerações Finais

- **Ambiente de Desenvolvimento:** O ngrok é ideal para testes e desenvolvimento. Em produção, utilize uma URL própria e estável.
- **Monitoramento:** Monitore os logs e as notificações para identificar e resolver possíveis problemas na comunicação entre o Mercado Pago e sua aplicação.

## 🏭 Arquitetura e Implementação em Kubernetes

A implantação em Kubernetes está detalhada em um documento separado. Consulte a documentação completa em: [Arquitetura Kubernetes](kubernetes/README.md).

## 🐳 Configuração e Execução

### Pré-requisitos
- Docker
- Docker Compose (para desenvolvimento local)
- Kubernetes (Minikube ou cluster em nuvem como AKS/EKS/GKE)
- kubectl

### Passos para Execução Local
1. Clone o repositório:
   ```bash
   git clone https://github.com/feligra/Martiello-CSharp
   cd martiello
   ```
2. Suba com Docker Compose:
   ```bash
   docker-compose up -d
   ```
3. Acesse o Swagger em `http://localhost:5000/swagger`.

## 📚 Documentação da API

### Endpoints
- **POST /api/checkout**: Cria um pedido e retorna o ID e QR Code.
  - Exemplo: `{"produtos": [{"id": "lanche1", "quantidade": 1}], "cpf": "123.456.789-00"}`
- **GET /api/pagamento/status/{id}**: Consulta o status do pagamento.
- **POST /api/webhook**: Recebe confirmação do Mercado Pago.
- **GET /api/pedidos**: Lista pedidos (ordenados: Pronto > Em Preparação > Recebido; mais antigos primeiro).
- **PUT /api/pedidos/{id}/status**: Atualiza o status do pedido.

### Collection
- Disponível no Swagger: `http://localhost:5000/swagger`.

## 👨‍👩‍👧‍👦 Sobre o Desenvolvimento

Desenvolvido durante o Tech Challenge, conciliando trabalho em tempo integral e vida familiar, o projeto atende aos requisitos da Fase 02 com aplicação dos princípios de Clean Code, Clean Architecture e a implantação em Kubernetes. Embora haja espaço para melhorias, como a implementação de mais testes unitários, o foco foi entregar uma solução funcional e alinhada aos desafios do setor de autoatendimento.
