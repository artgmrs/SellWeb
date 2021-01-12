# SellWeb

**Projeto pessoal**

Um sistema dividido em 3 camadas para controle de fornecedores e produtos, possui cadastro e login (inclusive com integração com Google), autorização em todas as rotas, validação de entidades no front e no back-end.

# Tecnologias utilizadas: 
.NET Core MVC 3.1; Entity Framework Core; Notyf (Notificações flutuantes); API viacep.com.br; FluentValidation; Padrão Repository de acesso à dados, Services, AutoMapper; TagHelpers e ViewComponents.

# Camadas do projeto: App, Business e Data.

# Para configurar o login com o Google

Adicione suas chaves nos User Secrets do projeto, com essa nomenclatura: ClientId e ClientSecret:
```
            //Google
            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    IConfigurationSection googleAuthSection = configuration.GetSection("Authentication:Google");

                    options.ClientId = googleAuthSection["ClientId"];
                    options.ClientSecret = googleAuthSection["ClientSecret"];
                });
```

