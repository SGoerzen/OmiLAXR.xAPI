using Newtonsoft.Json.Linq;
using xAPI.Registry;
using tc = TinCan;

namespace OmiLAXR.xAPI.Extensions
{
    public static class TinCan_Ext
    {
        public static void AssignTo(this xAPI_Extensions extensions, JObject jObject, string uri)
        {
            // Skip if there are no extensions
            if (jObject == null)
                return;
            foreach (var (extension, value) in extensions)
            {
                var id = extension.CreateValidId(uri);
                jObject.Add(id, value == null ? "null" : value.ToString());
            }
        }

        public static tc.Extensions ToTinCanExtensions(this xAPI_Extensions extensions, System.Uri uri)
            => ToTinCanExtensions(extensions, uri.ToString());
        public static tc.Extensions ToTinCanExtensions(this xAPI_Extensions extensions, string uri)
        {
            var jObject = new JObject();
            extensions.AssignTo(jObject, uri);

            return new tc.Extensions(jObject);
        }
        public static tc.ActivityDefinition ToTinCanActivityDefinition(this xAPI_Definition definition)
        {
            return new tc.ActivityDefinition
            {
                // translate xAPI name definitions to LanguageMap
                name = new tc.LanguageMap(definition.Names),
                description = new tc.LanguageMap(definition.Descriptions)
            };
        }
    }
}
