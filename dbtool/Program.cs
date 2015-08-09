﻿using System;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Data.Entity.Migrations.Design;
using System.IO;
using System.Resources;
using Pokefans.Data.Migrations;

namespace dbtool
{
    class MainClass
    {
        public static void Main (string[] args)
        {
            switch (args [0]) 
            {
                case "Add-Migration":
                    addMigration(args);
                    break;
                case "Update-Database":
                    updateDb();
                    break;
                default:
                    printHelp();
                    break;
            }
        }

        static void printHelp()
        {
            Console.WriteLine("dbtool.exe Usage:");
            Console.WriteLine();
            Console.WriteLine("dbtool Add-Migration name [path]");
            Console.WriteLine("   to add a new migration");
            Console.WriteLine();
            Console.WriteLine("dbtool Update-Database");
            Console.WriteLine("   to apply migrations to the database");
            Console.WriteLine();
            Console.WriteLine("So it's basically the same as within the visual studio command line, with some additional *nix flavor.");
        }

        static void addMigration(string[] args)
        {
            var config = new Configuration();
            var scaffolder = new MigrationScaffolder(config);
            var migration = scaffolder.Scaffold(args[1]);

            File.WriteAllText(Path.Combine(args[2], migration.MigrationId + ".cs"), migration.UserCode);

            File.WriteAllText(Path.Combine(args[2], migration.MigrationId + ".Designer.cs"), migration.DesignerCode);

            using (var writer = new ResXResourceWriter(Path.Combine(args[2], migration.MigrationId + ".resx")))
            {
                foreach (var resource in migration.Resources)
                {
                    writer.AddResource(resource.Key, resource.Value);
                }
            }
        }

        static void updateDb()
        {
            var config = new Configuration();
            DbMigrator migrator = new DbMigrator(config);

            int migrationcount = migrator.GetPendingMigrations().Count();

            if (migrationcount <= 0)
            {
                Console.WriteLine("Yay! Everything already up to date.");
                return;
            }

            Console.WriteLine("Applying {0} pending migrations - Please wait...", migrationcount);

            migrator.Update();
        }
    }
}