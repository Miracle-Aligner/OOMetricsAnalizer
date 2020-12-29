using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace OOPMetrics
{
    struct Metrics
    {
        public int DIT;
        public int NOC;
        public decimal MIF;
        public decimal MHF;
        public decimal AHF;
        public decimal AIF;
        public decimal POF;
    }

    struct ClassMetrics
    {
        public Metrics _Metrics;
        public String ClassName;
    }


    static class MetricsAnalizer
    {

        private static int GetDIT(Assembly assembly)
        {
            var DIT = 0;

            foreach (Type type in assembly.GetTypes())
            {
                var depth = 0;
                for (var current = type.BaseType; current != null; current = current.BaseType, depth++);

                DIT = Math.Max(DIT, depth);
            }

            return DIT;
        }

        private static int GetDIT(Type type)
        {
            var DIT = 0;

            var depth = 0;
            for (var current = type.BaseType; current != null; current = current.BaseType, depth++) ;

            DIT = Math.Max(DIT, depth);
            // Console.WriteLine("{0}", type.BaseType);
            return DIT;
        }

        private static int GetNOC(Assembly assembly)
        {
            var NOC = 0;
            var assemblyTypes = assembly.GetTypes();

            foreach (Type type in assemblyTypes)
            {
                NOC = Math.Max(NOC, assemblyTypes.Count(t => type.Equals(t.BaseType)));
            }

            return NOC;
        }

        private static int GetNOC(Type type)
        {
            var NOC = 0;
            var nestedTypes = type.GetNestedTypes();

            foreach (Type ntype in nestedTypes)
            {
                NOC = Math.Max(NOC, nestedTypes.Count(t => ntype.Equals(t.BaseType)));
            }

            return NOC;
        }


        private static decimal GetMIF(Assembly assembly)
        {
            int MethodsAll = 0;
            int MethodsInherited = 0;

            foreach (Type type in assembly.GetTypes())
            {
                MethodsAll += type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance |
                                              BindingFlags.Static | BindingFlags.Public)
                                  .Count();

                MethodsInherited += type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic |
                                      BindingFlags.Instance | BindingFlags.Static |
                                      BindingFlags.FlattenHierarchy)
                          .Where(m => m.DeclaringType != type && m.GetBaseDefinition() != m)
                          .Count();
            }

            return MethodsAll != 0 ? (decimal)MethodsInherited / MethodsAll : 0;
        }

        private static decimal GetMIF(Type type)
        {
            int MethodsAll = 0;
            int MethodsInherited = 0;

            MethodsAll = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance |
                                              BindingFlags.Static | BindingFlags.Public)
                                  .Count();

            MethodsInherited = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic |
                                  BindingFlags.Instance | BindingFlags.Static |
                                  BindingFlags.FlattenHierarchy)
                      .Where(m => m.DeclaringType != type && m.GetBaseDefinition() != m)
                      .Count();

            // Console.WriteLine("{0}, {1}", MethodsAll, MethodsInherited);

            
            var allMethods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance |
                                              BindingFlags.Static | BindingFlags.Public);

            /*Console.WriteLine("All:");
            foreach (var method in allMethods)
            {
                Console.WriteLine("{0}", method);
            }
            

            var inheritedMethods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic |
                                  BindingFlags.Instance | BindingFlags.Static |
                                  BindingFlags.FlattenHierarchy)
                      .Where(m => m.DeclaringType != type && m.GetBaseDefinition() != m);
            Console.WriteLine("Inherited:");
            foreach (var method in inheritedMethods)
            {
                Console.WriteLine("{0}", method);
            }*/
            

            return MethodsAll != 0 ? (decimal)MethodsInherited / MethodsAll : 0;
        }

        private static decimal GetMHF(Assembly assembly)
        {
            int MethodsVisible = 0;
            int MethodsHidden = 0;

            foreach (Type type in assembly.GetTypes())
            {
                MethodsVisible += type.GetMethods().Count();
                MethodsHidden += type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static).Count();
            }

            var allMethods = MethodsHidden + MethodsVisible;

            return allMethods != 0 ? (decimal)MethodsHidden / allMethods : 0;
        }

        private static decimal GetMHF(Type type)
        {
            int MethodsVisible = 0;
            int MethodsHidden = 0;

            MethodsVisible = type.GetMethods().Count();
            MethodsHidden = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static).Count();

            var allMethods = MethodsHidden + MethodsVisible;
           
            return allMethods != 0 ? (decimal)MethodsHidden / allMethods : 0;
        }

        private static decimal GetAHF(Assembly assembly)
        {
            int AttributesAll = 0;
            int AttributesHidden = 0;

            foreach (Type type in assembly.GetTypes())
            {
                AttributesAll += type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Static |
                                                BindingFlags.Public | BindingFlags.NonPublic |
                                                BindingFlags.Static | BindingFlags.Instance)
                                     .Count();

                AttributesHidden += type.GetFields(BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance)
                                        .Count();
            }

            return AttributesAll != 0 ? (decimal)AttributesHidden / AttributesAll : 0;
        }
        private static decimal GetAHF(Type type)
        {
            int AttributesAll = 0;
            int AttributesHidden = 0;

            AttributesAll += type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Static |
                                                BindingFlags.Public | BindingFlags.NonPublic |
                                                BindingFlags.Static | BindingFlags.Instance)
                                     .Count();

            AttributesHidden += type.GetFields(BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance)
                                    .Count();

            return AttributesAll != 0 ? (decimal)AttributesHidden / AttributesAll : 0;
        }


        private static decimal GetAIF(Assembly assembly)
        {
            int AttributesAll = 0;
            int AttributesInherited = 0;

            foreach (Type type in assembly.GetTypes())
            {
                AttributesAll += type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Static |
                                                BindingFlags.Public | BindingFlags.NonPublic |
                                                BindingFlags.Static | BindingFlags.Instance)
                    .Count();

                AttributesInherited += type.GetFields(BindingFlags.Public | BindingFlags.NonPublic |
                                                      BindingFlags.Instance | BindingFlags.Static |
                                                      BindingFlags.FlattenHierarchy)
                    .Where(f => type.GetField(f.Name, BindingFlags.DeclaredOnly |
                                                      BindingFlags.Public | BindingFlags.NonPublic |
                                                      BindingFlags.Instance | BindingFlags.Static) == null)
                    .Count();
            }

            
            return AttributesAll != 0 ? (decimal)AttributesInherited / AttributesAll : 0;
        }

        private static decimal GetAIF(Type type)
        {
            int AttributesAll = 0;
            int AttributesInherited = 0;

            AttributesAll = type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Static |
                                                BindingFlags.Public | BindingFlags.NonPublic |
                                                BindingFlags.Static | BindingFlags.Instance)
                    .Count();

            AttributesInherited = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic |
                                                  BindingFlags.Instance | BindingFlags.Static |
                                                  BindingFlags.FlattenHierarchy)
                .Where(f => type.GetField(f.Name, BindingFlags.DeclaredOnly |
                                                  BindingFlags.Public | BindingFlags.NonPublic |
                                                  BindingFlags.Instance | BindingFlags.Static) == null)
                .Count();

            return AttributesAll != 0 ? (decimal)AttributesInherited / AttributesAll : 0;
        }

        private static decimal GetPOF(Assembly assembly)
        {
            var MethodsInheritedOverriden = 0;
            var MethodsNew = 0;
            var DC = 0;
            var assemblyTypes = assembly.GetTypes();

            foreach (Type type in assemblyTypes)
            {
                MethodsInheritedOverriden += type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic |
                                                             BindingFlags.Instance | BindingFlags.Static |
                                                             BindingFlags.FlattenHierarchy)
                    .Where(m => m.GetBaseDefinition() != m && m.DeclaringType == type)
                    .Count();
                MethodsNew += type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic |
                                              BindingFlags.Instance | BindingFlags.Static |
                                              BindingFlags.DeclaredOnly)
                    .Where(m => m.GetBaseDefinition() == m)
                    .Count();

                DC += assemblyTypes.Count(t => type.Equals(t.BaseType));
            }

            return MethodsNew == 0 || DC == 0 ? 0 : (decimal)MethodsInheritedOverriden / (MethodsNew * DC);
        }

        private static decimal GetPOF(Type type)
        {
            var MethodsInheritedOverriden = 0;
            var MethodsNew = 0;
            var DC = 0;
            var nestedTypes = type.GetNestedTypes();

            foreach (Type ntype in nestedTypes)
            {
                MethodsInheritedOverriden += ntype.GetMethods(BindingFlags.Public | BindingFlags.NonPublic |
                                                             BindingFlags.Instance | BindingFlags.Static |
                                                             BindingFlags.FlattenHierarchy)
                    .Where(m => m.GetBaseDefinition() != m && m.DeclaringType == ntype)
                    .Count();
                MethodsNew += ntype.GetMethods(BindingFlags.Public | BindingFlags.NonPublic |
                                              BindingFlags.Instance | BindingFlags.Static |
                                              BindingFlags.DeclaredOnly)
                    .Where(m => m.GetBaseDefinition() == m)
                    .Count();

                DC += nestedTypes.Count(t => type.Equals(t.BaseType));
            }

            return MethodsNew == 0 || DC == 0 ? 0 : (decimal)MethodsInheritedOverriden / (MethodsNew * DC);
        }

        public static Metrics GetAssemblyMetrics(Assembly assembly)
        {
            return new Metrics
            {
                DIT = GetDIT(assembly),
                NOC = GetNOC(assembly),
                MHF = GetMHF(assembly),
                AHF = GetAHF(assembly),
                MIF = GetMIF(assembly),
                AIF = GetAIF(assembly),
                POF = GetPOF(assembly)
            };
        }

        public static List<ClassMetrics> GetClassMetrics(Type[] classes)
        {
            List<ClassMetrics> metrics = new List<ClassMetrics> {};

            foreach (var c in classes)
            {
                Metrics metric = new Metrics{
                    DIT = GetDIT(c),
                    NOC = GetNOC(c),
                    MHF = GetMHF(c),
                    AHF = GetAHF(c),
                    MIF = GetMIF(c),
                    AIF = GetAIF(c),
                    POF = GetPOF(c)
                };

                metrics.Add(new ClassMetrics {
                    _Metrics = metric,
                    ClassName = c.Name
                });
            }


            return metrics;
        }

        public static String MetricsToString(Metrics metrics)
        {
            String resultStr = String.Format("DIT: {0}\nNOC: {1}\nMIF: {2:0.0000}\nMHF: {3:0.0000}\n" +
                                             "AHF: {4:0.0000}\nAIF: {5:0.0000}\nPOF: {6:0.0000}\n", 
                                             metrics.DIT,
                                             metrics.NOC, metrics.MIF, 
                                             metrics.MHF, metrics.AHF, 
                                             metrics.AIF, metrics.POF);

            return resultStr;
        }

        public static String MetricsToString(List <ClassMetrics> metrics)
        {
            String resultStr = "";
            for (int i = 0; i < metrics.Count; i++)
            {
                resultStr += String.Format(  "ClassName: {0}, \nDIT: {1}\nNOC: {2}\nMIF: {3:0.0000}\nMHF: {4:0.0000}\n" +
                                             "AHF: {5:0.0000}\nAIF: {6:0.0000}\nPOF: {7:0.0000}\n\n",
                                             metrics.ElementAt(i).ClassName, metrics.ElementAt(i)._Metrics.DIT,
                                             metrics.ElementAt(i)._Metrics.NOC, metrics.ElementAt(i)._Metrics.MIF,
                                             metrics.ElementAt(i)._Metrics.MHF, metrics.ElementAt(i)._Metrics.AHF,
                                             metrics.ElementAt(i)._Metrics.AIF, metrics.ElementAt(i)._Metrics.POF);
            }
            
            return resultStr;
        }
    }
}
