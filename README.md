# Martiello - Sistema de Autoatendimento para Lanchonete

## 📝 Sobre o Projeto

Martiello é um sistema de autoatendimento desenvolvido para uma lanchonete em expansão, projetado para resolver problemas de controle de pedidos, como confusão entre atendentes e cozinha, atrasos e insatisfação dos clientes. A aplicação permite que clientes façam pedidos personalizados de forma eficiente, acompanhem o status em tempo real e realizem pagamentos integrados, enquanto o estabelecimento gerencia produtos, clientes e pedidos por um painel administrativo. Este projeto foi desenvolvido como parte do **Tech Challenge Fase 02**, refatorando a aplicação da Fase 01 para seguir padrões de **Clean Code** e **Clean Architecture**, além de implantar uma infraestrutura escalável em Kubernetes.

## 🚀 Funcionalidades Principais

- **Pedidos**: Interface de autoatendimento para clientes escolherem lanches, acompanhamentos e bebidas, com opção de identificação por CPF, cadastro (nome e e-mail) ou anonimato.
- **Pagamento**: Integração com Mercado Pago via QR Code para pagamentos rápidos e confirmados por Webhook.
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
1. **Input**: Valida dados de entrada (ex.: produtos do pedido).
2. **Output**: Define o formato de resposta (ex.: ID do pedido, status).
3. **UseCase**: Executa a lógica de negócio, garantindo encapsulamento e testabilidade.

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

## 💳 Integração com Mercado Pago

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

### Fluxo de Pagamento
1. O cliente finaliza o pedido → API gera um QR Code via Mercado Pago.
2. O cliente escaneia o QR Code pelo app do Mercado Pago.
3. O Webhook recebe a confirmação de pagamento e atualiza o status para "Recebido".
4. A cozinha inicia a preparação após a aprovação.

## 🏭 Arquitetura e implementação em Kubernetes

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

Desenvolvido por mim durante o Tech Challenge, conciliando trabalho em tempo integral e vida familiar (esposa e filhos). Apesar dos desafios de tempo, o projeto atende aos requisitos da Fase 02, aplicando boas práticas de Clean Code, Clean Architecture e Kubernetes. Há espaço para melhorias (ex.: mais testes unitários), mas o foco foi entregar uma solução funcional e alinhada ao problema da lanchonete.
