using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RedesignedPotato.Interfaces
{
    public interface IAuthorizer
    {
        /// <summary>
        /// Register a type and user "Id" as the key.
        /// </summary>
        /// <typeparam permissionSetName="T"></typeparam>
        /// <typeparam name="T"></typeparam>
        void Register<T>(string permissionSetName = "default");

        /// <summary>
        /// Register a type and specify the key with an expression
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <param name="permissionSetName"></param>
        void Register<T>(Expression<Func<T, object>> expression, string permissionSetName = "default");

        /// <summary>
        /// Permit user with token to access the following objects
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="token"></param>
        /// <param name="objects"></param>
        void Permit<T>(string token, IEnumerable<T> objects);

        /// <summary>
        /// Permit user with token to access the following objects in the specified permission set
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="token"></param>
        /// <param name="objects"></param>
        /// <param name="permissionSetName"></param>
        void Permit<T>(string token, IEnumerable<T> objects, string permissionSetName);

        /// <summary>
        /// Check if user has access to all objects in the default permission set
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="token"></param>
        /// <param name="objects"></param>
        /// <returns></returns>
        bool HasPermission<T>(string token, IEnumerable<T> objects);

        /// <summary>
        /// Check if user has access to all objects in the specified permission set
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="token"></param>
        /// <param name="objects"></param>
        /// <param name="permissionSetName"></param>
        /// <returns></returns>
        bool HasPermission<T>(string token, IEnumerable<T> objects, string permissionSetName);
    }
}
