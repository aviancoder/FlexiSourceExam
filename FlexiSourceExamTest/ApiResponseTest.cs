using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RestSharp;
using RestSharp.Extensions;
using System.Net;

namespace FlexiSourceExamTest
{
    class ApiResponseTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestStatusCode()
        {
            // arrange
            RestClient client = new RestClient("https://localhost:44369/api/customerfiles/1001");
            RestRequest request = new RestRequest(Method.GET);

            // act
            IRestResponse response = client.Execute(request);

            // assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public void TestContentType()
        {
            // arrange
            RestClient client = new RestClient("https://localhost:44369/api/customerfiles/1001");
            RestRequest request = new RestRequest(Method.GET);

            // act
            IRestResponse response = client.Execute(request);

            // assert
            StringAssert.Contains("application/json", response.ContentType);
        }

        [Test]
        public void TestContent()
        {
            // arrange
            RestClient client = new RestClient("https://localhost:44369/api/customerfiles/1001");
            RestRequest request = new RestRequest(Method.GET);

            // act
            IRestResponse response = client.Execute(request);
            string content = response.Content;
            JObject jObject = JObject.Parse(content);

            // assert
            Assert.That(string.IsNullOrEmpty(jObject["errorMessage"].ToString()));
        }
    }
}
