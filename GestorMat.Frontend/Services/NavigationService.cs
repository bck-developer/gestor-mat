using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace GestorMat.Frontend.Services
{
    public class NavigationService
    {
        private readonly NavigationManager _nav;
        private readonly IJSRuntime _js;

        public NavigationService(NavigationManager nav, IJSRuntime js)
        {
            _nav = nav;
            _js = js;
        }

        public string GetCurrentRoute()
        {
            var uri = _nav.Uri;
            var baseUri = _nav.BaseUri;

            return uri.StartsWith(baseUri)
                ? uri.Substring(baseUri.Length).Trim('/')
                : uri;
        }

        public void SubirNivel()
        {
            var ruta = GetCurrentRoute();

            if (string.IsNullOrEmpty(ruta))
            {
                _nav.NavigateTo("/");
                return;
            }

            var partes = ruta.Split('/', StringSplitOptions.RemoveEmptyEntries);

            if (partes.Length <= 1)
            {
                _nav.NavigateTo("/");
                return;
            }

            var nuevaRuta = string.Join("/", partes.Take(partes.Length - 1));
            _nav.NavigateTo("/" + nuevaRuta);
        }

        public List<string> ObtenerBreadcrumb()
        {
            var ruta = GetCurrentRoute();

            if (ruta.Contains("login"))
            {
                return new List<string> { "🔐 Login" };
            }

            var resultado = new List<string>
            {
                "🏠 Inicio"
            };

            if (string.IsNullOrEmpty(ruta))
                return resultado;

            var partes = ruta.Split('/', StringSplitOptions.RemoveEmptyEntries);

            foreach (var parte in partes)
            {
                if (int.TryParse(parte, out _))
                    continue;

                resultado.Add(TraducirRuta(parte));
            }

            return resultado;
        }

        private string TraducirRuta(string ruta)
        {
            const string crear = "➕ Crear";
            const string editar = "✏️ Editar";

            return ruta switch
            {
                "unidades" => "📏 Unidades",
                "crear-unidad" => crear,
                "editar-unidad" => editar,

                "materiales" => "📦 Materiales",
                "crear-material" => crear,
                "editar-material" => editar,

                "usuarios" => "👥 Usuarios",
                "crear-usuario" => crear,
                "editar-usuario" => editar,

                "depositos" => "🏭 Depósitos",
                "crear-deposito" => crear,
                "editar-deposito" => editar,

                "roles" => "🧩 Roles",
                "crear-rol" => crear,
                "editar-rol" => editar,

                "login" => "🔐 Login",

                _ => Capitalizar(ruta)
            };
        }

        private string Capitalizar(string texto) => string.IsNullOrEmpty(texto)
                ? texto
                : char.ToUpper(texto[0]) + texto.Substring(1);
    }
}