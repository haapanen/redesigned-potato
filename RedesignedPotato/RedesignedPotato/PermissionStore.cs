using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RedesignedPotato.Interfaces;

namespace RedesignedPotato
{
    public class AlreadyRegisteredException : Exception
    {
        public AlreadyRegisteredException()
        {
        }

        public AlreadyRegisteredException(string message) : base(message)
        {
        }
    }

    public class NotRegisteredException : Exception
    {
        public NotRegisteredException()
        {
        }

        public NotRegisteredException(string message) : base(message)
        {
        }
    }

    public class PermissionStore : IPermissionStore
    {
        private readonly Dictionary<string, Dictionary<Type, Dictionary<string, List<string>>>> _permissions = new Dictionary<string, Dictionary<Type, Dictionary<string, List<string>>>>();

        public void Register<T>(string permissionSetName)
        {
            var type = typeof(T);

            if (_permissions.ContainsKey(type))
            {
                if (_permissions[type].ContainsKey(permissionSetName))
                {
                    throw new AlreadyRegisteredException($"{type.FullName} already has a registered permissions list for name `{permissionSetName}`");
                }

                _permissions[type][permissionSetName] = new List<string>();
            }

            _permissions[type] = new Dictionary<string, List<string>> {[permissionSetName] = new List<string>()};
        }

        public void Permit<T>(string token, IEnumerable<string> ids, string permissionSetName)
        {
            throw new NotImplementedException();
        }

        public bool HasPermission<T>(string token, IEnumerable<string> ids, string permissionSetName)
        {
            throw new NotImplementedException();
        }

        public void Permit<T>(IEnumerable<string> ids, string permissionSetName)
        {
            var type = typeof(T);

            if (!_permissions.ContainsKey(type) || !_permissions[type].ContainsKey(permissionSetName))
            {
                throw new NotRegisteredException($"No permissions list has been registered for type {type.FullName} with name `${permissionSetName}`");
            }

            _permissions[type][permissionSetName].AddRange(ids);
        }

        public bool HasPermission<T>(IEnumerable<string> ids, string permissionSetName)
        {
            var type = typeof(T);

            if (!_permissions.ContainsKey(type) || !_permissions[type].ContainsKey(permissionSetName))
            {
                throw new NotRegisteredException($"No permissions list has been registered for type {type.FullName} with name `${permissionSetName}`");
            }

            return !ids.Except(_permissions[type][permissionSetName]).Any();
        }
    }
}
