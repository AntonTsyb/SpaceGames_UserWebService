## Setup for User Service application in one region
## 1 Setup DynamoDB table with Partition Key email
DynamoDB with table "Users:
```
        [DynamoDBProperty("nick")]
        [DynamoDBHashKey("email")]
        [DynamoDBProperty("logo")]
        [DynamoDBProperty("logokey")]
        [DynamoDBProperty("accesstime")]
        [DynamoDBProperty("rank")]
```
## 2 Setup S3
S3 bucket

AWS Cognito:
```
    email is required field
    email notification is activated
```

Environment variables for Lambda:

| name | description |
| ------ | ------ |
| BucketName | name of S3 bucket used for storing user logos |
| UserPoolClientId |Value of ClientID in AWS Cognito|
| UserPoolClientSecret | Value of Client secret in AWS Cognito |
| UserPoolId | id of user pool in AWS Cognito |
| Region | region code (eu-west-3) |
| ASPNETCORE_ENVIRONMENT | can be empty. DEVELOPMENT can be used to access swagger and error details page |

## Home work

All comments starting with // Home work
are supposed to be completed depending on level 

## Low-level 
add functionality to delete users from DynamoDb and from Cognito

## Middle level
Add validation for NickName.
Find all places (ctrl+shift+F) with "// Home work NickName".
### Mandatory for Middle: 
Add logic to Scan DynamoDbTable by nick name.
### Optional for Middle: 
On SignUp and Edit User throw Exception if nickName is not unique.
Use the easiest way to implement, it can be not the best. It is studying.
It can be using scan. It can be changing structure of table.

## Advanced level
Adding restriction to delete users only with admin rights.
Admin rights mean users who are added to group with name "Admin" in Cognito.
To do this one should decode IdToken and check User claims like "cognito:groups"
https://learn.microsoft.com/en-us/aspnet/core/security/authorization/policies?view=aspnetcore-6.0
https://medium.com/@samjwright/creating-a-custom-authorization-policy-in-net-core-5f2b053ce972

Useful links:
https://docs.amazonaws.cn/en_us/amazondynamodb/latest/developerguide/WorkingWithItemsDocumentClasses.html
https://docs.amazonaws.cn/en_us/amazondynamodb/latest/developerguide/ItemCRUDDotNetDocumentAPI.html
https://docs.amazonaws.cn/en_us/amazondynamodb/latest/developerguide/ScanMidLevelDotNet.html
https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Expressions.OperatorsAndFunctions.html

Code examples can be found here (following links inside Amazon doc)
https://docs.aws.amazon.com/sdk-for-net/v3/developer-guide/working-with-aws-services.html

## Important 
Before launch locally, go to  Properties-> launchSetting.json and fill all value of properties:
        "BucketName": "",
        "UserPoolClientId": "",
        "UserPoolClientSecret": "",
        "UserPoolId": "",
        "Region": "",


All [Authorize] methods can be tested with one of ways:
1. In postman adding Request Header  "Authorization" with value Bearer SecurityToken
where Bearer is mandatory word, SecurityToken is value you will get after SignIn request

2 in Swagger. After SignIn Request copy SecurityToken value and insert this in Authorize window (see screnshot Swagger.png)


Good luck!


# ASP.NET Core Web API Serverless Application

This project shows how to run an ASP.NET Core Web API project as an AWS Lambda exposed through Amazon API Gateway. The NuGet package [Amazon.Lambda.AspNetCoreServer](https://www.nuget.org/packages/Amazon.Lambda.AspNetCoreServer) contains a Lambda function that is used to translate requests from API Gateway into the ASP.NET Core framework and then the responses from ASP.NET Core back to API Gateway.


For more information about how the Amazon.Lambda.AspNetCoreServer package works and how to extend its behavior view its [README](https://github.com/aws/aws-lambda-dotnet/blob/master/Libraries/src/Amazon.Lambda.AspNetCoreServer/README.md) file in GitHub.


### Configuring for API Gateway HTTP API ###

1.0 payload format is used.
CloudFormation resource with should define event type as `Api`

### Configuring for Application Load Balancer ###

Application is not configured to use Application Load Balancer

### Project Files ###

* serverless.template - an AWS CloudFormation Serverless Application Model template file for declaring your Serverless functions and other AWS resources
* aws-lambda-tools-defaults.json - default argument settings for use with Visual Studio and command line deployment tools for AWS
* LambdaEntryPoint.cs - class that derives from **Amazon.Lambda.AspNetCoreServer.APIGatewayProxyFunction**. The code in 
this file bootstraps the ASP.NET Core hosting framework. The Lambda function is defined in the base class.
Change the base class to **Amazon.Lambda.AspNetCoreServer.ApplicationLoadBalancerFunction** when using an 
Application Load Balancer.
* LocalEntryPoint.cs - for local development this contains the executable Main function which bootstraps the ASP.NET Core hosting framework with Kestrel, as for typical ASP.NET Core applications.
* Startup.cs - usual ASP.NET Core Startup class used to configure the services ASP.NET Core will use.
* web.config - used for local development.
* Controllers\ValuesController - example Web API controller


## Here are some steps to follow to get started from the command line:

Once you have edited your template and code you can deploy your application using the [Amazon.Lambda.Tools Global Tool](https://github.com/aws/aws-extensions-for-dotnet-cli#aws-lambda-amazonlambdatools) from the command line.

Install Amazon.Lambda.Tools Global Tools if not already installed.
```
    dotnet tool install -g Amazon.Lambda.Tools
```

If already installed check if new version is available.
```
    dotnet tool update -g Amazon.Lambda.Tools
```

Execute unit tests
```
    cd "SpaceGame.UserService.API/test/SpaceGame.UserService.API.Tests"
    dotnet test
```

Deploy application
```
    cd "SpaceGame.UserService.API/src/SpaceGame.UserService.API"
    dotnet lambda deploy-serverless
```

