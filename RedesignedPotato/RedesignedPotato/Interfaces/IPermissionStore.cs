using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedesignedPotato.Interfaces
{
    public interface IPermissionStore
    {
        /// <summary>
        /// Register a store for the type 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="permissionSetName"></param>
        void Register<T>(string permissionSetName);

        /// <summary>
        /// Add a list of ids to the store
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="token"></param>
        /// <param name="ids"></param>
        /// <param name="permissionSetName"></param>
        void Permit<T>(string token, IEnumerable<string> ids, string permissionSetName);

        /// <summary>
        /// Checks if user has access to the permission set
        /// </summary>
        /// <param name="token"></param>
        /// <param name="ids"></param>
        /// <param name="permissionSetName"></param>
        /// <returns></returns>
        bool HasPermission<T>(string token, IEnumerable<string> ids, string permissionSetName);
    }
}
