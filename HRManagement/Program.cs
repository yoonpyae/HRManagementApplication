﻿using HRManagement.Models;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

static bool IsOriginAllowed(string origin)
{
    Uri uri = new(origin);
    bool isAllowed = uri.Host.Equals("localhost", StringComparison.OrdinalIgnoreCase);
    return isAllowed;
}

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// 1️⃣ Configure Database Context with Identity
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



//// 3️⃣ Configure Authentication with JWT
//IConfigurationSection jwtSettings = builder.Configuration.GetSection("Jwt");
//byte[] key = Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? throw new InvalidOperationException("JWT Key not found"));

//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//})
//.AddJwtBearer(options =>
//{
//    options.RequireHttpsMetadata = true;
//    options.SaveToken = true;
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuerSigningKey = true,
//        IssuerSigningKey = new SymmetricSecurityKey(key),
//        ValidateIssuer = true,
//        ValidIssuer = jwtSettings["Issuer"],
//        ValidateAudience = true,
//        ValidAudience = jwtSettings["Audience"],
//        ValidateLifetime = true,
//        ClockSkew = TimeSpan.Zero
//    };
//});

// 4️⃣ Add Controllers & OpenAPI
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Enable Cors
_ = builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowedCorsOrigins",
        builder =>
        {
            _ = builder
                .SetIsOriginAllowed(IsOriginAllowed)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

WebApplication app = builder.Build();

// 5️⃣ Enable OpenAPI in Development
if (app.Environment.IsDevelopment())
{
    _ = app.MapOpenApi();
    _ = app.MapScalarApiReference("/docs/scalar");
}

// 6️⃣ Enable Middleware for Authentication & Authorization
app.UseHttpsRedirection();

app.UseCors("AllowedCorsOrigins");

app.UseAuthentication(); // 🔥 This is required for Identity
app.UseAuthorization();

app.MapControllers();

app.Run();
