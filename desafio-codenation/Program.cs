using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace desafiocodenation
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            string token = "d37bbba08358d3bd5720de7dddc02a11531ab8e5";
            string url = $"https://api.codenation.dev/v1/challenge/dev-ps/generate-data?token={token}";
            string letras = "A B C D E F G H I J K L M N O P Q R S T U V W X Y Z";
            string[] letra = letras.Split(' ');

            try {
                JObject json = GetFirstJson(url);
                int numeroCasas = 26 - (int)json.GetValue("numero_casas");
                string textoCifrado = json.GetValue("cifrado").ToString();
                char[] textoChar = textoCifrado.ToCharArray();
                Console.WriteLine("---------------- Inicio ---------------");
                string textDecifrado = ToDecript(textoChar, letra, numeroCasas);
                Console.WriteLine("Texto original: " + textoCifrado);
                Console.WriteLine("Texto decifrado: "+ textDecifrado);
                Console.WriteLine("Hash SHA1: "+  ToSha1(textDecifrado).ToLower());
                Console.WriteLine("---------------- Fim ---------------");
            }
            catch (Exception e) {
                Console.WriteLine(e.ToString());
            }            
        }

        public static JObject GetFirstJson(string url) {
            var request = WebRequest.CreateHttp(url);
            request.Method = "GET";
            var resposta = request.GetResponse();
            var streamDados = resposta.GetResponseStream();
            StreamReader reader = new StreamReader(streamDados);
            object objResponse = reader.ReadToEnd();
            JObject json = JObject.Parse(objResponse.ToString());
            return json;
        }

        public static string ToDecript(char[] textoChar, string[] letra, int numeroCasas) {
            List<string> lista = new List<string>();
            string[] letrasNovas = new string[] { };
            for (int i = 0; i < textoChar.Length; i++)
            {
                int indice = ((int)Array.IndexOf(letra, textoChar[i].ToString().ToUpper()));
                int novoIndice = (indice + numeroCasas);

                if (textoChar[i].ToString().ToUpper() == " " || textoChar[i].ToString().ToUpper() == "," || textoChar[i].ToString().ToUpper() == "!")
                {
                    lista.Add(textoChar[i].ToString());
                    letrasNovas = lista.ToArray();
                }

                else if (novoIndice < 26)
                {
                    lista.Add(letra[novoIndice].ToString());
                    letrasNovas = lista.ToArray();
                }
                else
                {
                    int ii = novoIndice - 26;
                    lista.Add(letra[ii].ToString());
                    letrasNovas = lista.ToArray();
                }
            }
            string decifrado = string.Join("", letrasNovas).ToLower();
            return decifrado;
        }

        public static string ToSha1(string value)
        {
            var data = Encoding.ASCII.GetBytes(value);
            var hashData = new SHA1Managed().ComputeHash(data);
            var hash = string.Empty;
            foreach (var b in hashData)
            {
                hash += b.ToString("X2");
            }
            return hash;
        }

    }
}
