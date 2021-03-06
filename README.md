# About Project
This is Online Notepad and user can manage their notes in this application. It has feature to add notes and view notes.

## Prerequisite to run the code
- Visual Studio 2019
- Azure Function CLI
- .Netcore 3.1
- CosmosDB Simulator
- Postman
- PHP



## About Architecture
Application has been design in a such way so that it can be scalled at any point of time in future. 
There are three part of the application like as 
- Frontend layer
    - This layer cover the UI part and below applications would be developed
        - Mobile Application
        - Web Application 
        - Desktop Application 
- Second layer is API Layer
    - This layer is scalable engough to handle the load and trafic simultanously. 
    - This layer has main core business logics in it.
    - It is easy to host with any cloud hosting environment in future.
    - This REST based service will expose secure private endpoints to API gateway and then API gateway will expose the public endpoints to customers/applications.
    - The REST APIs are being designed and developed with best practices like as
        - Created separate layers to break it logically and follow the SOLID principles as well.
            - There are separate models for Frontend response.
            - There separate models for Business layer.
            - And then separate models for Database layer.
        - Database layer is quite genric and can be used any database in future. 
        - Code structure is designed with Interfaces so that more classes could cover with Unit Testing and Mocking with the help of Interfaces.
        - DI is being implemented to resolve the dependancies.     

        ![Alt text](docs/restapi.jpg?raw=true "REST API Design")   
- Third Layer is to manage the older data.
    - There is option to take the offline backup of the older data more than 3 years and keep it in cheapest storage for longer time.
    - If in future some customer want their older data then it can be easily provided to them from the offline storage.
    - This will also save the cose of data storage and also improve the performance of application.
## Architecture Design Diagram 

![Alt text](docs/design.jpg?raw=true "Architecture Design")

## Class Diagram 
![Alt text](docs/classdiagram.jpg?raw=true "Architecture Design")


## Multi Stage Deployment
Considering that Application would be developed and deployed frequently and also new releases would be rolled out. So below deployment strategy is about how to handle such situation and how to roll out the application frequently to production in agility mode.

## Repository Structure and Branching Strategy 

### Feature Branch
- For each feature development there should be always new feature branch from master branch. 
- All developer should have full access to feature branch like as they can do check-in, check-out, run CI-CD etc.
- Feature branch could be deployed on Development Envirnment for testing purpose. 
- Feature branch should not be deployed beyond to Development Envirnment. 
- Developer could run CI-CD pipeline against feature branch and could packaged Alpha release out of it for testing purpose in Development Envirnment.
- Developer should always raise Merge Request to merge code it in master. 
- If MR is reviewed and approved then only it could be merged into master branch.

### Master Branch
- This branch should be protected and no one should be access to directly push the commits to this branch.
- Branch should be always merged to master via MR approval process.
- Master branch will generate Beta version packages and those could be used for Integration Tests or System Tests.
- Master branch could be deployed on System Test Envirnment also.
- Master branch Beta version also should not go beyond to Development Envirnment.

### Release Branch
- Release branch should be created from master branch always
- Release branch would be deployed on all the Envirnments.
- Hotfix branch will be merged into release branch.

### Hotfix Branch:
- Hotfix branch always will be taken from Release Branch.
- Hotfix branch should be merged back to Release branch and delta part into master branch using MR approval process.

### Deployment Design
![Alt text](docs/deployment.jpg?raw=true "Deployment Design")

## Database Schema
CosmosDB has been used into this application. Below are some benifit of it.
- It NoSQL DB and best for performance.
- Easily driven by schema and very flexible for schema related changes in future.
- Easy to migrate to any other NoSQL DB in future.
- More details about CosmosDB is here https://docs.microsoft.com/en-us/azure/cosmos-db/use-cases 

### Sample Schema
```
{
    "id": "7cb28c75-dd57-42c8-b343-bb2cd546a127",
    "noteText": "testing notes",
    "noteName": "test",
    "createdDate": "2021-06-15T20:44:32.8682268+02:00",
    "lastModifiedDate": "2021-06-15T20:44:32.8682929+02:00",
    "padType": "Personal",
    "userId": "12",
    "_type": "NotesContent"
}
```
- The CosmosDB Collection is partitioned collection and `_type` is the partition name.
- We can store multiple types of document in the single collection.

## Project structure

![Alt text](docs/project.jpg?raw=true "Code Structure")

## User Interface
User Interface is just one PHP page `index.php` which is having just one table where all notes are being rendered. The UI part is still pending but concept is there how to build it.. 

![Alt text](docs/uipart.jpg?raw=true "UI")

## Pending Work
The design and conceptual part is done. However the end to end code is not done which is also not expected as part of this. Project structure is also well defined now and developer should just add more classes inside to it. 
Database collection is there and just need to add more schema under to it. Below are some open points to be developed.
- Implementation of Authentication and Authorization
- Register App in AAD
- Implementation of SSO
- Write the Unit Tests for all classes
- Complete UI work
- Add Users schema in DB
- Implementation of User Sign-Up part 
- CI-CD Pipeline

## ARM Template to create Azure Function 

