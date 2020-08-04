FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["DevSkill.TenantPro.Web/*.csproj", "DevSkill.TenantPro.Web/"]
COPY ["DevSkill.TenantPro.Core/*.csproj", "DevSkill.TenantPro.Core/"]
COPY ["DevSkill.TenantPro.Tenantship/*.csproj", "DevSkill.TenantPro.Tenantship/"]
COPY ["DevSkill.TenantPro.Billing/*.csproj", "DevSkill.TenantPro.Billing/"]
RUN dotnet restore "DevSkill.TenantPro.Web/DevSkill.TenantPro.Web.csproj"
COPY . .
WORKDIR "/src/DevSkill.TenantPro.Web"
RUN dotnet build "DevSkill.TenantPro.Web.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "DevSkill.TenantPro.Web.csproj" -c Release -o /app

FROM build AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "DevSkill.TenantPro.Web.dll"]