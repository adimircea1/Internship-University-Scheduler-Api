using System.ComponentModel.DataAnnotations;
using System.Reflection;
using OnEntitySharedLogic.Utils;

namespace OnEntitySharedLogic.Extensions;

public static class Validate
{
    //I used the dictionary for caching purposes
    //This extension has been created to validate model properties by comparing the values to a minvalue and a maxvalue

    private static readonly Dictionary<Type, PropertyInfo[]> ValidateAttributePropertiesCache = new();

    public static void ValidateListOfEntities<TEntity>(this List<TEntity> entities)
        where TEntity : IEntity
    {
        ValidateListElementsProperties(entities);
    }

    public static void ValidateEntity<TEntity>(this TEntity entity)
        where TEntity : IEntity
    {
        ValidateElementProperties(entity);
    }


    private static void ValidateElementProperties<TEntity>(this TEntity entity)
    {
        var entityType = entity!.GetType();

        if (!ValidateAttributePropertiesCache.TryGetValue(entityType, out var propertiesWithValidateAttribute))
        {
            propertiesWithValidateAttribute = entityType
                .GetProperties()
                .Where(property => Attribute
                    .IsDefined(property, typeof(ValidateAttribute)))
                .ToArray();

            ValidateAttributePropertiesCache[entityType] = propertiesWithValidateAttribute;
        }

        foreach (var propertyInfo in propertiesWithValidateAttribute)
        {
            var validateAttribute = propertyInfo.GetCustomAttribute(typeof(ValidateAttribute)) as ValidateAttribute;

            var propertyValue = propertyInfo.GetValue(entity);

            switch (propertyValue)
            {
                case "" 
                    or null:
                    continue;
            }
            
            if (validateAttribute != null && validateAttribute.Validate(propertyValue) == false)
            {
                throw new ValidationException(
                    $"Invalid property {propertyInfo.Name} - validation failed for the current entity!");
            }
        }
    }

    private static void ValidateListElementsProperties<TEntity>(this List<TEntity> entities) where TEntity : IEntity
    {
        foreach (var entity in entities)
        {
            var entityType = entity.GetType();

            if (!ValidateAttributePropertiesCache.TryGetValue(entityType,
                    out var propertiesWithValidateAttribute))
            {
                propertiesWithValidateAttribute = entityType
                    .GetProperties()
                    .Where(property => Attribute
                        .IsDefined(property, typeof(ValidateAttribute)))
                    .ToArray();

                ValidateAttributePropertiesCache[entityType] = propertiesWithValidateAttribute;
            }


            foreach (var propertyInfo in propertiesWithValidateAttribute)
            {
                var validateAttribute =
                    propertyInfo.GetCustomAttribute(typeof(ValidateAttribute)) as ValidateAttribute;

                var propertyValue = propertyInfo.GetValue(entity);

                switch (propertyValue)
                {
                    case "" 
                        or null:
                        continue;
                }

                if (validateAttribute != null && validateAttribute.Validate(propertyValue) == false)
                {
                    throw new ValidationException(
                        $"Invalid property {propertyInfo.Name} - validation failed for the current entity - cannot use the list of entities!");
                }
            }
        }
    }
}