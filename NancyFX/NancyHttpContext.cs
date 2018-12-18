using System;
using System.IO;
using System.Text;
using System.Linq;
using ActiveQueryBuilder.Web.Server.Infrastructure;
using Nancy;

namespace NancyTest
{
    // Wrapper for Nancy.NancyContext
    public class NancyHttpContext: IHttpContext
    {
        private readonly NancyContext _context;
        public IHttpRequest Request { get; }
        public IHttpResponse Response { get; }

        public NancyHttpContext(NancyContext context)
        {
            _context = context;

            Request = new NancyHttpRequest(context.Request);

            context.Response = new Response();
            Response = new NancyHttpResponce(context.Response);
        }

        public string GetKey()
        {
            var sessionId = GetData("SessionID");

            if (!string.IsNullOrEmpty(sessionId))
                return sessionId;

            sessionId = Guid.NewGuid().ToString();
            SetData("SessionID", sessionId);

            return sessionId;
        }

        public int GetTimeout()
        {
            return 30;
        }

        public string GetUserLanguage()
        {
            return Request.GetHeader("Accept-Language");
        }

        public void SetData(string key, string value)
        {
            _context.Request.Session[key] = value;
        }

        public string GetData(string key)
        {
            return (string)_context.Request.Session[key];
        }
    }

    // Wrapper for Nancy.Request
    class NancyHttpRequest : AspNetHttpRequestBase
    {
        private Request _request;
        private string _body;

        public override string Body
        {
            get
            {
                if (_body == null)
                {
                    using (var reader = new StreamReader(_request.Body))
                        _body = reader.ReadToEnd();
                }

                return _body;
            }
        }

        public NancyHttpRequest(Request request): base(request.Path)
        {
            _request = request;
        }

        public override string GetHeader(string name)
        {
            return _request.Headers[name].First();
        }
    }

    // Wrapper for Nancy.Response
    public class NancyHttpResponce : IHttpResponse
    {
        private Response _response;

        public string ContentType
        {
            get { return _response.ContentType; }
            set { _response.ContentType = value; }
        }

        public int StatusCode
        {
            get { return (int)_response.StatusCode; }
            set { _response.StatusCode = (HttpStatusCode) value; }
        }

        public Stream Filter { get; set; }

        public NancyHttpResponce(Response response)
        {
            _response = response;
        }

        public void AppendHeader(string header, string value)
        {
            _response.Headers[header] = value;
        }

        public void Write(string data)
        {
            var bytes = Encoding.UTF8.GetBytes(data);
            BinaryWrite(bytes);
        }

        public void BinaryWrite(byte[] bytes)
        {
            _response.Contents = s =>
            {
                using (var helloStream = new MemoryStream(bytes))
                    helloStream.CopyTo(s);
            };
        }

        public void Clear()
        {
            
        }
    }
}