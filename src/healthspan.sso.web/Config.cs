// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace healthspan.sso.web
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
                   new IdentityResource[]
                   {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                   };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("healthspanmd.api"),
                new ApiScope("scope2"),
            };

        public static IEnumerable<Client> Clients(IConfiguration config) =>
            new Client[]
            {
                
                new Client
                {
                    ClientId = config.GetSection("IdentityServer:HealthspanMDWebClient").GetValue<string>("ClientId"),
                    ClientName = "HealthspanMDWebClient",
                    ClientSecrets = { new Secret(config.GetSection("IdentityServer:HealthspanMDWebClient").GetValue<string>("ClientSecret").Sha256()) },

                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    RequireConsent = false,

                    RequirePkce = false,
                    
                 
                    AccessTokenLifetime = 3600, // 1 hours              
                    RefreshTokenUsage = TokenUsage.ReUse,
                    SlidingRefreshTokenLifetime = 7200,    // 2 hours
                    
                    // where to redirect to after login
                    RedirectUris = { config.GetSection("IdentityServer:HealthspanMDWebClient").GetValue<string>("RedirectUris") },

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { config.GetSection("IdentityServer:HealthspanMDWebClient").GetValue<string>("PostLogoutRedirectUris") },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        //"roles"
                        //IdentityServerConstants.StandardScopes.Email
                    },

                    AllowOfflineAccess = true
                },

                new Client
                {
                    ClientId = config.GetSection("IdentityServer:HealthspanMDWebDevClient").GetValue<string>("ClientId"),
                    ClientName = "HealthspanMDWebClient",
                    ClientSecrets = { new Secret(config.GetSection("IdentityServer:HealthspanMDWebDevClient").GetValue<string>("ClientSecret").Sha256()) },

                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    RequireConsent = false,

                    RequirePkce = false,


                    AccessTokenLifetime = 3600, // 1 hours              
                    RefreshTokenUsage = TokenUsage.ReUse,
                    SlidingRefreshTokenLifetime = 7200,    // 2 hours
                    
                    // where to redirect to after login
                    RedirectUris = { config.GetSection("IdentityServer:HealthspanMDWebDevClient").GetValue<string>("RedirectUris") },

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { config.GetSection("IdentityServer:HealthspanMDWebDevClient").GetValue<string>("PostLogoutRedirectUris") },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        //"roles"
                        //IdentityServerConstants.StandardScopes.Email
                    },

                    AllowOfflineAccess = true
                },

            };
    }
}