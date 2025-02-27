﻿using Core.Exceptions;
using Entities.Common;
using System;
using System.Reflection;

namespace Core.Utils
{
    public static class EntityHelper
    {
        /// <summary>
        /// Gets primary key type of given entity type
        /// </summary>
        public static Type GetPrimaryKeyType(Type entityType)
        {
            foreach (var interfaceType in entityType.GetTypeInfo().GetInterfaces())
            {
                if (interfaceType.GetTypeInfo().IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IEntity<>))
                {
                    return interfaceType.GenericTypeArguments[0];
                }
            }

            throw new STSGenericException("Can not find primary key type of given entity type: " + entityType + ". Be sure that this entity type implements IEntity<TPrimaryKey> interface");
        }
    }
}