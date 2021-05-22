using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeEntrySystem.Models
{
    public static class TimeEntryInitializer
    {

        public static List<Tuple<string, string>> GetEmployees()
        {
            return new List<Tuple<string, string>>()
            {
                new Tuple<string, string>("Chuck", "Bartowski"),
                new Tuple<string, string>("John","Casy"),
                new Tuple<string, string>("Morgan","Grimes")
            };
        }
        public static List<string> GetProjects()
        {
            return new List<string>()
            {
                "Project Intersect",
                "Project Helicopter",
                "Project Tango",
                "Project Wookiee",
                "Project Sizzling Shrimp",
                "Project Sandworm",
                "Project Alma Mater",
                "Project Truth" ,
                "Project Imported Hard Salami" ,
                "Project Nemesis" ,
                "Project Crown Vic" ,
                "Project Undercover Lover" ,
                "Project Marlin",
                "Project First Date",
                "Project Seduction",
                "Project Break-Up",
                "Project Cougars",
                "Project Tom Sawyer",
                "Project Ex",
                "Project Fat Lady",
                "Project Gravitron",
                "Project Sensei",
                "Project DeLorean",
                "Project Santa Claus",
                "Project Third Dimension",
                "Project Suburbs",
                "Project Best Friend",
                "Project Beefcake",
                "Project Lethal Weapon",
                "Project Predator",
                "Project Broken Heart",
                "Project Dream Job",
                "Project First Kill",
                "Project Colonel",
                "Project Ring",
                "Project Pink Slip",
                "Project Three Words",
                "Project Angel de la Muerte",
                "Project Operation Awesome",
                "Project First Class",
                "Project Nacho Sampler",
                "Project Mask",
                "Project Fake Name",
                "Project Beard",
                "Project Tic Tac",
                "Project Final Exam",
                "Project American Hero",
                "Project Other Guy",
                "Project Honeymooners",
                "Project Role Models",
                "Project Tooth",
                "Project Living Dead",
                "Project Subway",
                "Project Ring: Part II"
            };
        }
    }
}