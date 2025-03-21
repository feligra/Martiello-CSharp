# 🐳 Arquitetura em Kubernetes - Martiello

## Sobre esta Documentação

Esta documentação detalha a infraestrutura em Kubernetes do sistema **Martiello**, atendendo aos requisitos do **Tech Challenge Fase 02**. O objetivo é suportar uma lanchonete em expansão, garantindo escalabilidade, segurança e eficiência no gerenciamento de pedidos de autoatendimento. Abaixo, descrevemos a arquitetura implantada, os manifestos YAML, os comandos necessários e as boas práticas aplicadas, alinhados ao problema do negócio e aos requisitos técnicos.

## Visão Geral do Kubernetes

**Kubernetes (K8s)** é uma plataforma open-source para orquestração de containers, projetada para automatizar a implantação, escalabilidade e operação de aplicações. No contexto da lanchonete, ele resolve problemas como picos de demanda nos totens e falhas na aplicação, oferecendo:
- **Escalabilidade**: Ajuste automático de recursos para atender mais clientes.
- **Resiliência**: Recuperação rápida de falhas em pods.
- **Gerenciamento**: Controle centralizado de serviços e banco de dados.

## Componentes Utilizados

1. **Pod**: Unidade básica que executa os containers da API e do MongoDB.
2. **Deployment**: Garante que a API esteja sempre disponível com o número desejado de réplicas.
3. **Service**: Expõe a API para os totens e a cozinha.
4. **ConfigMap**: Armazena configurações como ambiente e nome do banco.
5. **Secret**: Protege dados sensíveis (ex.: credenciais do MongoDB e Mercado Pago).
6. **StatefulSet**: Gerencia o MongoDB com persistência de dados.
7. **HorizontalPodAutoscaler (HPA)**: Escala pods da API automaticamente com base na carga.

## Arquitetura Implantada

### Diagrama
```
[Cliente/Totem] --> [Service: martiello-api-service] --> [Deployment: martiello-api]
|                           |
[ConfigMap/Secrets]        [Service: mongodb-service] --> [StatefulSet: mongodb]
|                           |
[HPA]                    [Webhook Mercado Pago]
|
[Kitchen Monitor]
```

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

Aplcando todos os manifestos

```bash
# 1. Primeiro os ConfigMaps e Secrets
kubectl apply -f kubernetes/api-configmap.yaml
kubectl apply -f kubernetes/db-configmap.yaml
kubectl apply -f kubernetes/db-secrets.yaml

# 2. Depois o MongoDB (StatefulSet e Service)
kubectl apply -f kubernetes/db-statefulset.yaml
kubectl apply -f kubernetes/db-service.yaml

# 3. Por fim, a API (Deployment, Service e HPA)
kubectl apply -f kubernetes/api-deployment.yaml
kubectl apply -f kubernetes/api-service.yaml
kubectl apply -f kubernetes/api-hpa.yaml
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


## Alinhamento com o Tech Challenge

- **Escalabilidade**: HPA atende ao requisito de aumento/diminuição de pods.
- **Segurança**: ConfigMap e Secrets separam configurações sensíveis e não sensíveis.
- **Boas Práticas**: Uso de Deployment e Service para expor a API.
- **Documentação**: Diagrama e instruções completas fornecidos.

