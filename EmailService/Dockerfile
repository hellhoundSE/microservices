FROM mcr.microsoft.com/dotnet/sdk:3.1
COPY bin/Release/netcoreapp3.1/publish/ App/
WORKDIR /App
ENTRYPOINT ["dotnet", "EmailMicroservice.dll"]
EXPOSE 7001