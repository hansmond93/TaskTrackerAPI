﻿using Microsoft.EntityFrameworkCore;
using Core.Reflection;
using Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Core.DataAccess.EfCore
{
    public class EfCoreDbContextEntityFinder
    {
        public static IEnumerable<EntityTypeInfo> GetEntityTypeInfos(Type dbContextType)
        {
            return
                from property in dbContextType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                where
                    ReflectionHelper.IsAssignableToGenericType(property.PropertyType, typeof(DbSet<>)) &&
                    ReflectionHelper.IsAssignableToGenericType(property.PropertyType.GenericTypeArguments[0], typeof(IEntity<>))
                select new EntityTypeInfo(property.PropertyType.GenericTypeArguments[0], property.DeclaringType);
        }
    }
}