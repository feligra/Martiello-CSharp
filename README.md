# Martiello - Sistema de Gerenciamento de Lanchonete

## 📝 Sobre o Projeto

Martiello é uma aplicação de gerenciamento de lanchonete que permite realizar pedidos, cadastrar produtos e processar pagamentos. O sistema oferece uma experiência flexível, permitindo a criação de pedidos com ou sem identificação do cliente (CPF).

## 🚀 Funcionalidades Principais

- Gerenciamento de produtos
- Sistema de pedidos com atualização automática de status
- Integração com Mercado Pago para pagamentos
- Rastreamento de status do pedido em tempo real
- Cadastro opcional de clientes

## 🐳 Kubernetes e Infraestrutura

### O que é Kubernetes?

Kubernetes (também conhecido como K8s) é uma plataforma de código aberto para automação de implantação, dimensionamento e gerenciamento de aplicações em containers. Ele foi projetado para:

1. **Automação**: Gerencia automaticamente a implantação e atualização de aplicações
2. **Escalabilidade**: Permite escalar aplicações horizontalmente
3. **Resiliência**: Mantém as aplicações funcionando mesmo se alguns componentes falharem
4. **Portabilidade**: Funciona em qualquer ambiente (local, nuvem, híbrido)

### Componentes Principais do Kubernetes

1. **Pod**: É a menor unidade de deploy no Kubernetes. Um pod pode conter um ou mais containers.
2. **Deployment**: Gerencia um conjunto de pods idênticos, garantindo que o número desejado de pods esteja sempre rodando.
3. **Service**: Fornece um endpoint estável para acessar os pods.
4. **ConfigMap**: Armazena configurações não sensíveis.
5. **Secret**: Armazena informações sensíveis como senhas e chaves.
6. **StatefulSet**: Gerencia pods com identidade estável e armazenamento persistente.
7. **HorizontalPodAutoscaler (HPA)**: Escala automaticamente o número de pods baseado em métricas.

### Manifestos Kubernetes

#### 1. ConfigMap (api-configmap.yaml)

```yaml
apiVersion: v1
kind: ConfigMap
metadata:
  name: api-config
data:
  ASPNETCORE_ENVIRONMENT: 'Development'
  DatabaseName: 'Martiello'
  # ... outras configurações
```

**Função**: Armazena configurações não sensíveis da aplicação.

#### 2. Deployment (api-deployment.yaml)

```yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: martiello-api
spec:
  replicas: 1
  template:
    spec:
      containers:
        - name: api
          image: martiello-api:latest
          ports:
            - containerPort: 5000
```

**Função**: Define como a aplicação deve ser executada, incluindo:

- Número de réplicas
- Imagem Docker a ser usada
- Recursos necessários (CPU/memória)
- Probes de saúde
- Variáveis de ambiente

#### 3. Service (api-service.yaml)

```yaml
apiVersion: v1
kind: Service
metadata:
  name: martiello-api-service
spec:
  type: LoadBalancer
  ports:
    - port: 80
      targetPort: 5000
```

**Função**: Expõe a aplicação para acesso externo.

#### 4. HorizontalPodAutoscaler (api-hpa.yaml)

```yaml
apiVersion: autoscaling/v2
kind: HorizontalPodAutoscaler
spec:
  scaleTargetRef:
    name: martiello-api
  minReplicas: 1
  maxReplicas: 5
  metrics:
    - type: Resource
      resource:
        name: cpu
        target:
          averageUtilization: 70
```

**Função**: Escala automaticamente o número de pods baseado em uso de CPU/memória.

#### 5. StatefulSet (db-statefulset.yaml)

```yaml
apiVersion: apps/v1
kind: StatefulSet
metadata:
  name: mongodb
spec:
  serviceName: mongodb-service
  replicas: 1
  template:
    spec:
      containers:
        - name: mongodb
          image: mongo:6.0
```

**Função**: Gerencia o MongoDB com armazenamento persistente.

### Comandos Kubernetes (kubectl)

O `kubectl` é a ferramenta de linha de comando para interagir com clusters Kubernetes. Aqui estão os principais comandos que usamos:

1. **Aplicar manifestos**:

```bash
kubectl apply -f arquivo.yaml
```

**Função**: Cria ou atualiza recursos no cluster.

2. **Verificar status dos pods**:

```bash
kubectl get pods
```

**Função**: Lista todos os pods e seus status.

3. **Ver logs**:

