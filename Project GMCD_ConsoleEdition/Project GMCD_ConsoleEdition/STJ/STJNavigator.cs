using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace Project_GMCD_ConsoleEdition
{
    class STJNavigator
    {
        public void ConnectionPostData(string data_inicial, string data_final)
        {
            var parsedDI = DateTime.Parse(data_inicial);
            var parsedDF = DateTime.Parse(data_final);

            // Inicia uma nova seção
            SessionBuilder newSession = new SessionBuilder();

            newSession.GetXpathForm(GetForm());
            newSession.Get(GetUrl());
            
            // Campos a serem passados na request
            newSession.FormElements["acao"] = "pesquisar";
            newSession.FormElements["novaConsulta"] = "true";
            newSession.FormElements["i"] = "1";
            newSession.FormElements["data"] = "@DTPB >= " + parsedDI.ToString("yyyyMMdd") + " e @DTPB <= " + parsedDF.ToString("yyyyMMdd");
            newSession.FormElements["livre"] = string.Empty;
            newSession.FormElements["ref"] = string.Empty;
            newSession.FormElements["opAjuda"] = "SIM";
            newSession.FormElements["tipo_visualizacao"] = "RESUMO";
            newSession.FormElements["thesaurus"] = "null";
            newSession.FormElements["p"] = "true";
            newSession.FormElements["operador"] = "e";
            newSession.FormElements["processo"] = string.Empty;
            newSession.FormElements["livreMinistro"] = string.Empty;
            newSession.FormElements["relator"] = string.Empty;
            newSession.FormElements["data_inicial"] = parsedDI.ToString("dd/MM/yyyy");
            newSession.FormElements["data_final"] = parsedDF.ToString("dd/MM/yyyy");
            newSession.FormElements["tipo_data"] = "DTPB";
            newSession.FormElements["livreOrgaoJulgador"] = string.Empty;
            newSession.FormElements["orgao"] = string.Empty;
            newSession.FormElements["ementa"] = string.Empty;
            newSession.FormElements["siglajud"] = string.Empty;
            newSession.FormElements["numero_leg"] = string.Empty;
            newSession.FormElements["tipo1"] = string.Empty;
            newSession.FormElements["numero_art1"] = string.Empty;
            newSession.FormElements["tipo2"] = string.Empty;
            newSession.FormElements["numero_art2"] = string.Empty;
            newSession.FormElements["tipo3"] = string.Empty;
            newSession.FormElements["numero_art3"] = string.Empty;
            newSession.FormElements["nota"] = string.Empty;
            newSession.FormElements["b"] = "ACOR";

            var response = newSession.Post(GetUrl());
        }

        public string GetForm()
        {
            var xpathForm = "//form[@id='frmConsulta']";
            return xpathForm;
        }

        public string GetUrl()
        {
            var url = "http://www.stj.jus.br/SCON/";
            return url;
        }
    }
}
