{
    "repository": {
        "repositoryArn": "arn:aws:ecr:us-east-1:571600865246:repository/martiello-api",
        "registryId": "571600865246",
        "repositoryName": "martiello-api",
        "repositoryUri": "571600865246.dkr.ecr.us-east-1.amazonaws.com/martiello-api",
        "createdAt": "2025-03-07T17:19:45.283000-03:00",
        "imageTagMutability": "MUTABLE",
        "imageScanningConfiguration": {
            "scanOnPush": false
        },
        "encryptionConfiguration": {
            "encryptionType": "AES256"
        }
    }
}
