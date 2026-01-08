using HtmlAgilityPack;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace SanitizationAPI
{
    public static class PeopleNormalizer
    {
        public static People Normalize(People people)
        {
            return new People
            {
                Name = NormalizeName(people.Name),
                Email = NormalizeEmail(people.Email),
                Phone = NormalizedPhone(people.Phone),
                Description = NormalizedDescription(people.Description),
                Content = NormalizeContent(people.Content)
            };
        }

        public static string NormalizeName(string? name)
        {
            if(string.IsNullOrWhiteSpace(name))
            {
                return string.Empty;
            }

            name = name.Trim();

            name = Regex.Replace(name, @"\s+", " ");

            return name;
        }

        public static string NormalizeEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return string.Empty;
            }

            email = email.Trim().ToLowerInvariant();
            return email;
        }

        public static string NormalizedPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
            {
                return string.Empty;
            }

            phone = Regex.Replace(phone, @"\D", "");

            return phone;
        }
        
        public static string NormalizedDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                return string.Empty;
            }

            // Al poner ^ dentro de los corchetes, se indica una negación, es decir, se seleccionan todos
            // los caracteres que NO están en el conjunto especificado.

            description = Regex.Replace(description, @"[^a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s]", "");
            description = Regex.Replace(description, @"\s+", " ");
            description = description.Trim();

            return description;
        }

        public static string NormalizeContent(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                return string.Empty;
            }

            var doc = new HtmlDocument();
            doc.LoadHtml(content);

            var scripts = doc.DocumentNode.SelectNodes("//script");

            if (scripts != null)
            {
                foreach (var script in scripts)
                {
                    script.Remove();
                }
            }

            return doc.DocumentNode.InnerHtml.Trim();
        }
    }
}
