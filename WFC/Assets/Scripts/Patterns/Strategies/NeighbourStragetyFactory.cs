using System;
using System.Collections.Generic;
using System.Reflection;

namespace WaveFunctionCollapse
{
    public class NeighbourStragetyFactory 
    {
        Dictionary<string, Type> strategies;
        public NeighbourStragetyFactory()
        {
            LoadTypesIFindNeighbourStrategy();
        }

        private void LoadTypesIFindNeighbourStrategy()
        {
            #region Boilerplate code
            //  boilerplate code Def: Code, der wiederholt in mehreren Stellen
            //  verwendet wird mit wenig bis gar keiner variation
            strategies = new Dictionary<string, Type>();
            Type[] typesInAssembly = Assembly.GetExecutingAssembly().GetTypes();
            foreach (var type in typesInAssembly)
            {
                if (type.GetInterface(typeof(IFindNeighbourStrategy).ToString()) != null)
                {
                    strategies.Add(type.Name.ToLower(), type);
                }
            }
            #endregion
        }

        internal IFindNeighbourStrategy CreateInstance(string nameOfStrategy)
        {
            Type t = GetTypeToCreate(nameOfStrategy);
            if (t == null)
            {
                t = GetTypeToCreate("more");
            }
            return Activator.CreateInstance(t) as IFindNeighbourStrategy;
        }

        private Type GetTypeToCreate(string nameOfStrategy)
        {
            foreach (var possibleStrategy in strategies)
            {
                if (possibleStrategy.Key.Contains(nameOfStrategy))
                {
                    return possibleStrategy.Value;
                }
            }
            return null;
        }
    }
}