```bash
kubectl logs -f deployment/nome-do-deployment
```

**Função**: Mostra os logs em tempo real.

4. **Descrever recursos**:

```bash
kubectl describe pod nome-do-pod
```

**Função**: Mostra informações detalhadas sobre um recurso.

5. **Expor serviço**:

```bash
minikube service nome-do-servico
```

**Função**: Expõe um serviço para acesso externo.

### Minikube

Minikube é uma ferramenta que permite executar um cluster Kubernetes localmente. Ele cria uma máquina virtual (VM) que contém um cluster Kubernetes de nó único.

Comandos principais:

1. **Iniciar cluster**:

```bash
minikube start
```

2. **Parar cluster**:

```bash
minikube stop
```

3. **Configurar Docker**:

```bash
eval $(minikube docker-env)
```

### Processo de Deploy

1. **Preparação**:

   - Iniciar Minikube
   - Configurar Docker para usar o registro do Minikube

2. **Construir Imagem**:

   - Construir a imagem Docker da aplicação
   - A imagem é construída no contexto do Minikube

3. **Aplicar Manifestos**:

   - Aplicar ConfigMaps e Secrets
   - Aplicar StatefulSet e Service do MongoDB
   - Aplicar Deployment, Service e HPA da API

4. **Verificação**:
   - Verificar status dos pods
   - Verificar logs se necessário
   - Expor o serviço para acesso

### Boas Práticas

1. **Segurança**:

   - Usar Secrets para dados sensíveis
   - Configurar limites de recursos
   - Implementar probes de saúde

2. **Escalabilidade**:

   - Usar HPA para escalonamento automático
   - Definir limites de recursos apropriados
   - Implementar health checks

3. **Manutenibilidade**:
   - Organizar manifestos em diretórios
   - Usar labels e seletores consistentes
   - Documentar configurações

### Solução de Problemas

1. **Pod não inicia**:

   - Verificar logs: `kubectl logs pod/nome-do-pod`
   - Verificar descrição: `kubectl describe pod/nome-do-pod`
   - Verificar eventos: `kubectl get events`

2. **Imagem não é encontrada**:

   - Verificar se a imagem foi construída no contexto do Minikube
   - Verificar `imagePullPolicy` no deployment

3. **Serviço não está acessível**:
   - Verificar se o serviço está exposto: `kubectl get services`
   - Usar `minikube service nome-do-servico` para expor

### Limpeza

Para remover todos os recursos:

```bash
kubectl delete -f kubernetes/
minikube stop
```

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
- Minikube
- kubectl

### Passos para Execução

1. Clone o repositório

   ```bash
   git clone [url-do-repositorio]
   ```

2. Navegue até a pasta do projeto

   ```bash
   cd martiello
   ```

3. Inicie o Minikube

   ```bash
   minikube start
   ```

4. Configure o Docker para usar o registro do Minikube

   ```bash
   eval $(minikube docker-env)
   ```

5. Construa a imagem da aplicação

   ```bash
   docker build -t martiello-api:latest .
   ```

6. Aplique os manifestos Kubernetes

   ```bash
   kubectl apply -f kubernetes/
   ```

7. Exponha o serviço da API
   ```bash
   minikube service martiello-api-service
   ```

A aplicação estará disponível na URL fornecida pelo comando `minikube service`.

## ⚙️ Configurações

As principais configurações podem ser ajustadas nos arquivos:

- `appsettings.json`: Configurações da aplicação
- `kubernetes/api-configmap.yaml`: Configurações do ambiente
- `kubernetes/db-configmap.yaml`: Configurações do MongoDB

## 📚 Documentação da API

A documentação da API está disponível através do Swagger UI em:

```
http://[minikube-ip]:[porta]/swagger
```

## 👨‍👩‍👧‍👦 Sobre o Desenvolvimento

Este projeto foi desenvolvido inteiramente por mim, conciliando as responsabilidades do trabalho em período integral com a vida familiar, incluindo esposa e filhos. Apesar dos desafios de tempo e das múltiplas responsabilidades, consegui desenvolver as funcionalidades essenciais solicitadas.

Embora existam muitas possibilidades de expansão e melhorias, o projeto atende aos requisitos principais e demonstra a aplicação de boas práticas de arquitetura e desenvolvimento. Espero que os avaliadores possam compreender esse contexto pessoal ao analisar o projeto, considerando o esforço de equilibrar vida profissional, estudos e família durante o desenvolvimento.
