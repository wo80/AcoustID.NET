
namespace AcoustID.Tests.Web
{
    using AcoustID.Web;
    using NUnit.Framework;
    using System.Net;

    public class XmlResponseParserTest
    {
        [Test]
        public void TestCanParse()
        {
            var xml = TestsHelper.LoadEmbeddedResource("lookup-simple.xml");

            var parser = new XmlResponseParser();
            var response = parser.CanParse(xml);

            Assert.That(true, Is.EqualTo(response));
        }

        [Test]
        public void TestParseLookupResponse()
        {
            var xml = TestsHelper.LoadEmbeddedResource("lookup-simple.xml");

            var parser = new XmlResponseParser();
            var response = parser.ParseLookupResponse(xml);

            Assert.That(3, Is.EqualTo(response.Results.Count));

            var result = response.Results[0];

            Assert.That(result.Score, Is.GreaterThan(0.0));
            Assert.That(string.IsNullOrEmpty(result.Id), Is.False);
        }

        [Test]
        public void TestParseLookupResponseMeta1()
        {
            var xml = TestsHelper.LoadEmbeddedResource("lookup-recordings.xml");

            var parser = new XmlResponseParser();
            var response = parser.ParseLookupResponse(xml);

            Assert.That(3, Is.EqualTo(response.Results.Count));

            var recordings = response.Results[0].Recordings;

            Assert.That(recordings, Is.Not.Null);
            Assert.That(recordings.Count, Is.GreaterThan(0));

            var recording = recordings[0];

            Assert.That(recording, Is.Not.Null);
            Assert.That(string.IsNullOrEmpty(recording.Id), Is.False);
            Assert.That(string.IsNullOrEmpty(recording.Title), Is.False);
            Assert.That(recording.Duration, Is.GreaterThan(0));

            Assert.That(recording.Artists.Count, Is.GreaterThan(0));

            var artist = recording.Artists[0];

            Assert.That(artist, Is.Not.Null);
            Assert.That(string.IsNullOrEmpty(artist.Id), Is.False);
            Assert.That(string.IsNullOrEmpty(artist.Name), Is.False);
        }

        [Test]
        public void TestParseLookupResponseMeta2()
        {
            var xml = TestsHelper.LoadEmbeddedResource("lookup-recordings-releasegroups.xml");

            var parser = new XmlResponseParser();
            var response = parser.ParseLookupResponse(xml);

            var recordings = response.Results[0].Recordings;

            Assert.That(recordings, Is.Not.Null);
            Assert.That(recordings.Count, Is.GreaterThan(0));

            var recording = recordings[0];

            Assert.That(recording, Is.Not.Null);
            Assert.That(recording.ReleaseGroups.Count, Is.GreaterThan(0));

            var releasegroup = recording.ReleaseGroups[0];

            Assert.That(releasegroup, Is.Not.Null);
            Assert.That(string.IsNullOrEmpty(releasegroup.Id), Is.False);
            Assert.That(string.IsNullOrEmpty(releasegroup.Title), Is.False);
        }

        [Test]
        public void TestParseLookupResponseError()
        {
            var xml = TestsHelper.LoadEmbeddedResource("lookup-error.xml");

            var parser = new XmlResponseParser();
            var response = parser.ParseLookupResponse(xml);

            Assert.That(HttpStatusCode.BadRequest, Is.EqualTo(response.StatusCode));
            Assert.That(string.IsNullOrEmpty(response.ErrorMessage), Is.False);
        }
    }
}
