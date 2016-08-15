﻿// Copyright 2016 the pokefans authors. See copying.md for legal info.
using Pokefans.App_Start;
using Pokefans.Data;
using Pokefans.Util;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Practices.Unity;

namespace Pokefans.Areas.fanart
{
    public class fanartAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "fanart";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            RouteValueDictionary dataTokens = new RouteValueDictionary();
            dataTokens["Namespaces"] = new string[] { "Pokefans.Areas.fanart.Controllers" };
            dataTokens["UseNamespaceFallback"] = false;
            var route = new DomainRoute(
                            "fanart." + ConfigurationManager.AppSettings["Domain"],
                            "{controller}/{action}/{id}",
                            new { action = "Index", controller = "FanartHome", id = UrlParameter.Optional },
                            dataTokens
                        );

            context.Routes.Add("fanartHome", new DomainRoute(
                            "fanart." + ConfigurationManager.AppSettings["Domain"],
                            "",
                            new { action = "New", controller = "FanartHome" }
                ));

            context.Routes.Add("fanartNeu", new DomainRoute(
                            "fanart." + ConfigurationManager.AppSettings["Domain"],
                            "neu/{image}",
                            new { action = "New", controller = "FanartHome", image = UrlParameter.Optional }
                ));

            context.Routes.Add("fanartPopular", new DomainRoute(
                            "fanart." + ConfigurationManager.AppSettings["Domain"],
                            "beliebt/{image}",
                            new { action = "Popular", controller = "FanartHome", image = UrlParameter.Optional }
                ));

            context.Routes.Add("fanartUpload", new DomainRoute(
                            "fanart." + ConfigurationManager.AppSettings["Domain"],
                            "verwaltung/upload",
                            new { action = "Upload", controller = "Manage" },
                            dataTokens
                ));
            context.Routes.Add("fanartManage", new DomainRoute(
                            "fanart." + ConfigurationManager.AppSettings["Domain"],
                            "verwaltung",
                            new { action = "Index", controller = "Manage" },
                            dataTokens
                ));
            context.Routes.Add("fanartManageSingle", new DomainRoute(
                            "fanart." + ConfigurationManager.AppSettings["Domain"],
                            "verwaltung/einreichung/{id}",
                            new { action = "Edit", controller = "Manage" },
                            dataTokens
                ));
            context.Routes.Add("fanartUser", new DomainRoute(
                            "fanart." + ConfigurationManager.AppSettings["Domain"],
                            "user/{id}",
                            new { action = "User", controller = "FanartHome" }
                ));
            context.Routes.Add("fanartUserRss", new DomainRoute(
                            "fanart." + ConfigurationManager.AppSettings["Domain"],
                            "user/{id}/feed.rss",
                            new { action = "UserRss", controller = "FanartHome" }
                ));
            context.Routes.Add("fanartGlobalRss", new DomainRoute(
                            "fanart." + ConfigurationManager.AppSettings["Domain"],
                            "index/feed.rss",
                            new { action = "Rss", controller = "FanartHome" }
                ));

            // now, this is the fun part: we "dynamically" (as in: data source is the database,
            // but it will only be registered on app start) register routes for the cateogries.
            // each categories gets a seperate index and RSS feed, to differentiate from the
            // "normal" search queries.

            Entities db = UnityConfig.GetConfiguredContainer().Resolve<Entities>();

            foreach (var cat in db.FanartCategories)
            {
                context.Routes.Add("fanart" + cat.Uri + "Rss", new DomainRoute(
                        "fanart." + ConfigurationManager.AppSettings["Domain"],
                        cat.Uri + "/feed.rss",
                        new { action = "CategoryRss", controller = "FanartHome", id = cat.Uri }
                    ));
                context.Routes.Add("fanart" + cat.Uri + "Display", new DomainRoute(
                        "fanart." + ConfigurationManager.AppSettings["Domain"],
                        cat.Uri + "/{image}",
                        new { action = "Category", controller = "FanartHome", id = cat.Uri, image = UrlParameter.Optional }
                    ));
            }

            // API
            context.Routes.Add("fanartApiImage", new DomainRoute(
                            "fanart." + ConfigurationManager.AppSettings["Domain"],
                            "api/v1/{id}",
                            new { action = "ImageApi", controller = "FanartHome" }
                ));

            context.Routes.Add("fanartApiList", new DomainRoute(
                            "fanart" + ConfigurationManager.AppSettings["Domain"],
                            "api/v1/list/{index}/{start}",
                            new { action = "ListApi", controller = "FanartHome" }
                ));
            context.Routes.Add("fanartApiSearch", new DomainRoute(
                            "fanart." + ConfigurationManager.AppSettings["Domain"],
                            "api/v1/search/{term}/{start}",
                            new { action = "SerachApi", controller = "FanartHome" }
                ));
            context.Routes.Add("fanartApiTags", new DomainRoute(
                            "fanart." + ConfigurationManager.AppSettings["Domain"],
                            "api/v1/tags/{q}",
                            new { action = "GetTags", Controller = "Manage", q = UrlParameter.Optional },
                            dataTokens
                ));

            context.Routes.Add("fanartdefault", route);
        }
    }
}