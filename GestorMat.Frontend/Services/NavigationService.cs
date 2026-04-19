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
            if (uri.StartsWith(baseUri))
                return uri.Substring(baseUri.Length).TrimStart('/');
            return uri;
        }

        public async Task Volver()
        {
            await _js.InvokeVoidAsync("history.go", -1);
        }

        public void IrHome()
        {
            _nav.NavigateTo("/");
        }
    }
}
