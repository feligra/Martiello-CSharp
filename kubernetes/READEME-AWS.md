# Documentação de Implantação - Projeto Martiello na AWS

## Índice

1. [Visão Geral](#visão-geral)
2. [Pré-requisitos](#pré-requisitos)
3. [Estrutura do Projeto](#estrutura-do-projeto)
4. [Implantação Inicial](#implantação-inicial)
5. [Atualizações e Manutenção](#atualizações-e-manutenção)
6. [Monitoramento](#monitoramento)
7. [Troubleshooting](#troubleshooting)
8. [Comandos Úteis](#comandos-úteis)

## Visão Geral

Este projeto consiste em uma API .NET Core com banco de dados MongoDB, implantado na AWS usando Kubernetes (EKS). A arquitetura inclui:

- **API .NET Core**: Aplicação principal
- **MongoDB**: Banco de dados NoSQL
- **Kubernetes**: Orquestração de contêineres
- **AWS EKS**: Serviço gerenciado de Kubernetes
- **AWS EBS**: Armazenamento persistente para o MongoDB

## Pré-requisitos

Para trabalhar com este projeto, você precisará:

- AWS CLI configurado com credenciais apropriadas
- kubectl instalado e configurado
- eksctl instalado
- Docker instalado (para builds locais)
- Git para controle de versão

## Estrutura do Projeto

```
kubernetes/
├── api-configmap.yaml    # Configurações da API
├── api-deployment.yaml   # Deployment da API
├── api-hpa.yaml          # Configuração de auto-scaling
├── api-service.yaml      # Serviço para expor a API
├── db-configmap.yaml     # Configurações do MongoDB
├── db-secrets.yaml       # Secrets do MongoDB
├── db-service.yaml       # Serviço do MongoDB
├── db-statefulset.yaml   # StatefulSet do MongoDB
└── cluster.yml           # Configuração do cluster EKS
```

## Implantação Inicial

### 1. Configuração da AWS

Primeiro, configure suas credenciais AWS:

```bash
aws configure
```

Insira suas credenciais:

```
AWS Access Key ID: AKIAYKFQRA7PDVKTA4EC
AWS Secret Access Key: ************
Default region name: us-east-1
Default output format: json
```

### 2. Criação do Cluster EKS

```bash
# Criar o cluster EKS usando a configuração em cluster.yml
eksctl create cluster -f kubernetes/cluster.yml

# Verificar se o cluster foi criado corretamente
eksctl get cluster --region us-east-1

# Configurar o kubectl para usar o cluster
aws eks update-kubeconfig --name martiello-cluster --region us-east-1

# Verificar os nós do cluster
kubectl get nodes
```

### 3. Configuração do EBS CSI Driver

Para permitir o uso de volumes persistentes no EKS:

```bash
# Habilitar OIDC provider para o cluster
eksctl utils associate-iam-oidc-provider --region=us-east-1 --cluster=martiello-cluster --approve

# Criar conta de serviço IAM para o EBS CSI Driver
eksctl create iamserviceaccount \
  --name ebs-csi-controller-sa \
  --namespace kube-system \
  --cluster martiello-cluster \
  --attach-policy-arn arn:aws:iam::aws:policy/service-role/AmazonEBSCSIDriverPolicy \
  --approve \
  --region us-east-1

# Instalar o EBS CSI Driver como addon do EKS
eksctl create addon --name aws-ebs-csi-driver --cluster martiello-cluster --service-account-role-arn <ROLE_ARN> --force --region us-east-1

# Verificar se o driver foi instalado corretamente
kubectl get pods -n kube-system | grep ebs
```

### 4. Implantação do MongoDB

```bash
# Aplicar ConfigMap e Secrets
kubectl apply -f kubernetes/db-configmap.yaml
kubectl apply -f kubernetes/db-secrets.yaml

# Criar o serviço do MongoDB
kubectl apply -f kubernetes/db-service.yaml

# Implantar o StatefulSet do MongoDB
kubectl apply -f kubernetes/db-statefulset.yaml

# Verificar se o pod do MongoDB está em execução
kubectl get pods
```

**Nota**: Se o pod do MongoDB ficar pendente, pode ser necessário verificar as permissões do EBS CSI Driver ou usar um volume emptyDir temporariamente.

### 5. Implantação da API

```bash
# Aplicar ConfigMap
kubectl apply -f kubernetes/api-configmap.yaml

# Criar o deployment da API
kubectl apply -f kubernetes/api-deployment.yaml

# Criar o serviço da API (LoadBalancer)
kubectl apply -f kubernetes/api-service.yaml

# Configurar auto-scaling
kubectl apply -f kubernetes/api-hpa.yaml

# Verificar se o pod da API está em execução
kubectl get pods

# Obter o endereço do LoadBalancer
kubectl get svc martiello-api-service
```

### 6. Acessando a API

Após a implantação, a API estará disponível através do LoadBalancer da AWS:

```bash
# Obter o endereço do LoadBalancer
kubectl get svc martiello-api-service
```

O Swagger da API estará disponível em:

```
http://<LOADBALANCER_URL>/swagger
```

## Atualizações e Manutenção

### Atualização do Código da API

1. **Fazer alterações no código**

2. **Gerar nova imagem Docker**

```bash
# Build da nova imagem
docker build -t 571600865246.dkr.ecr.us-east-1.amazonaws.com/martiello-api:latest .

# Autenticar no ECR
aws ecr get-login-password --region us-east-1 | docker login --username AWS --password-stdin 571600865246.dkr.ecr.us-east-1.amazonaws.com

# Push da imagem
docker push 571600865246.dkr.ecr.us-east-1.amazonaws.com/martiello-api:latest
```

3. **Atualizar o deployment**

- Editar `api-deployment.yaml` com a nova versão da imagem

```bash
kubectl apply -f kubernetes/api-deployment.yaml
```

4. **Verificar o rollout**

```bash
kubectl rollout status deployment/martiello-api
```

### Atualização de Configurações

1. **Alterações no ConfigMap da API**

```bash
# Editar api-configmap.yaml
kubectl apply -f kubernetes/api-configmap.yaml

# Reiniciar os pods para aplicar as novas configurações
kubectl rollout restart deployment martiello-api
```

2. **Alterações no MongoDB**

```bash
# Editar configurações do MongoDB
kubectl apply -f kubernetes/db-configmap.yaml
kubectl apply -f kubernetes/db-secrets.yaml

# Reiniciar o StatefulSet
kubectl rollout restart statefulset mongodb
```

### Escalar a Aplicação

O HPA (Horizontal Pod Autoscaler) já está configurado para escalar automaticamente com base na utilização de CPU. Para escalar manualmente:

```bash
# Escalar para um número específico de réplicas
kubectl scale deployment martiello-api --replicas=3
```

## Monitoramento

### Verificar Status dos Componentes

```bash
# Status dos pods
kubectl get pods

# Status dos serviços
kubectl get svc

# Logs da API
kubectl logs -f deployment/martiello-api

# Logs do MongoDB
kubectl logs -f statefulset/mongodb
```

### Métricas e Dashboards

Para monitoramento mais avançado, considere instalar:

- Prometheus para coleta de métricas
- Grafana para visualização
- ELK Stack para logs centralizados

## Troubleshooting

### Pod MongoDB Pendente

Se o pod do MongoDB ficar pendente, verificar:

1. **Status do PVC**

```bash
kubectl get pvc
kubectl describe pvc mongodb-data-mongodb-0
```

2. **Eventos do pod**

```bash
kubectl describe pod mongodb-0
```

3. **Logs do EBS CSI Driver**

```bash
kubectl logs -n kube-system -l app=ebs-csi-controller
```

4. **Solução temporária**: Usar emptyDir
   - Editar `db-statefulset.yaml` para usar emptyDir em vez de PVC
   - Aplicar as alterações: `kubectl apply -f kubernetes/db-statefulset.yaml`

### API Inacessível

1. **Verificar status do pod**

```bash
kubectl get pods
kubectl describe pod <pod-name>
```

2. **Verificar logs**

```bash
kubectl logs deployment/martiello-api
```

3. **Verificar serviço**

```bash
kubectl describe svc martiello-api-service
```

4. **Verificar regras de segurança da AWS**
   - Verificar se as portas necessárias estão abertas no Security Group

### Problemas de Conexão com MongoDB

1. **Verificar secrets**

```bash
kubectl get secrets
kubectl describe secret mongodb-secret
```

2. **Verificar configurações**

```bash
kubectl get configmap
kubectl describe configmap mongodb-config
```

3. **Testar conexão dentro do cluster**

```bash
kubectl exec -it <pod-name> -- mongosh
```

## Comandos Úteis

```bash
# Verificar logs em tempo real
kubectl logs -f deployment/martiello-api

# Escalar manualmente
kubectl scale deployment martiello-api --replicas=3

# Verificar histórico de deployments
kubectl rollout history deployment/martiello-api

# Rollback para versão anterior
kubectl rollout undo deployment/martiello-api

# Executar comando no pod do MongoDB
kubectl exec -it mongodb-0 -- mongosh

# Verificar eventos do cluster
kubectl get events --sort-by='.lastTimestamp'

# Verificar uso de recursos
kubectl top nodes
kubectl top pods

# Acessar shell em um pod
kubectl exec -it <pod-name> -- /bin/bash

# Reiniciar deployment
kubectl rollout restart deployment martiello-api
```

## Limpeza de Recursos

Quando não precisar mais do ambiente:

```bash
# Excluir todos os recursos do Kubernetes
kubectl delete -f kubernetes/

# Excluir o cluster EKS
eksctl delete cluster --name martiello-cluster --region us-east-1
```

---

**Importante**: Mantenha este documento atualizado conforme o projeto evolui e novas práticas são adotadas.
