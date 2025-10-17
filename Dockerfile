
# Stage 1 : build construccion 

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
#WORKDIR /the/workdir/path

WORKDIR /app

# restaurar depencias 

COPY *.csproj ./

# copiar el resto del codigo y copilar 

COPY . ./
# crea o se entra a la carpeta publish 

# RUN dotnet publish -c  Release 0- /app/publish
Run dotnet publish -c Release -o /app/publish



### STAGE 2: runtime ###

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .


# exponer el puerdto donde ser va a correr la app

EXPOSE 8080

#Ejecutar la app

ENTRYPOINT [ "dotnet", "medicos_c.dll" ]

# para construir la imagen 

## docker run -d -p 8080:8080 --name medicos_app medicos_c
