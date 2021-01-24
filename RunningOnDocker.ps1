dotnet publish -c release -o ./PaymentGatewayApi --no-restore .\src\PaymentGateway.Api\
docker build --rm -t paymentgatewayapi . 
docker run -i --tty --rm -p 2345:8080 paymentgatewayapi bash