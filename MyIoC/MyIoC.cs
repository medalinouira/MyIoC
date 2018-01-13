/// http://www.sweetmit.com
/// https://github.com/medalinouira
/// Copyright © Mohamed Ali NOUIRA. All rights reserved.
using System;
using System.Linq;
using System.Collections.Generic;

namespace MyIoC
{
    /// <summary>
    /// The MyIoC class.
    /// </summary>
    public class MyIoC
    {
        #region Singleton
        /// <summary>
        /// The private class instance.
        /// </summary>
        private static MyIoC instance;

        /// <summary>
        /// The static and public instance.
        /// </summary>
        public static MyIoC Instance
        {
            get
            {
                if (instance == null)
                    instance = new MyIoC();
                return instance;
            }
        }
        #endregion

        #region Fields
        /// <summary>
        /// Create a dictionary to get all the types mapped correctly 
        /// </summary>           
        Dictionary<Type, Type> dependencyMap = new Dictionary<Type, Type>();
        #endregion

        #region Methods

        /// <summary>
        /// Provides a way to get an instance of a given type. If no instance had been
        /// instantiated before, a new instance will be created. If an instance had already<para>
        /// been created, that same instance will be returned.  If the class has nottypeToResolve is the class to resolve.
        /// been registered before, this method returns null!
        /// <typeparamref name="T">
        /// Is the given class.     
        /// </typeparamref>    
        /// </summary>         
        public T Resolve<T>()
        {
            return (T)Resolve(typeof(T));
        }

        /// <summary> 
        /// Create an instance of the typeToResolve type.
        /// <param name="typeToResolve">
        /// Type of class to instantiate.
        /// </param>
        /// <returns>    
        /// An instance of the given type.
        /// </returns> 
        /// </summary>    
        private object Resolve(Type typeToResolve)
        {
            Type resolvedType = null;
            try
            {
                resolvedType = dependencyMap[typeToResolve];
            }
            catch
            {
                throw new Exception(String.Format("Can't resolve type of {0}", typeToResolve));
            }

            var firstConstructor = resolvedType.GetConstructors().First();
            var constructorParameters = firstConstructor.GetParameters();
            if (constructorParameters.Count() == 0)
                return Activator.CreateInstance(resolvedType);

            IList<Object> parameters = new List<Object>();
            foreach (var parameterToResolve in constructorParameters)
            {
                parameters.Add(Resolve(parameterToResolve.ParameterType));
            }

            return firstConstructor.Invoke(parameters.ToArray());

        }

        /// <summary> 
        /// Registers a given type for the given interface.
        /// <typeparamref name="TTo"> 
        /// Is the given interface. 
        /// </typeparamref>
        /// <typeparamref name="TFrom">
        /// Is the given type to register.
        /// </typeparamref>
        /// </summary> 
        public void Register<TTo, TFrom>()
        {
            dependencyMap.Add(typeof(TTo), typeof(TFrom));
        }

        /// <summary> 
        /// Gets a value indicating whether a given type T is already registered.
        /// <typeparamref name="T">
        /// The type that the method checks for.
        /// </typeparamref>
        /// <returns>
        /// True if the type is registered, false otherwise.
        /// </returns>   
        /// </summary> 
        public bool IsRegistered<T>()
        {
            return dependencyMap.ContainsKey(typeof(T));
        }

        /// <summary> 
        /// Unregisters a class from the cache and removes all the previously created
        /// <typeparamref name="T">
        /// is the dependency to unregister from the cache.
        /// </typeparamref>
        /// </summary> 
        public void Unregister<T>()
        {
            dependencyMap.Remove(typeof(T));
        }

        #endregion
    }
}
