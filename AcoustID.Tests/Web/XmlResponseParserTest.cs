
namespace AcoustID.Tests.Web
{
    using AcoustID.Web;
    using NUnit.Framework;
    using System.Net;

    public class XmlResponseParserTest
    {
#if TEST_LOCAL_FILES
        [Test]
        public void TestCanParse()
        {
            var xml = TestsHelper.LoadTextFile("lookup-simple.xml");

            var parser = new XmlResponseParser();
            var response = parser.CanParse(xml);

            Assert.AreEqual(response, true);
        }

        [Test]
        public void TestParseLookupResponse()
        {
            var xml = TestsHelper.LoadTextFile("lookup-simple.xml");

            var parser = new XmlResponseParser();
            var response = parser.ParseLookupResponse(xml);

            Assert.AreEqual(response.Results.Count, 3);

            var result = response.Results[0];

            Assert.IsTrue(result.Score > 0.0);
            Assert.IsFalse(string.IsNullOrEmpty(result.Id));
        }

        [Test]
        public void TestParseLookupResponseMeta1()
        {
            var xml = TestsHelper.LoadTextFile("lookup-recordings.xml");

            var parser = new XmlResponseParser();
            var response = parser.ParseLookupResponse(xml);

            Assert.AreEqual(response.Results.Count, 3);

            var recordings = response.Results[0].Recordings;

            Assert.IsNotNull(recordings);
            Assert.IsTrue(recordings.Count > 0);

            var recording = recordings[0];

            Assert.IsNotNull(recording);
            Assert.IsFalse(string.IsNullOrEmpty(recording.Id));
            Assert.IsFalse(string.IsNullOrEmpty(recording.Title));
            Assert.IsTrue(recording.Duration > 0);

            Assert.IsTrue(recording.Artists.Count > 0);

            var artist = recording.Artists[0];

            Assert.IsNotNull(artist);
            Assert.IsFalse(string.IsNullOrEmpty(artist.Id));
            Assert.IsFalse(string.IsNullOrEmpty(artist.Name));
        }

        [Test]
        public void TestParseLookupResponseMeta2()
        {
            var xml = TestsHelper.LoadTextFile("lookup-recordings-releasegroups.xml");

            var parser = new XmlResponseParser();
            var response = parser.ParseLookupResponse(xml);

            var recordings = response.Results[0].Recordings;

            Assert.IsNotNull(recordings);
            Assert.IsTrue(recordings.Count > 0);

            var recording = recordings[0];

            Assert.IsNotNull(recording);
            Assert.IsTrue(recording.ReleaseGroups.Count > 0);

            var releasegroup = recording.ReleaseGroups[0];

            Assert.IsNotNull(releasegroup);
            Assert.IsFalse(string.IsNullOrEmpty(releasegroup.Id));
            Assert.IsFalse(string.IsNullOrEmpty(releasegroup.Title));
        }

        [Test]
        public void TestParseLookupResponseError()
        {
            var xml = TestsHelper.LoadTextFile("lookup-error.xml");

            var parser = new XmlResponseParser();
            var response = parser.ParseLookupResponse(xml);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.BadRequest);
            Assert.IsFalse(string.IsNullOrEmpty(response.ErrorMessage));
        }
#endif
    }
}
