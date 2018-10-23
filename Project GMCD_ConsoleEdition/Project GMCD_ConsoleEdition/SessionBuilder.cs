using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Project_GMCD_ConsoleEdition
{
    class SessionBuilder
    {
        /// <summary>
        /// Define se a requisição será do tipo Get[false] ou POST[true]
        /// </summary>
        private bool TypePost;
        
        /// <summary>
        /// Armazena o HTML
        /// </summary>
        private HtmlDocument Html;

        /// <summary>
        /// Armazena os Cookies
        /// </summary>
        public CookieCollection Cookies { get; set; }

        /// <summary>
        /// Permite o uso dos metodos da classe FormParseCollection
        /// </summary>
        public FormParseCollection FormElements { get; set; }

        /// <summary>
        /// Esse metodo verifica se o metodo de acesso será feito por GET
        /// </summary>
        /// <param name="url"> Endereço do site, ela receberá posteriormente o GetUrl de cada tribunal </param>
        /// <returns> Retorna um acesso GET </returns>
        public string Get(string url)
        {
            TypePost = false;
            CreateWebRequestObject().Load(url);
            return Html.DocumentNode.InnerHtml;
        }

        /// <summary>
        /// Esse metodo verifica se o metodo de acesso será feito por POST
        /// </summary>
        /// <param name="url"> Endereço do site, ela receberá posteriormente o GetUrl de cada tribunal </param>
        /// <returns> Retorna um acesso POST </returns>
        public string Post(string url)
        {
            TypePost = true;
            CreateWebRequestObject().Load(url, "POST");
            return Html.DocumentNode.InnerHtml;
        }

        /// <summary>
        /// Adiciona um cookie a request
        /// </summary>
        /// <param name="request"> Essa variavel irá armazenar todo o conteudo de um post de uma página </param>
        private void AddCookiesTo(HttpWebRequest request)
        {
            if (Cookies != null && Cookies.Count > 0)
            {
                request.CookieContainer.Add(Cookies);
            }
        }

        /// <summary>
        /// Adiciona os dados a serem passados no POST
        /// </summary>
        /// <param name="request"> Essa variavel irá armazenar todo o conteudo de um post de uma página </param>
        private void AddPostDataTo(HttpWebRequest request)
        {
            string payload = FormElements.AssemblePostPayload();
            byte[] buff = Encoding.UTF8.GetBytes(payload.ToCharArray());

            request.ContentLength = buff.Length;
            request.ContentType = "application/x-www-form-urlencoded";

            Stream reqStream = request.GetRequestStream();

            reqStream.Write(buff, 0, buff.Length);
        }

        /// <summary>
        /// Faz a configuração do PreRequest
        /// Se a request for do tipo POST, a mesma passará os dados para que a request com o POST possa ser feita
        /// </summary>
        /// <param name="request"> Essa variavel irá armazenar todo o conteudo de um post de uma página </param>
        /// <returns> </returns>
        protected bool PreRequest(HttpWebRequest request)
        {
            // Adiciona um cookie de uma request
            AddCookiesTo(request);

            if (TypePost)
            {
                AddPostDataTo(request);
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        private void SaveCookiesFrom(HttpWebResponse response)
        {
            if (response.Cookies.Count > 0)
            {
                if (Cookies == null) Cookies = new CookieCollection();
                Cookies.Add(response.Cookies);
            }
        }

        /// <summary>
        /// Salva o resultado da PreRequest
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        protected void AfterResponse(HttpWebRequest request, HttpWebResponse response)
        {
            SaveCookiesFrom(response);
        }

        /// <summary>
        /// Transfere o HTML com o POST
        /// </summary>
        /// <param name="document"></param>
        private void SaveHtmlDocument(HtmlDocument document)
        {
            Html = document;
            FormElements = new FormParseCollection(Html);
        }

        /// <summary>
        /// Salva o HTML com o cookie já injetado
        /// </summary>
        /// <param name="document"></param>
        protected void PreHandleDocument(HtmlDocument document)
        {
            SaveHtmlDocument(document);
        }

        /// <summary>
        /// Cria uma conexão com a página que deseja acessar
        /// </summary>
        /// <returns> Retorna a requisição WEB </returns>
        private HtmlWeb CreateWebRequestObject()
        {
            HtmlWeb web = new HtmlWeb();
            web.UseCookies = true;                                                          // Serve para utilizar cookie's
            web.PreRequest = new HtmlWeb.PreRequestHandler(PreRequest);                     // Faz o primeiro acesso para a captura do Html
            web.PostResponse = new HtmlWeb.PostResponseHandler(AfterResponse);              // Envia o formulário POST para o site
            web.PreHandleDocument = new HtmlWeb.PreHandleDocumentHandler(PreHandleDocument);// Recebe a página após o POST
            return web;
        }
    }
}
