using IdentityServer.Data;
using IdentityServer.Models;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace IdentityServer
{
    public class SeedData
    {
        //public static void PersistendGrantMigration(IServiceCollection services)
        //{
        //    using (var serviceProvider = services.BuildServiceProvider())
        //    {
        //        using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
        //        {
        //            var context = scope.ServiceProvider.GetService<PersistedGrantDbContext>();
        //            context.Database.Migrate();
        //        }
        //    }
        //}


        public static void InitDB(string connectionString)
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddDbContextPool<ApplicationDbContext>(options =>
               options.UseSqlServer(connectionString));

            using (var serviceProvider = services.BuildServiceProvider())
            {
                using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
                    context.Database.Migrate();
                }
            }
        }

        public static void EnsureSeedData(string connectionString)
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(connectionString)
            );

            services.AddIdentity<ApplicationUser, IdentityRole<int>>()
                .AddRoles<IdentityRole<int>>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            using (var serviceProvider = services.BuildServiceProvider())
            {
                using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
                    context.Database.Migrate();

                    //create the roles and seed them to the database
                    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
                    var roleCheck = roleManager.RoleExistsAsync("Admin").Result;
                    if (!roleCheck)
                    {
                        IdentityResult result = roleManager.CreateAsync(new IdentityRole<int>("Admin")).Result;
                    }

                    roleCheck = roleManager.RoleExistsAsync("Guest").Result;
                    if (!roleCheck)
                    {
                        IdentityResult result = roleManager.CreateAsync(new IdentityRole<int>("Guest")).Result;
                    }

                    roleCheck = roleManager.RoleExistsAsync("User").Result;
                    if (!roleCheck)
                    {
                        IdentityResult result = roleManager.CreateAsync(new IdentityRole<int>("User")).Result;
                    }

                    var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                    var admin = userMgr.FindByEmailAsync("abhishek_rai@hcl.com").Result;
                    if (admin == null)
                    {
                        admin = new ApplicationUser
                        {
                            UserName = "abhishek_rai@hcl.com",
                            Email = "abhishek_rai@hcl.com",
                            EmailConfirmed = true,
                            DisplayName = "Abhishek Rai",
                            Gender = "Male"
                        };

                        var result = userMgr.CreateAsync(admin, "Pass123$").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        ApplicationUser adminUser = userMgr.FindByEmailAsync("abhishek_rai@hcl.com").Result;
                        result = userMgr.AddToRoleAsync(adminUser, "Admin").Result;

                    }
                }
            }
        }
    }
}
