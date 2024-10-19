using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OnEntitySharedLogic.Extensions;

public static class UpdateByReflection
{
    public static void UpdateEntityByReflection<TEntity>(this TEntity outdatedEntity, string? updatedEntityJson) where TEntity : class
    {
        if (updatedEntityJson is null)
        {
            return;
        }
        
        var updatedEntityJsonObject = JObject.Parse(updatedEntityJson);
        var propertiesToChange = updatedEntityJsonObject.Properties();
        
        var updatedEntity = JsonConvert.DeserializeObject<TEntity>(updatedEntityJson);
        
        foreach (var property in propertiesToChange)
        {
            //Note that all my properties are named using CamelCase
            //The JObject contain properties with this kind of name: studyYear
            var targetProperty = typeof(TEntity).GetProperties().FirstOrDefault(propertyElement => 
                    string.Equals(propertyElement.Name, property.Name, StringComparison.InvariantCultureIgnoreCase));
            
            var updatedEntityPropertyValue = targetProperty!.GetValue(updatedEntity);
            
            targetProperty.SetValue(outdatedEntity, updatedEntityPropertyValue);
        }
    }
}