### Parameter File
```
{
  "$schema": "https://schema.management.azure.com/schemas/2019-04-01/deploymentParameters.json#",
  "contentVersion": "1.0.0.0",
  "parameters": {
    "appName": {
      "value": "function-app-notes"
    },
    "storageAccountType": {
      "value": "Standard_LRS"
    },
    "location": {
      "value": "[resourceGroup().location]"
    },
    "appInsightsLocation": {
      "value": null
    },
    "runtime": {
      "value": "dotnet"
    }
  }
}

```

### Template 
```

{
  "$schema": "https://schema.management.azure.com/schemas/2019-04-01/deploymentTemplate.json#",
  "contentVersion": "1.0.0.0",
  "parameters": {
    "appName": {
      "type": "string",
      "defaultValue": "[concat('fnapp', uniqueString(resourceGroup().id))]",
      "metadata": {
        "description": "The name of the function app that you wish to create."
      }
    },
    "storageAccountType": {
      "type": "string",
      "defaultValue": "Standard_LRS",
      "allowedValues": [
        "Standard_LRS",
        "Standard_GRS",
        "Standard_RAGRS"
      ],
      "metadata": {
        "description": "Storage Account type"
      }
    },
    "location": {
      "type": "string",
      "defaultValue": "[resourceGroup().location]",
      "metadata": {
        "description": "Location for all resources."
      }
    },
    "appInsightsLocation": {
      "type": "string",
      "metadata": {
        "description": "Location for Application Insights"
      }
    },
    "runtime": {
      "type": "string",
      "defaultValue": "node",
      "allowedValues": [
        "node",
        "dotnet",
        "java"
      ],
      "metadata": {
        "description": "The language worker runtime to load in the function app."
      }
    }
  },
  "variables": {
    "functionAppName": "[parameters('appName')]",
    "hostingPlanName": "[parameters('appName')]",
    "applicationInsightsName": "[parameters('appName')]",
    "storageAccountName": "[concat(uniquestring(resourceGroup().id), 'azfunctions')]",
    "functionWorkerRuntime": "[parameters('runtime')]"
  },
  "resources": [
    {
      "type": "Microsoft.Storage/storageAccounts",
      "apiVersion": "2019-06-01",
      "name": "[variables('storageAccountName')]",
      "location": "[parameters('location')]",
      "sku": {
        "name": "[parameters('storageAccountType')]"
      },
      "kind": "Storage"
    },
    {
      "type": "Microsoft.Web/serverfarms",
      "apiVersion": "2020-06-01",
      "name": "[variables('hostingPlanName')]",
      "location": "[parameters('location')]",
      "sku": {
        "name": "Y1",
        "tier": "Dynamic"
      },
      "properties": {
        "name": "[variables('hostingPlanName')]",
        "computeMode": "Dynamic"
      }
    },
    {
      "type": "Microsoft.Web/sites",
      "apiVersion": "2020-06-01",
      "name": "[variables('functionAppName')]",
      "location": "[parameters('location')]",
      "kind": "functionapp",
      "dependsOn": [
        "[resourceId('Microsoft.Web/serverfarms', variables('hostingPlanName'))]",
        "[resourceId('Microsoft.Storage/storageAccounts', variables('storageAccountName'))]"
      ],
      "properties": {
        "serverFarmId": "[resourceId('Microsoft.Web/serverfarms', variables('hostingPlanName'))]",
        "siteConfig": {
          "appSettings": [
            {
              "name": "AzureWebJobsStorage",
              "value": "[concat('DefaultEndpointsProtocol=https;AccountName=', variables('storageAccountName'), ';EndpointSuffix=', environment().suffixes.storage, ';AccountKey=',listKeys(resourceId('Microsoft.Storage/storageAccounts', variables('storageAccountName')), '2019-06-01').keys[0].value)]"
            },
            {
              "name": "WEBSITE_CONTENTAZUREFILECONNECTIONSTRING",
              "value": "[concat('DefaultEndpointsProtocol=https;AccountName=', variables('storageAccountName'), ';EndpointSuffix=', environment().suffixes.storage, ';AccountKey=',listKeys(resourceId('Microsoft.Storage/storageAccounts', variables('storageAccountName')), '2019-06-01').keys[0].value)]"
            },
            {
              "name": "WEBSITE_CONTENTSHARE",
              "value": "[toLower(variables('functionAppName'))]"
            },
            {
              "name": "FUNCTIONS_EXTENSION_VERSION",
              "value": "~2"
            },
            {
              "name": "WEBSITE_NODE_DEFAULT_VERSION",
              "value": "~10"
            },
            {
              "name": "APPINSIGHTS_INSTRUMENTATIONKEY",
              "value": "[reference(resourceId('microsoft.insights/components', variables('applicationInsightsName')), '2020-02-02-preview').InstrumentationKey]"
            },
            {
              "name": "FUNCTIONS_WORKER_RUNTIME",
              "value": "[variables('functionWorkerRuntime')]"
            }
          ]
        }
      }
    },
    {
      "type": "microsoft.insights/components",
      "apiVersion": "2020-02-02-preview",
      "name": "[variables('applicationInsightsName')]",
      "location": "[parameters('appInsightsLocation')]",
      "tags": {
        "[concat('hidden-link:', resourceId('Microsoft.Web/sites', variables('applicationInsightsName')))]": "Resource"
      },
      "properties": {
        "ApplicationId": "[variables('applicationInsightsName')]",
        "Request_Source": "IbizaWebAppExtensionCreate"
      }
    }
  ]
}




``` 