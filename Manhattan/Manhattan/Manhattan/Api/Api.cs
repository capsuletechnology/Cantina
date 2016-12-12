using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Manhattan.Api
{

    public static class Api
    {       
        #region GET
        public static async Task<List<Model.Cliente>> GetClientes()
        {
            using (var client = new HttpClient())
            {
                var json = await client.GetStringAsync("http://189.103.76.4:8080/WS_Manhattan/webresources/model.cliente");
                var clientes = JsonConvert.DeserializeObject<List<Model.Cliente>>(json);
                return clientes;
            }
        }

        public static async Task<List<Model.Produto>> GetProdutos()
        {
            using (var client = new HttpClient())
            {
                var json = await client.GetStringAsync("http://189.103.76.4:8080/WS_Manhattan/webresources/model.produto");
                var produtos = JsonConvert.DeserializeObject<List<Model.Produto>>(json);
                return produtos;
            }
        }

        public static async Task<List<Model.Pedido>> GetPedidos()
        {
            using (var client = new HttpClient())
            {
                var json = await client.GetStringAsync("http://189.103.76.4:8080/WS_Manhattan/webresources/model.pedido");
                var pedidos = JsonConvert.DeserializeObject<List<Model.Pedido>>(json);
                return pedidos;
            }
        }

        public static async Task<List<Model.PedidoProduto>> GetPedidoProduto()
        {
            using (var client = new HttpClient())
            {
                var json = await client.GetStringAsync("http://189.103.76.4:8080/WS_Manhattan/webresources/model.pedidoproduto");
                var lista = JsonConvert.DeserializeObject<List<Model.PedidoProduto>>(json);
                return lista;
            }
        }
        #endregion



        #region INSERT
        public static async void InsertCliente(Model.Cliente cliente)
        {
            var _cliente = new Model.Cliente
            {
                nome = cliente.nome,
                sobrenome = cliente.sobrenome,
                cpf = cliente.cpf,
                telefone = cliente.telefone,
                usuario = cliente.usuario,
                senha = cliente.senha,
                isadmin = false
            };
            string jsonInput = JsonConvert.SerializeObject(_cliente);

            HttpContent contentPost = new StringContent(jsonInput, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                var response = client.PostAsync("http://189.103.76.4:8080/WS_Manhattan/webresources/model.cliente/", contentPost).Result;
                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();
            }
        }

        public static async void InsertProduto(Model.Produto produto)
        {
            var _produto = new Model.Produto
            {
                nome = produto.nome,                
                tipo = produto.tipo,
                preco = produto.preco,
                qtdestoque = produto.qtdestoque,
                descricao = produto.descricao
            };
            string jsonInput = JsonConvert.SerializeObject(_produto);

            HttpContent contentPost = new StringContent(jsonInput, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                var response = client.PostAsync("http://189.103.76.4:8080/WS_Manhattan/webresources/model.produto/", contentPost).Result;
                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();
            }
        }

        public static async void InsertPedido(Model.Cliente cliente, double valorTotal, List<Model.ListaPedidoProduto> _lista)
        {
            var pedido = new Model.Pedido();
            var lista = new List<Model.Pedido>();

            var _pedido = new Model.Pedido
            {
                cliente = cliente,
                valortotal = valorTotal,
                qrcode = pedido.RandomQR(),
                data = DateTime.UtcNow.ToString("dd/MM/yyyy"),
                isfinalizado = false
            };            

            string jsonInput = JsonConvert.SerializeObject(_pedido);

            HttpContent contentPost = new StringContent(jsonInput, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                var response = client.PostAsync("http://189.103.76.4:8080/WS_Manhattan/webresources/model.pedido/", contentPost).Result;

                var json2 = await client.GetStringAsync("http://189.103.76.4:8080/WS_Manhattan/webresources/model.pedido/");
                lista = JsonConvert.DeserializeObject<List<Model.Pedido>>(json2);

                pedido = lista[lista.Count - 1];

                InsertPedidoProduto(pedido, _lista);
                 
                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();
            }           
        }

        public static async void InsertPedidoProduto(Model.Pedido pedido, List<Model.ListaPedidoProduto> _lista)
        {
            var _pedido = new Model.PedidoProduto();

            for (int i = 0; i < _lista.Count; i++)
            {
                _pedido.codpedido = pedido.codigo;
                _pedido.nome = _lista[i].produto.nome;
                _pedido.qtdrequisitada = _lista[i].qtdrequisitada;
                _pedido.valortotal = _lista[i].valortotal;            
           
                string jsonInput = JsonConvert.SerializeObject(_pedido);

                HttpContent contentPost = new StringContent(jsonInput, Encoding.UTF8, "application/json");

                using (var client = new HttpClient())
                {
                    var response = client.PostAsync("http://189.103.76.4:8080/WS_Manhattan/webresources/model.pedidoproduto/", contentPost).Result;

                    response.EnsureSuccessStatusCode();
                    string json = await response.Content.ReadAsStringAsync();
                }
            }
        }
        #endregion



        #region UPDATE
        public static void UpdateCliente(Model.Cliente cliente)
        {
            var _cliente = new Model.Cliente
            {
                codigo = cliente.codigo,
                nome = cliente.nome,
                sobrenome = cliente.sobrenome,
                cpf = cliente.cpf,
                telefone = cliente.telefone,
                usuario = cliente.usuario,
                senha = cliente.senha,
                isadmin = false
            };
            string jsonInput = JsonConvert.SerializeObject(_cliente);

            HttpContent contentPost = new StringContent(jsonInput, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                var response = client.PutAsync("http://189.103.76.4:8080/WS_Manhattan/webresources/model.cliente/" + _cliente.codigo, contentPost).Result;
            }
        }

        public static void UpdateProduto(Model.Produto produto)
        {
            var _produto = new Model.Produto
            {
                codigo = produto.codigo,
                nome = produto.nome,
                tipo = produto.tipo,
                preco = produto.preco,
                descricao = produto.descricao,
                qtdestoque = produto.qtdestoque
            };
            string jsonInput = JsonConvert.SerializeObject(_produto);

            HttpContent contentPost = new StringContent(jsonInput, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                var response = client.PutAsync("http://189.103.76.4:8080/WS_Manhattan/webresources/model.produto/" + _produto.codigo, contentPost).Result;
            }
        }


        public static void UpdateProduto(List<Model.ListaPedidoProduto> produto)
        {
            for (int i = 0; i < produto.Count; i++)
            {
                var _produto = produto[i].produto;

                _produto.qtdestoque -= produto[i].qtdrequisitada;

                string jsonInput = JsonConvert.SerializeObject(_produto);

                HttpContent contentPost = new StringContent(jsonInput, Encoding.UTF8, "application/json");

                using (var client = new HttpClient())
                {
                    var response = client.PutAsync("http://189.103.76.4:8080/WS_Manhattan/webresources/model.produto/" + _produto.codigo, contentPost).Result;
                }
            }            
        }

        public static void UpdatePedido(Model.Pedido pedido)
        {
            var _pedido = new Model.Pedido
            {
                codigo = pedido.codigo,
                cliente = pedido.cliente,
                valortotal = pedido.valortotal,
                qrcode = pedido.qrcode,
                data = pedido.data,
                isfinalizado = true
            };
            string jsonInput = JsonConvert.SerializeObject(_pedido);

            HttpContent contentPost = new StringContent(jsonInput, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                var response = client.PutAsync("http://189.103.76.4:8080/WS_Manhattan/webresources/model.pedido/" + _pedido.codigo, contentPost).Result;
            }
        }
        #endregion



        #region DELETE
        public static async void DeleteProduto(Model.Produto produto)
        {
            string jsonInput = JsonConvert.SerializeObject(produto);

            HttpContent contentPost = new StringContent(jsonInput, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                var response = client.DeleteAsync("http://189.103.76.4:8080/WS_Manhattan/webresources/model.produto/" + produto.codigo).Result;
                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();
            }
        }
        #endregion
    }
}
