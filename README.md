# Martiello - Sistema de Gerenciamento de Lanchonete

## 📝 Sobre o Projeto

Martiello é uma aplicação de gerenciamento de lanchonete que permite realizar pedidos, cadastrar produtos e processar pagamentos. O sistema oferece uma experiência flexível, permitindo a criação de pedidos com ou sem identificação do cliente (CPF).

## 🚀 Funcionalidades Principais

- Gerenciamento de produtos
- Sistema de pedidos com atualização automática de status
- Integração com Mercado Pago para pagamentos
- Rastreamento de status do pedido em tempo real
- Cadastro opcional de clientes

## 💻 Arquitetura

### Visão Geral
O projeto está estruturado em camadas seguindo os princípios de Clean Architecture:
- **Martiello.Domain**: Contém as entidades e regras de negócio
- **Martiello.Application**: Implementa os casos de uso da aplicação
- **Martiello.Infrastructure**: Gerencia acesso a dados e serviços externos
- **Martiello**: API principal da aplicação

### Padrão UseCase
O projeto implementa uma arquitetura baseada em UseCases, onde cada operação da API possui seu próprio UseCase específico, contendo três componentes principais:

1. **Input**
   - Responsável por receber e validar os dados de entrada
   - Define o contrato de dados necessários para a operação
   - Garante que os dados estejam no formato correto antes do processamento

2. **Output**
   - Define a estrutura de retorno da operação
   - Padroniza as respostas da aplicação
   - Facilita o mapeamento de respostas para o cliente

3. **UseCase**
   - Contém a lógica de negócio específica da operação
   - Implementa as regras e fluxos necessários
   - Garante a separação de responsabilidades

Esta arquitetura traz diversos benefícios:
- Código mais organizado e manutenível
- Separação clara de responsabilidades
- Facilidade para implementar novos recursos
- Melhor testabilidade
- Evita classes de serviço grandes e acopladas

### Banco de Dados

O MongoDB foi escolhido como banco de dados para este projeto devido a:
- Natureza dinâmica dos pedidos
- Flexibilidade no esquema de dados
- Melhor performance para operações de leitura/escrita em documentos
- Facilidade para armazenar informações complexas dos pedidos em um único documento
- Escalabilidade horizontal

## 🔄 Sistema de Status de Pedidos

O sistema atualiza automaticamente o status dos pedidos seguindo o fluxo:
1. Recebido (1-3 minutos)
2. Em Preparação (30s-1min)
3. Pronto
4. Finalizado

O tempo de preparação é calculado com base nos produtos do pedido, e o sistema possui um multiplicador de tempo configurável para testes.

## 💳 Integração Mercado Pago

### Integração com Mercado Pago

#### Configuração das Credenciais

1. Acesse [Mercado Pago](https://www.mercadopago.com.br)
2. Faça login ou crie uma nova conta
3. Acesse a aba "Seu negócio"
4. Vá em "Configurações"
5. Selecione "Credenciais"
6. Em "Credenciais de teste", você encontrará:
   - Access Token
   - Public Key
7. Configure as credenciais no arquivo `appsettings.json`:
   ```json
   {
     "MercadoPago": {
       "AccessToken": "seu_access_token",
       "PublicKey": "sua_public_key"
     }
   }
   ```

#### Funcionalidade de Pagamento

A integração com o Mercado Pago permite:
- Geração automática de QR Code para cada pedido
- Pagamento rápido via aplicativo do Mercado Pago
- Atualização em tempo real do status do pagamento
- Experiência seamless para o cliente

Quando um pedido é criado:
1. O sistema gera automaticamente um QR Code único
2. O cliente pode escanear o QR Code com o app do Mercado Pago
3. O pagamento é processado instantaneamente
4. O sistema recebe a confirmação do pagamento e atualiza o status do pedido

## 🐳 Configuração Docker

### Pré-requisitos
- Docker
- Docker Compose

### Passos para Execução

1. Clone o repositório
   ```bash
   git clone [url-do-repositorio]
   ```

2. Navegue até a pasta do projeto
   ```bash
   cd martiello
   ```

3. Execute o Docker Compose
   ```bash
   docker-compose up -d
   ```

O docker-compose irá criar e iniciar todos os containers necessários para a aplicação, incluindo:
- API
- MongoDB
- Serviços relacionados

A aplicação estará disponível em `http://localhost:5000`

## ⚙️ Configurações

As principais configurações podem ser ajustadas no arquivo `appsettings.json`:

```json
{
  "OrderProcessing": {
    "UseRealTimePreparation": true
  },
  "ConnectionStrings": {
    "MongoDb": "sua_connection_string_mongodb"
  }
}
```

## 📚 Documentação da API

A documentação da API está disponível através do Swagger UI em:
```
http://localhost:5000/swagger
```

## 👨‍👩‍👧‍👦 Sobre o Desenvolvimento

Este projeto foi desenvolvido inteiramente por mim, conciliando as responsabilidades do trabalho em período integral com a vida familiar, incluindo esposa e filhos. Apesar dos desafios de tempo e das múltiplas responsabilidades, consegui desenvolver as funcionalidades essenciais solicitadas. 

Embora existam muitas possibilidades de expansão e melhorias, o projeto atende aos requisitos principais e demonstra a aplicação de boas práticas de arquitetura e desenvolvimento. Espero que os avaliadores possam compreender esse contexto pessoal ao analisar o projeto, considerando o esforço de equilibrar vida profissional, estudos e família durante o desenvolvimento.