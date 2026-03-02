using System.Net;
using System.Text;

namespace Climaster_feature.Services
{
    public class LocalServerService
    {
        private HttpListener? _listener;
        private string _jsonContent = string.Empty;
        private const int Port = 8080;
        private CancellationTokenSource? _cancellationTokenSource;

        public async Task<string> StartServer(string jsonContent)
        {
            _jsonContent = jsonContent;
            
            // Find available port
            int port = Port;
            while (IsPortInUse(port))
            {
                port++;
            }

            _listener = new HttpListener();
            string localIp = GetLocalIPAddress();
            string prefix = $"http://{localIp}:{port}/";
            
            _listener.Prefixes.Add(prefix);
            
            try
            {
                _listener.Start();
            }
            catch (Exception ex)
            {
                throw new Exception($"No se pudo iniciar el servidor: {ex.Message}", ex);
            }

            // Create cancellation token
            _cancellationTokenSource = new CancellationTokenSource();

            // Start listening in background - DON'T await it!
            _ = HandleRequests(_cancellationTokenSource.Token);

            return $"{prefix}widget.json";
        }

        public void StopServer()
        {
            _cancellationTokenSource?.Cancel();
            _listener?.Stop();
            _listener?.Close();
            _listener = null;
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }

        private async Task HandleRequests(CancellationToken cancellationToken)
        {
            while (_listener != null && _listener.IsListening && !cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var context = await _listener.GetContextAsync();
                    
                    // Handle in background to not block the listener
                    _ = Task.Run(() => ProcessRequest(context), cancellationToken);
                }
                catch (HttpListenerException)
                {
                    // Server stopped
                    break;
                }
                catch (ObjectDisposedException)
                {
                    // Listener disposed
                    break;
                }
                catch (Exception)
                {
                    // Handle other errors silently
                }
            }
        }

        private void ProcessRequest(HttpListenerContext context)
        {
            try
            {
                var response = context.Response;

                // Set CORS headers
                response.AddHeader("Access-Control-Allow-Origin", "*");
                response.AddHeader("Access-Control-Allow-Methods", "GET");
                response.AddHeader("Access-Control-Allow-Headers", "Content-Type");
                response.ContentType = "application/json; charset=utf-8";

                byte[] buffer = Encoding.UTF8.GetBytes(_jsonContent);
                response.ContentLength64 = buffer.Length;
                response.StatusCode = 200;
                
                response.OutputStream.Write(buffer, 0, buffer.Length);
                response.OutputStream.Close();
            }
            catch (Exception)
            {
                // Ignore errors when processing individual requests
            }
        }

        private static bool IsPortInUse(int port)
        {
            try
            {
                using var listener = new HttpListener();
                listener.Prefixes.Add($"http://localhost:{port}/");
                listener.Start();
                listener.Stop();
                return false;
            }
            catch
            {
                return true;
            }
        }

        private static string GetLocalIPAddress()
        {
            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        return ip.ToString();
                    }
                }
            }
            catch
            {
                // Fallback to localhost
            }
            
            return "127.0.0.1";
        }
    }
}
