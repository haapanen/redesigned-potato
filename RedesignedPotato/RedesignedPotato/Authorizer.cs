using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using RedesignedPotato.Interfaces;

namespace RedesignedPotato
{
    public class RegisterException : Exception
    {
        public RegisterException()
        {
        }

        public RegisterException(string message) : base(message)
        {
        }
    }

    public class Authorizer : IAuthorizer
    {
        private readonly IPermissionStore _permissionStore;
        private readonly Dictionary<Type, PropertyInfo> _typeIdFields = new Dictionary<Type, PropertyInfo>();
        private const string DefaultPermissionSetName = "default";

        public Authorizer(IPermissionStore permissionStore)
        {
            _permissionStore = permissionStore;
        }

        public void Register<T>(string permissionSetName = DefaultPermissionSetName)
        {
            var type = typeof(T);
            const string defaultId = "Id";

            if (!HasMember(type, defaultId))
            {
                throw new RegisterException($"{type.FullName} has no member `Id`");
            }

            AddKeyForType(type, defaultId);

            _permissionStore.Register<T>(permissionSetName);
        }

        public void Register<T>(Expression<Func<T, object>> expression, string permissionSetName = DefaultPermissionSetName)
        {
            var memberExpression = expression.Body as MemberExpression;
            var name = memberExpression?.Member.Name;
            if (name == null)
            {
                throw new InvalidOperationException("Name is null for member expression");
            }

            AddKeyForType(typeof(T), name);

            _permissionStore.Register<T>(permissionSetName);
        }

        public void Permit<T>(string token, IEnumerable<T> objects)
        {
            var type = typeof(T);
            var propertyInfo = GetPropertyInfoForType(type);

            _permissionStore.Permit<T>(token, objects.Select(x => propertyInfo.GetValue(x, null) as string), DefaultPermissionSetName);
        }

        public void Permit<T>(string token, IEnumerable<T> objects, string permissionSetName)
        {
            if (string.IsNullOrEmpty(permissionSetName)) permissionSetName = DefaultPermissionSetName;

            var type = typeof(T);
            var propertyInfo = GetPropertyInfoForType(type);

            _permissionStore.Permit<T>(token, objects.Select(x => propertyInfo.GetValue(x, null) as string), permissionSetName);
        }

        public bool HasPermission<T>(string token, IEnumerable<T> objects)
        {
            var type = typeof(T);
            var propertyInfo = GetPropertyInfoForType(type);

            return _permissionStore.HasPermission<T>(token,
                objects.Select(x => propertyInfo.GetValue(x, null) as string), DefaultPermissionSetName);
        }

        public bool HasPermission<T>(string token, IEnumerable<T> objects, string permissionSetName)
        {
            var type = typeof(T);
            var propertyInfo = GetPropertyInfoForType(type);

            return _permissionStore.HasPermission<T>(token,
                objects.Select(x => propertyInfo.GetValue(x, null) as string), permissionSetName);
        }

        private static bool HasMember(Type type, string memberName)
        {
            var typeMembers = type.GetMembers();

            if (typeMembers.All(x => x.Name != memberName))
            {
                return false;
            }
            return true;
        }

        private void AddKeyForType(Type type, string key)
        {
            if (_typeIdFields.ContainsKey(type) && _typeIdFields[type].Name != key)
            {
                throw new RegisterException(
                    $"Trying to register type `{type.FullName}` with a different key: Original: {_typeIdFields[type]} New: {key}");
            }
            else
            {
                _typeIdFields[type] = type.GetProperty(key);
            }
        }

        private PropertyInfo GetPropertyInfoForType(Type type)
        {
            if (!_typeIdFields.ContainsKey(type))
            {
                throw new KeyNotFoundException($"Type `{type.FullName}` has no key specified");
            }
            return _typeIdFields[type];
        }
    }
}
