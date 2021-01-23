FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build

# Docker file arguments
ARG userid=200
ARG username=app
ARG homedir=/home/app

RUN groupadd -g $userid $username
RUN useradd -m -r -u $userid -g $userid -d $homedir -s /sbin/nologin -c "Docker image user" $username

WORKDIR $homedir

ADD PaymentGatewayApi .

RUN chown -R $userid:$userid .

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Docker
ENV HOME $homedir
EXPOSE 8080
USER $userid:$userid

ENTRYPOINT ["dotnet", "PaymentGateway.Api.dll"]