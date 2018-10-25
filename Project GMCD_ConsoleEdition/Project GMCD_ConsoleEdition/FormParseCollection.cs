using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Project_GMCD_ConsoleEdition
{
    class FormParseCollection : Dictionary<string, string>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        private void AddInputElement(HtmlNode element)
        {
            string name = element.GetAttributeValue("name", "");
            string value = element.GetAttributeValue("value", "");
            string type = element.GetAttributeValue("type", "");

            if (string.IsNullOrEmpty(name))
            {
                return;
            };

            switch (type.ToLower())
            {
                case "checkbox":
                case "radio":
                    if (!ContainsKey(name))
                    {
                        Add(name, "");
                    }

                    string isChecked = element.GetAttributeValue("checked", "unchecked");

                    if (!isChecked.Equals("unchecked"))
                    { 
                        this[name] = value;  
                    }
                    break;
                default:
                    Add(name, value);
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        private void AddMenuElement(HtmlNode element)
        {
            string name = element.GetAttributeValue("name", "");
            var options = element.Descendants("option");

            if (string.IsNullOrEmpty(name)) return;

            // choose the first option as default
            var firstOp = options.First();
            string defaultValue = firstOp.GetAttributeValue("value", firstOp.NextSibling.InnerText);

            Add(name, defaultValue);

            // check if any option is selected
            foreach (var option in options)
            {
                string selected = option.GetAttributeValue("selected", "notSelected");
                if (!selected.Equals("notSelected"))
                {
                    string selectedValue = option.GetAttributeValue("value", option.NextSibling.InnerText);
                    this[name] = selectedValue;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        private void AddTextareaElement(HtmlNode element)
        {
            string name = element.GetAttributeValue("name", "");
            if (string.IsNullOrEmpty(name)) return;
            Add(name, element.InnerText);
        }

        /// <summary>
        /// Construtor da classe. Esse constutor transfere os valores do POST para o campo especifico
        /// </summary>
        /// <param name="htmlDoc"></param>
        public FormParseCollection(HtmlDocument htmlDoc, string xpathform)
        {
            // Passa o Xpath do form
            var form = htmlDoc.DocumentNode.SelectSingleNode(xpathform);

            // Para Input
            var inputs = form.SelectNodes("./input");
            if (inputs != null)
            {
                foreach (var element in inputs)
                {
                    AddInputElement(element);
                }
            }

            // Para ComboBox
            var menus = form.SelectNodes("select");
            if (menus != null)
            {
                foreach (var element in menus)
                {
                    AddMenuElement(element);
                }
            }

            // Para TextArea
            var textareas = form.SelectNodes("textarea");
            if (textareas != null)
            {
                foreach (var element in textareas)
                {
                    AddTextareaElement(element);
                }
            }
        }

        /// <summary>
        /// Formata os elementos do POST para o formato necessário
        /// </summary>
        /// <returns> Retorna a StringBuilder no formato desejado </returns>
        public string AssemblePostPayload()
        {
            // String Builder é uma string que poderá ser modificada de acordo com a rotina
            StringBuilder sb = new StringBuilder();

            foreach (var element in this)
            {
                var value = HttpUtility.UrlEncode(element.Value);
                sb.Append("&" + element.Key + "=" + value);         // Modifica um objeto StringBuilder com os caracteres desejados
            }

            return sb.ToString().Substring(1);
        }
    }
}
