using GreenPipes;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailMictoservice {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {

            //services.AddScoped(provider => {
            //    return new EmailService("26ec68c592d70fe330d0d4e7a538f497", "e5e971781eeb3fb7f17207cb677e111b", "aop-notification@gmail.com");
            //});
            services.AddScoped<EmailService>();

            services.AddControllers();

            services.AddMassTransit(x => {
                x.AddConsumer<EmailService>();
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cur => {
                    cur.UseHealthCheck(provider);
                    cur.Host(new Uri("rabbitmq://localhost"), h => {
                        h.Username("guest");
                        h.Password("guest");
                    });
                    cur.ReceiveEndpoint("emailQueue", oq => {
                        oq.UseMessageRetry(r => r.Interval(1, 100));
                        oq.ConfigureConsumer<EmailService>(provider);
                    });
                }));
            });
            services.AddMassTransitHostedService();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
